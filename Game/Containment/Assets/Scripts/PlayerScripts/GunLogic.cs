using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
public class GunLogic : NetworkBehaviour
{
    
    [Header("Stats")]
    [SerializeField] float damage;
    [SerializeField] float delay;
    [SerializeField] float spread;
    [SerializeField] float range;
    [SerializeField] float reloadtime;
    [SerializeField] float rateOfFire;
    public int magSize;
    [SerializeField] int bulletsPerPress;
    [SyncVar] public int bulletsLeft;
    int bulletsShot;

    public bool allowButtonHold;
    bool reloading;
    bool shooting;
    bool canBeShot;

    public GameObject Viewpoint;
    public Transform bulletOrigin;
    public RaycastHit hit;
    public LayerMask Enemy;

    public Player player;
    [Header("GameObjects")]
    public GameObject muzzleFlash;
    public GameObject bulletMark;
    public Text ammocount;
    public Image ReloadUI;

    private void Awake()
    {
        bulletsLeft = magSize;
        canBeShot = true;
        
        // ammocount.text = $"{bulletsLeft} / {magSize}";
    }

    private void Update()
    {
        Inputs();


        //Add ui that sets mag size

    }



    public void Inputs()
    {
        if (player == null)
        {
            return;
        }

        if (!player.isLocalPlayer)
        {
            return;
        }
        if (allowButtonHold)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }


        if (canBeShot && shooting && !reloading && bulletsLeft > 0 && Cursor.lockState == CursorLockMode.Locked)
        {
            bulletsShot = bulletsPerPress;
            Shoot();
            // ClientSend.PlayerShoot(Viewpoint.transform.forward, bulletsPerPress);
        }
        //if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magSize && !reloading && Cursor.lockState == CursorLockMode.Locked)
        //{
        //    Reload();
        //    ClientSend.PlayerReloaded();
        //}

        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    allowButtonHold = !allowButtonHold;
        //}
    }

    public void GunFire()
    {
        if (canBeShot && shooting && !reloading && bulletsLeft > 0 && Cursor.lockState == CursorLockMode.Locked)
        {
            bulletsShot = bulletsPerPress;
            Shoot();
        }
    }

    public void SwapFireMode()
    {
        allowButtonHold = !allowButtonHold;
    }

  
    void Shoot()
    {
        canBeShot = false;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        Vector3 direction = Viewpoint.transform.forward + new Vector3(y, y, x);


        if (Physics.Raycast(Viewpoint.transform.position, direction, out hit, range))
        {


            if (hit.collider.CompareTag("Player"))
            {
                if (hit.collider.GetComponent<Player>().islocal == false)
                {
                    hit.collider.GetComponent<Player>().TakeDamage(damage);

                }
            }
        }

        // Used only for debug
        Debug.DrawLine(Viewpoint.transform.position, hit.point, Color.red, 1000);


        // get object normal and make it face that
        GameObject bullet = Instantiate(bulletMark, hit.point, Quaternion.Euler(0, 180, 0));
        //NetworkServer.Spawn(bullet);
        // Instantiate(muzzleFlash, bulletOrigin.position,Quaternion.identity);

        bulletsLeft--;
        bulletsShot--;
        //     ammocount.text = $"{bulletsLeft} / {magSize}";
        Invoke("ResetShot", delay);

        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Shoot", rateOfFire);
        }

        mshoot(direction);
    }

    [Command]
    void mshoot(Vector3 vec)
    {
        GameObject bullet = Instantiate(bulletMark, hit.point, Quaternion.Euler(0, 180, 0));
        NetworkServer.Spawn(bullet);
    }



    void ResetShot()
    {
        canBeShot = true;
    }


    void Reload()
    {
        reloading = true;
        Invoke("Reloaded", reloadtime);
    }
    void Reloaded()
    {
        bulletsLeft = magSize;
        reloading = false;
        // ammocount.text = $"{bulletsLeft} / {magSize}";
    }

    public void GunReload()
    {
        if (bulletsLeft < magSize && !reloading && Cursor.lockState == CursorLockMode.Locked)
        {
            Reload();

        }

    }


}
