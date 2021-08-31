using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public float Health;
    public float maxHealth;
    public int itemCount = 0;
    public MeshRenderer model;
    public bool isLocal;
    public Scrollbar Healthbar;
    public void Initialize(int _id,string _username)
    {
        id = _id;
        username = _username;
        Health = maxHealth;
        if (GetComponent<PlayerController>() == true)
        {
            isLocal = true;
            
        }
        else
        {
            isLocal = false;
        }
    }

    public void SetHealth(float _health)
    {

        Health = _health;
        if (Health <= 0f)
        {
            Die();
        }
        if (isLocal == true)
        {
            Healthbar.size = Health / 100;
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
