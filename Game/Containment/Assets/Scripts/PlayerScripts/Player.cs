using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    //private bool[] inputs;

    public Transform[] Guns;

    public int CurrentWeapon = 0;



    [Header("PlayerInfo")]
    public int id;
    public string username;
    public bool islocal;
    [Header("Health")]
  [SyncVar]  public float curHealth;
    public float maxHealth = 100f;

    [Header("Movement")]
    public CharacterController controller;
    public float gravity = -9.81f;
    public float jumpspeed = 5f;
    public float moveSpeed = 5f;
    public float yVelo = 0;
    public bool running = false;

    [Header("Weapons")]
    public PlayerWeapons Weapons;
    public Transform GunHolder;

    [Header("Shoot Origin")]
    public Transform shootOrigin;


    [Header("Flashlight")]
    public GameObject Flashlight;

    [Header("Interactions")]
    public Transform PickupOrigin;
    public float Range;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        curHealth = maxHealth;

    }


    public void Start()
    {
        curHealth = maxHealth;
        Application.targetFrameRate = 60;
        gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        moveSpeed *= Time.fixedDeltaTime;
        jumpspeed *= Time.fixedDeltaTime;
        UpdateWeapons();
    }


    public void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (curHealth <= 0f)
        {
            return;
        }

        #region movement

        Vector2 _inputDirection = Vector2.zero;
        // Normal Movement
        if (Input.GetKey(KeyCode.W))
        {
            _inputDirection.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _inputDirection.y -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            _inputDirection.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _inputDirection.x += 1;
        }

        //Running input
        if (Input.GetKey(KeyCode.LeftShift))
        {
            running = true;
        }
        else
        {
            running = false;
        }

        Move(_inputDirection);

        #endregion

        #region Weapons

        //Fire   INPUT CHECKED IN SCRIPT
        Weapons.Fire();

        // ADS,Zoom in
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Weapons.ADS();
        }
        // Reload
        if (Input.GetKeyDown(KeyCode.R))
        {
            Weapons.Reload();
        }
        //Scroll Up
        if (Input.mouseScrollDelta.y > 0)
        {
            if (CurrentWeapon >= Guns.Length - 1)
            {
                CurrentWeapon = 0;
            }
            else
            {
                CurrentWeapon++;
            }
            UpdateWeapons();
            // Weapons.SwapUp();
        }
        //Scroll Down
        if (Input.mouseScrollDelta.y < 0)
        {
            if (CurrentWeapon <= 0)
            {
                CurrentWeapon = Guns.Length - 1;
            }
            else
            {
                CurrentWeapon--;
            }
            UpdateWeapons();
            //Weapons.SwapDown();
        }
        //Change WeaponMode
        if (Input.GetKeyDown(KeyCode.V))
        {
            Weapons.Firemode();
        }

        #endregion
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(PickupOrigin.position, PickupOrigin.forward, out hit, Range))
            {
                if (hit.collider.CompareTag("Weapon"))
                {
                    
                    Weapons.EquipWeapon(hit.collider.gameObject);
                   
                }
            }

        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Flashlight.SetActive(!Flashlight.activeSelf);
        }

    }

    [Command]
    private void test()
    {
        Debug.LogError("test");
    }

    private void Move(Vector2 _inputDirection)
    {

        Vector3 _moveDirection = transform.right * _inputDirection.x + transform.forward * _inputDirection.y;
        //Running      
        if (running)
        {
            _moveDirection *= (moveSpeed * 2);
        }
        else
        {
            _moveDirection *= moveSpeed;
        }

        if (controller.isGrounded)
        {
            yVelo = 0f;
            // JUMP BUTTON
            if (Input.GetKey(KeyCode.Space))
            {
                yVelo = jumpspeed;
            }
        }
        yVelo += gravity;
        _moveDirection.y = yVelo;
        controller.Move(_moveDirection);



    }

    //public void Shoot(Vector3 _viewDirection)
    //{
    //    if (curHealth <= 0)
    //    {
    //        return;
    //    }

    //    if (Physics.Raycast(shootOrigin.position, _viewDirection, out RaycastHit _hit, 25f))
    //    {
    //        if (_hit.collider.CompareTag("Player"))
    //        {

    //            _hit.collider.GetComponent<Player>().TakeDamage(50f);
    //        }


    //    }
    //}


    public void TakeDamage(float _damage)
    {
        if (curHealth <= 0f)
        {
            return;
        }
        curHealth -= _damage;
        if (curHealth <= 0)
        {
            curHealth = 0f;
            controller.enabled = false;
            //PlayerSpawnpoint
            transform.position = new Vector3(0, 10, 0);
            //  ServerSend.PlayerPosition(this);
            StartCoroutine(Respawn());
        }

        // ServerSend.PlayerHealth(this);

    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3);
        curHealth = maxHealth;
        controller.enabled = true;
        // ServerSend.PlayerRespawned(this);
    }



    void UpdateWeapons()
    {
        //int i = 0;
        //foreach (Transform wep in Guns)
        //{
        //    if (i == CurrentWeapon)
        //    {
        //        wep.gameObject.SetActive(true);
        //    }
        //    else
        //    {
        //        wep.gameObject.SetActive(false);
        //    }
        //    i++;
        //}

        if (isServer)
        {
            RpcUpdateWeapons();
        }
        else
        {
            cmdUpdateWeapons();

        }
    }

    [Command]
    public void cmdUpdateWeapons()
    {
        RpcUpdateWeapons();
    }


    [ClientRpc]
    public void RpcUpdateWeapons()
    {
        int i = 0;
        foreach (Transform wep in Guns)
        {
            if (i == CurrentWeapon)
            {
                wep.gameObject.SetActive(true);
            }
            else
            {
                wep.gameObject.SetActive(false);
            }
            i++;
        }
    }



}
