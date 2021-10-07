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

    [Header("Weapons")]
    public int currentWep = 1;
    public GameObject[] Weapons;
    int wepAmount = 0;
    public void Initialize(int _id,string _username)
    {
        id = _id;
        username = _username;
        Health = maxHealth;
        wepAmount = Weapons.Length;
        foreach ( GameObject w in Weapons)
        {
            w.SetActive(false);
        }
        if (GetComponent<PlayerController>() == true)
        {
            isLocal = true;
            
        }
        else
        {
            isLocal = false;
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < wepAmount ; i++)
        {
            if (i == currentWep)
            {
                Weapons[i].SetActive(true);
            }
            else
            {
                Weapons[i].SetActive(false);
            }
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

    public void SwapWeapon(int CurrWep)
    {
        currentWep = CurrWep;
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
