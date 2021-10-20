using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Player : MonoBehaviour
{
    public int id;
    public string username;
    public CharacterController controller;
    public float gravity = -9.81f;
    public float jumpspeed = 5f;
    public float moveSpeed = 5f;
    public float yVelo = 0;
    private bool[] inputs;
    public Transform shootOrigin;
    public float health;
    public float maxHealth = 100f;
    public int itemAmount = 0;
    public int maxItemAmount = 3;
    public float throwForce = 600f;
    public float BulletDamage = 50f;

    public GameObject[] Allpoints;
    public GameObject spawnPoint;
    [Header("Guns")]
    public int currentWep = 1;
    public GameObject[] Guns;
    int wepAmount = 0;
    bool running;
    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;
        inputs = new bool[5];
        wepAmount = Guns.Length;
        Allpoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        Array.Sort(Allpoints, SpawnpointSort);
        for (int i = 0; i < Allpoints.Length; i++)
        {
            if (Allpoints[i].GetComponent<SpawnPoint>().taken == false)
            {
                Allpoints[i].GetComponent<SpawnPoint>().taken = true;
                Allpoints[i].GetComponent<SpawnPoint>().playerID = id;
                spawnPoint = Allpoints[i];
                MoveToSpawn();
                Spawnpointdata.instance.UpdateSpawnpoints();
                break;
            }
        }
    }




    private void OnDestroy()
    {
        if (spawnPoint != null)
        {

        spawnPoint.GetComponent<SpawnPoint>().playerLeft();
        }
    }

    public void Start()
    {
        gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        moveSpeed *= Time.fixedDeltaTime;
        jumpspeed *= Time.fixedDeltaTime;
       
        
    }


    public void FixedUpdate()
    {
        
        if (health <= 0f)
        {
            return;
        }
        Vector2 _inputDirection = Vector2.zero;
        if (inputs[0])
        {
            _inputDirection.y += 1;
        }
        if (inputs[1])
        {
            _inputDirection.y -= 1;
        }
        if (inputs[2])
        {
            _inputDirection.x -= 1;
        }
        if (inputs[3])
        {
            _inputDirection.x += 1;
        }
        if (inputs[5])
        {
            running = true;
        }
        else
        {
            running = false;
        }

        Move(_inputDirection);

        for (int i = 1; i < wepAmount ; i++)
        {
            if (i == currentWep)
            {
                Guns[i].SetActive(true);
            }
            else
            {
                Guns[i].SetActive(false);
            }
        }

    }

    private void Move(Vector2 _inputDirection)
    {
   

        Vector3 _moveDirection = transform.right * _inputDirection.x + transform.forward * _inputDirection.y;
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
            if (inputs[4])
            {
                yVelo = jumpspeed;
            }
        }
        yVelo += gravity;
        _moveDirection.y = yVelo;
        controller.Move(_moveDirection);

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }

    public void SetInput(bool[] _inputs, Quaternion _rotation)
    {
        inputs = _inputs;
        transform.rotation = _rotation;
    }

    public void Shoot(Vector3 _viewDirection, int _bulletsPerPress)
    {
        if (health <= 0)
        {
            return;
        }
        Guns[currentWep].GetComponent<GunLogic>().bulletsShot = _bulletsPerPress;
        Guns[currentWep].GetComponent<GunLogic>().Shoot(_viewDirection);

       
    }

    public void Reload()
    {
        Guns[currentWep].GetComponent<GunLogic>().Reload();
    }

    public void ThrowItem(Vector3 _viewDirection)
    {
        if (health <= 0)
        {
            return;
        }

        if (itemAmount > 0)
        {
            itemAmount--;
            NetworkManager.instance.InstantiateProjectile(shootOrigin).Initialize(_viewDirection, throwForce, id);
        }

    }
    public void TakeDamage(float _damage)
    {
        if (health <= 0f)
        {
            return;
        }
        health -= _damage;
        if (health <= 0)
        {
            health = 0f;
            controller.enabled = false;
            transform.position = spawnPoint.transform.position;
            ServerSend.PlayerPosition(this);
            StartCoroutine(Respawn());
        }

        ServerSend.PlayerHealth(this);

    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3);
        health = maxHealth;
        controller.enabled = true;
        ServerSend.PlayerRespawned(this);
    }

    public bool AttemptPickupItem()
    {
        if (itemAmount >= maxItemAmount)
        {
            return false;
        }

        itemAmount++;
        return true;

    }

    public void SwapWeapon(bool _direction)
    {
        if (_direction)
        {
            currentWep += 1;
        }
        else
        {
            currentWep -= 1;
        }

        if (currentWep > wepAmount -1)
        {
            currentWep = 0;
        }

        if (currentWep < 0)
        {
            currentWep = wepAmount-1;
        }
        ServerSend.PlayerSwapWeapon(this);
    }

    public void MoveToSpawn()
    {
        controller.enabled = false;
  
        transform.position = spawnPoint.transform.position;
        controller.enabled = true;
    }

    int SpawnpointSort(GameObject a, GameObject b)
    {
        return a.name.CompareTo(b.name);
    }

}
