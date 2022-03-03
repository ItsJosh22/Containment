using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Animations;
public class PlayerScript : NetworkBehaviour
{

    private SceneScript sceneScript;
    [Header("Player Info")]

    [SyncVar(hook = nameof(OnNameChanged))]
    public string playerName;
    public TextMesh playerNameText;

    [SyncVar(hook = nameof(OnColorChanged))]
    public Color playerColor;
    private Material playerMatclone;

    public GameObject floatingInfo;

    public float health = 100;


    [Header("Movement")]
    public CharacterController controller;
    public float walkSpeed = 5f;
    public float jumpForce = 10f;
    public float gravity = 18f;
    public float yVelo = 0;
    public bool running = false;
    public float smoothMoveTime = 0.1f;
    Camera cam;
    float verticalVelocity;
    Vector3 velocity;
    Vector3 smoothV;
    bool jumping;
    float lastGroundedTime;



    [Header("Weapons")]
    [SyncVar(hook = nameof(OnWeaponChanged))]
    public int activeWeaponSynced = 1;
    private int selectedWepLocal = 0;
    public GameObject[] wepArray;
    private WeaponSS activeWeapon;
    private float weaponCDTime;
    public Transform Gunorigins;

    bool shooting;

    Vector3 _spread;
    bool allowinvoke = true;
    GameObject g;
    [Header("Other")]
    public GameObject PlayerModel;
    public Animator ani;
    public float Range;
    public GameObject Flashlight;

    #region StarupStuff

    private void Awake()
    {
        PlayerList.Instance.Addplayer(this);
        sceneScript = GameObject.Find("SceneReference").GetComponent<SceneReference>().sceneScript;
        foreach (var item in wepArray)
        {
            if (item != null)
            {
                item.SetActive(false);
            }
        }

        if (selectedWepLocal < wepArray.Length && wepArray[selectedWepLocal] != null)
        {
            activeWeapon = wepArray[selectedWepLocal].GetComponent<WeaponSS>();
            sceneScript.UIAmmo($"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerTap} / {activeWeapon.magSize / activeWeapon.bulletsPerTap}");
        }
        else
        {
            activeWeapon = null;
            sceneScript.UIAmmo("None");
        }
    }

    public override void OnStartLocalPlayer()
    {
        cam = Camera.main;
        PlayerModel.layer = 1;
        SetLayerRecursively(PlayerModel, 1);
        sceneScript.playerScript = this;

        //camera
        cam.transform.SetParent(transform);
        cam.transform.localPosition = new Vector3(0, 0.6f, 0);
        cam.GetComponent<CameraController>().player = this;
        
        Gunorigins.SetParent(cam.transform);
        floatingInfo.transform.localPosition = new Vector3(0, 0.3f, 0.6f);
        floatingInfo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        string name = "Player" + Random.Range(100, 999);
        Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        cmdSetupPlayer(name, color);

       
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }



    [Command]
    public void cmdSetupPlayer(string _name, Color _color)
    {
        //info sent to server and severupdates syncvars
        playerName = _name;
        playerColor = _color;
        sceneScript.statusText = $"{playerName} joined.";
    }

    #endregion StarupStuff

    private void Update()
    {
        if (!isLocalPlayer)
        {
            floatingInfo.transform.LookAt(Camera.main.transform);
            return;
        }

        #region Movement
        ani.SetBool("Walking", false);
        ani.SetBool("Backwards", false);
        ani.SetBool("Running", false);
        Move();





        if (Input.GetKey(KeyCode.W))
        {
            ani.SetBool("Walking", true);
            
        }
        if (Input.GetKey(KeyCode.S))
        {
            ani.SetBool("Backwards", true);
            
        }
        if (Input.GetKey(KeyCode.A))
        {
            ani.SetBool("Walking", true);
            
        }
        if (Input.GetKey(KeyCode.D))
        {

            ani.SetBool("Walking", true);
            
        }

        //Running input
        if (Input.GetKey(KeyCode.LeftShift))
        {
            ani.SetBool("Running", true);
           
        }
        

       




        #endregion Movement

        #region WeaponKeys

        if (activeWeapon != null)
        {
            if (Input.GetKeyDown(KeyCode.V) && activeWeapon.canChangeFireMode == true)
            {
                activeWeapon.allowButtonHold = !activeWeapon.allowButtonHold;
            }
            if (activeWeapon.allowButtonHold)
            {
                shooting = Input.GetKey(KeyCode.Mouse0);
            }
            else
            {
                shooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            if (Input.GetKeyDown(KeyCode.R) && activeWeapon.bulletsLeft < activeWeapon.magSize && !activeWeapon.reloading)
            {
                Reload();
            }

            if (activeWeapon.readytoshoot && shooting && !activeWeapon.reloading && activeWeapon.bulletsLeft <= 0)
            {
                Reload();
            }

            if (activeWeapon.readytoshoot && shooting && !activeWeapon.reloading && activeWeapon.bulletsLeft > 0)
            {
                activeWeapon.bulletsShot = 0;
                ShootWeapon();
            }
        }

        //scroll up
        if (Input.mouseScrollDelta.y > 0)
        {
            selectedWepLocal += 1;
            if (selectedWepLocal > wepArray.Length - 1)
            {
                selectedWepLocal = 0;
                activeWeapon = null;
                sceneScript.UIAmmo("None");
            }

            cmdChangedActiveWeapon(selectedWepLocal);
        }
        //scroll down
        if (Input.mouseScrollDelta.y < 0)
        {
            selectedWepLocal -= 1;
            if (selectedWepLocal == 0)
            {
                activeWeapon = null;
                sceneScript.UIAmmo("None");
            }
            if (selectedWepLocal < 0)
            {
                selectedWepLocal = wepArray.Length - 1;
            }

            cmdChangedActiveWeapon(selectedWepLocal);
        }

        #endregion WeaponKeys

        if (Input.GetKeyDown(KeyCode.F) && Flashlight != null)
        {
            Flashlight.SetActive(!Flashlight.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {

            Interact();
        }

    }

    private void Move()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        Vector3 inputDir = new Vector3(input.x, 0, input.y).normalized;
        Vector3 worldInputDir = transform.TransformDirection(inputDir);

        float currentSpeed = (Input.GetKey(KeyCode.LeftShift)) ? (walkSpeed * 2) : walkSpeed;
        Vector3 targetVelocity = worldInputDir * currentSpeed;
        velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref smoothV, smoothMoveTime);

        verticalVelocity -= gravity * Time.deltaTime;
        velocity = new Vector3(velocity.x, verticalVelocity, velocity.z);

        var flags = controller.Move(velocity * Time.deltaTime);
        if (flags == CollisionFlags.Below)
        {
            jumping = false;
            lastGroundedTime = Time.time;
            verticalVelocity = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float timeSinceLastTouchedGround = Time.time - lastGroundedTime;
            if (controller.isGrounded || (!jumping && timeSinceLastTouchedGround < 0.15f))
            {
                jumping = true;
                verticalVelocity = jumpForce;
            }
        }

       

  

        //Vector3 _moveDirection = transform.right * _inputDirection.x + transform.forward * _inputDirection.y;
        ////Running      
        //if (running)
        //{
        //    _moveDirection *= (moveSpeed * 2);
        //}
        //else
        //{
        //    _moveDirection *= moveSpeed;
        //}

        //if (controller.isGrounded)
        //{
        //    yVelo = 0f;
        //    // JUMP BUTTON
        //    if (Input.GetKey(KeyCode.Space))
        //    {
        //        yVelo = jumpspeed;
        //    }
        //}
        //yVelo += gravity;
        //_moveDirection.y = yVelo;
        //controller.Move(_moveDirection);

    }

    #region Interact
    void Interact()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Range))
        {

            if (hit.collider.CompareTag("Weapon"))
            {

                g = hit.collider.transform.parent.gameObject;


            }
        }
    }




    #endregion Interact



    #region WeaponStuff

    void ShootWeapon()
    {
        activeWeapon.readytoshoot = false;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
       
        RaycastHit hit;
        Vector3 targetPoint;
        //if (Physics.Raycast(ray, out hit))
        //{
        //    targetPoint = hit.point;
        //}
        //else
        //{
            targetPoint = ray.GetPoint(75);
        //}
       
        Vector3 noSpread = targetPoint - activeWeapon.muzzle.position;
        float x = Random.Range(-activeWeapon.Spread, activeWeapon.Spread);
        float y = Random.Range(-activeWeapon.Spread, activeWeapon.Spread);

        Vector3 spread = noSpread + new Vector3(y, y, x);
        cmdSyncSpread(spread);
        activeWeapon.bulletsLeft--;
        activeWeapon.bulletsShot++;

        // instatiate bullet
        CmdShootRay();

        sceneScript.UIAmmo($"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerTap} / {activeWeapon.magSize / activeWeapon.bulletsPerTap}");

        if (allowinvoke)
        {
            Invoke("ResetShot", activeWeapon.timeBetweenShooting);
            allowinvoke = false;
        }

        if (activeWeapon.bulletsShot < activeWeapon.bulletsPerTap && activeWeapon.bulletsLeft > 0)
        {
            Invoke("ShootWeapon", activeWeapon.timeBetweenShots);
        }

    }

    [Command]
    void cmdSyncSpread(Vector3 v)
    {
        rpcSyncSpread(v);
    }
    [ClientRpc]
    void rpcSyncSpread(Vector3 v)
    {
        _spread = v;
    }

    void ResetShot()
    {
        activeWeapon.readytoshoot = true;
        allowinvoke = true;
    }

    void Reload()
    {
        activeWeapon.reloading = true;
        Invoke("ReloadFinished", activeWeapon.reloadTime);
        // do reload animation
        // cmd animations to everyplayer
    }
    void ReloadFinished()
    {
        activeWeapon.bulletsLeft = activeWeapon.magSize;
        activeWeapon.reloading = false;
        sceneScript.UIAmmo($"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerTap} / {activeWeapon.magSize / activeWeapon.bulletsPerTap}");
    }
    [Command]
    void CmdShootRay()
    {
        RpcFireWeapon();
    }

    [ClientRpc]
    void RpcFireWeapon()
    {
        //bulletAudio.Play(); muzzleflash  etc
        
        if (activeWeapon.v != null)
        {
            activeWeapon.v.Play();
            //Instantiate(activeWeapon.muzzleFlash, activeWeapon.muzzle.position, Quaternion.identity);
        }

        GameObject bullet = Instantiate(activeWeapon.bullet, activeWeapon.muzzle.position, Quaternion.identity);
        bullet.transform.forward = _spread;
        bullet.GetComponent<Rigidbody>().AddForce(_spread.normalized * activeWeapon.shootForce, ForceMode.Impulse);

        // if you want bounce
        // bullet.GetComponent<Rigidbody>().AddForce(Camera.main.transform.up * activeWeapon.upwardForce, ForceMode.Impulse);
    }
    void OnWeaponChanged(int _old, int _new)
    {
        // disable old
        if (0 < _old && _old < wepArray.Length && wepArray[_old] != null)
        {
            wepArray[_old].SetActive(false);
        }
        //enable new
        if (0 < _new && _new < wepArray.Length && wepArray[_new] != null)
        {
            
                wepArray[_new].SetActive(true);
                activeWeapon = wepArray[activeWeaponSynced].GetComponent<WeaponSS>();
                if (isLocalPlayer)
                {
                    sceneScript.UIAmmo($"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerTap} / {activeWeapon.magSize / activeWeapon.bulletsPerTap}");
                }
            
        }
    }

    [Command]
    public void cmdChangedActiveWeapon(int index)
    {
        activeWeaponSynced = index;
    }

    #endregion WeaponStuff

    #region otherStuff

    [Command]
    public void cmdSendPlayerMessage()
    {
        if (sceneScript)
        {
            sceneScript.statusText = $"{playerName} says hello {Random.Range(10, 99)}";
        }
    }

    void OnNameChanged(string _old, string _new)
    {
        playerNameText.text = playerName;
    }

    void OnColorChanged(Color _old, Color _new)
    {
        playerNameText.color = _new;
        playerMatclone = new Material(GetComponent<Renderer>().material);
        playerMatclone.color = _new;
        // GetComponent<Renderer>().material = playerMatclone;
    }

    #endregion otherStuff

    
 

    [Command]
    public void cmdTakeDamge(float f)
    {
        rpcTakeDamge(f);
    }

    [ClientRpc]
    public void rpcTakeDamge(float f)
    {
        health -= f;
    }

    public override void OnStopClient()
    {
        PlayerList.Instance.Removeplayer(this);
        base.OnStopClient();
    }

}
