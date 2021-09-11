using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayer : MonoBehaviour
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

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;
        //inputs = new bool[5];
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
        Move(_inputDirection);


        // WASD INPUTS SENT TO SERVER

    }

    private void Move(Vector2 _inputDirection)
    {


        Vector3 _moveDirection = transform.right * _inputDirection.x + transform.forward * _inputDirection.y;
        _moveDirection *= moveSpeed;

        if (controller.isGrounded)
        {
            yVelo = 0f;
            // JUMP BUTTON
            if ( Input.GetKey(KeyCode.Space))
            {
                yVelo = jumpspeed;
            }
        }
        yVelo += gravity;
        _moveDirection.y = yVelo;
        controller.Move(_moveDirection);

    }

    public void SetInput(bool[] _inputs, Quaternion _rotation)
    {
        inputs = _inputs;
        transform.rotation = _rotation;
    }

    public void Shoot(Vector3 _viewDirection)
    {
        if (health <= 0)
        {
            return;
        }

        if (Physics.Raycast(shootOrigin.position, _viewDirection, out RaycastHit _hit, 25f))
        {
            if (_hit.collider.CompareTag("Player"))
            {
                
                _hit.collider.GetComponent<SinglePlayer>().TakeDamage(50f);
            }


        }
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
            //NetworkManager.instance.InstantiateProjectile(shootOrigin).Initialize(_viewDirection, throwForce, id);
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
            transform.position = new Vector3(0, 10, 0);
          //  ServerSend.PlayerPosition(this);
            StartCoroutine(Respawn());
        }

       // ServerSend.PlayerHealth(this);

    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3);
        health = maxHealth;
        controller.enabled = true;
       // ServerSend.PlayerRespawned(this);
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
}
