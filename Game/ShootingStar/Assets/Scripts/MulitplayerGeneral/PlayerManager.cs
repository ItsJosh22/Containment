using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public float Health;
    public float maxHealth;
    public int itemCount = 0;
    public MeshRenderer model;

    public void Initialize(int _id,string _username)
    {
        id = _id;
        username = _username;
        Health = maxHealth;
    }

    public void SetHealth(float _health)
    {
        Health = _health;
        if (Health <= 0f)
        {
            Die();
        }
    }


    public void Die()
    {
        model.enabled = false;
    }

    public void Respawn()
    {
        model.enabled = true;
        SetHealth(maxHealth);
    }

}
