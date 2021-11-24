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
    public GameObject shootOrigin;
    [Header("Weapons")]
    [Range(0,1)]
    public int currentWep = 1;

    public GameObject Primary;
    public GameObject Secondary;

    public Transform gunPos;
    [Header("FlashLight")]
    public GameObject Flashlight;
    
    public void Initialize(int _id,string _username)
    {
        id = _id;
        username = _username;
        Health = maxHealth;
        
       
        if (GetComponent<OldPlayerController>() == true)
        {
            isLocal = true;
            
        }
        else
        {
            isLocal = false;
        }
        Primary = null;
        Secondary = null;
    }

    private void FixedUpdate()
    {

        if (Primary == null && Secondary == null)
        {
            return;
        }
        else
        {
            if (Primary != null && Secondary == null)
            {
                currentWep = 2;
            }
            if (Secondary != null && Primary == null)
            {
                currentWep = 3;
            }
        }
        switch (currentWep)
        {
            case 0:
                Primary.SetActive(true);
                Secondary.SetActive(false);
                break;
            case 1:
                Primary.SetActive(false );
                Secondary.SetActive(true);
                break;
            case 2:
                Primary.SetActive(true);
               
                break;
            case 3:
                
                Secondary.SetActive(true);
                break;
            default:
                break;
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

   
    public void EnableFlashLight()
    {
        Flashlight.SetActive(!Flashlight.activeSelf);
    }

   
}
