using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerWeapons : NetworkBehaviour
{
    public Player Player;

    public Transform[] Guns;

    public int CurrentWeapon = 0;


   // public GameObject CurrentWeapon;


    public bool Aiming = false;
    public RaycastHit hit;




   
   
    
    
    public void Fire()
    {

        if (CheckWeapons())
            return;

       // CurrentWeapon.GetComponent<GunLogic>().Inputs();

    }
    
    public void ADS()
    {
        if (CheckWeapons())
            return;

        Aiming = !Aiming;
    }

    
    public void Reload()
    {
        if (CheckWeapons())
            return;
        //CurrentWeapon.GetComponent<GunLogic>().GunReload();
    }
    
    public void SwapUp()
    {
        if (CurrentWeapon >= Guns.Length -1)
        {
            CurrentWeapon = 0;
        }
        else
        {
            CurrentWeapon++;
        }
        UpdateWeapons();
    }
    
    public void SwapDown()
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
    }
    
    public void Firemode()
    {
        if (CheckWeapons())
            return;

        //CurrentWeapon.GetComponent<GunLogic>().SwapFireMode();
    }

    bool CheckWeapons()
    {
        //if (CurrentWeapon == null)
        //{
        //    return true;
        //}
        return false;
    }

    
    void UpdateWeapons()
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



    public void EquipWeapon(GameObject wep)
    {
        if (Guns[wep.GetComponent<WeaponManager>().slot] != null)
        {
            Guns[wep.GetComponent<WeaponManager>().slot].gameObject.GetComponent<WeaponManager>().Dequip();
            Guns[wep.GetComponent<WeaponManager>().slot] = null;
        }

        Guns[wep.GetComponent<WeaponManager>().slot] = wep.transform;
        Guns[wep.GetComponent<WeaponManager>().slot].gameObject.GetComponent<WeaponManager>().Equip();
    }



    

}
