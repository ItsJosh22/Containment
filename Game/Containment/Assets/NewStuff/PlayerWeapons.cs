using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    public Player Player;




    public GameObject Primary;
    public GameObject Secondary;


    public GameObject CurrentWeapon;


    public bool Aiming = false;
    public RaycastHit hit;


    int select = 0;
    int maxweps = 2;
    public void Fire()
    {

        if (CheckWeapons())
            return;

        CurrentWeapon.GetComponent<GunLogic>().Inputs();

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
        CurrentWeapon.GetComponent<GunLogic>().GunReload();
    }

    public void SwapUp()
    {

        //if (Primary == null || Secondary == null)
        //{
        //    return;
        //}

        select += 1;
        if (select > maxweps)
        {
            select = 0;
        }

        swap();
    }

    public void SwapDown()
    {
        //if (Primary == null || Secondary == null)
        //{
        //    return;
        //}


        select -= 1;
        if (select < 0)
        {
            select = maxweps;
        }

        swap();

    }

    public void Firemode()
    {
        if (CheckWeapons())
            return;

        CurrentWeapon.GetComponent<GunLogic>().SwapFireMode();
    }

    bool CheckWeapons()
    {
        if (CurrentWeapon == null)
        {
            return true;
        }
        return false;
    }


    public void EquipWeapon(GameObject wep)
    {
        if (wep.GetComponent<WeaponManager>().isEquiped)
        {
            return;
        }



        switch (wep.GetComponent<WeaponManager>().slot)
        {
            case 0:

                break;
            case 1:
                // if there is no primary
                if (Primary == null)
                {
                    
                    wep.GetComponent<WeaponManager>().isEquiped = true;
                    wep.GetComponent<WeaponManager>().transform.parent = Player.GunHolder.transform;
                    wep.GetComponent<GunLogic>().Viewpoint = Player.shootOrigin.gameObject;
                    wep.transform.localPosition = Vector3.zero;
                    wep.transform.localRotation = Quaternion.identity;
                    wep.GetComponent<GunLogic>().enabled = true;
                }
                else
                {
                    //Dequip current wep
                    Primary.GetComponent<WeaponManager>().isEquiped = false;
                    Primary.GetComponent<GunLogic>().Viewpoint = null;
                    Primary.GetComponent<GunLogic>().enabled = false;


                    Primary.GetComponent<WeaponManager>().transform.parent = Primary.GetComponent<WeaponManager>().Returnpoint;
                    Primary.transform.localPosition = Vector3.zero;
                    Primary.transform.localRotation = Quaternion.identity;


                    //equip stuff
                    wep.GetComponent<WeaponManager>().isEquiped = true;
                    wep.GetComponent<WeaponManager>().transform.parent = Player.GunHolder.transform;
                    wep.GetComponent<GunLogic>().Viewpoint = Player.shootOrigin.gameObject;
                    wep.GetComponent<GunLogic>().enabled = true;
                    wep.transform.localPosition = Vector3.zero;
                    wep.transform.localRotation = Quaternion.identity;
                }
                Primary = wep;
                select = 1;

                break;
            case 2:
                // if there is no Secondary
                if (Secondary == null)
                {
                   
                    wep.GetComponent<WeaponManager>().isEquiped = true;
                    wep.GetComponent<WeaponManager>().transform.parent = Player.GunHolder.transform;
                    wep.GetComponent<GunLogic>().Viewpoint = Player.shootOrigin.gameObject;
                    wep.transform.localPosition = Vector3.zero;
                    wep.transform.localRotation = Quaternion.identity;
                    wep.GetComponent<GunLogic>().enabled = true;
                }
                else
                {
                    //Dequip current wep
                    Secondary.GetComponent<WeaponManager>().isEquiped = false;
                    Secondary.GetComponent<GunLogic>().Viewpoint = null;
                    Secondary.GetComponent<GunLogic>().enabled = false;

                    Secondary.GetComponent<WeaponManager>().transform.parent = Secondary.GetComponent<WeaponManager>().Returnpoint;
                    Secondary.transform.localPosition = Vector3.zero;
                    Secondary.transform.localRotation = Quaternion.identity;


                    //equip stuff
                    wep.GetComponent<WeaponManager>().isEquiped = true;
                    wep.GetComponent<WeaponManager>().transform.parent = Player.GunHolder.transform;
                    wep.GetComponent<GunLogic>().Viewpoint = Player.shootOrigin.gameObject;
                    wep.GetComponent<GunLogic>().enabled = true;
                    wep.transform.localPosition = Vector3.zero;
                    wep.transform.localRotation = Quaternion.identity;
                }
                Secondary = wep;
                select = 2;

                break;
            default:
                break;
        }

        swap();
    }

    void swap()
    {
        CurrentWeapon = null;


        switch (select)
        {
            case 0:
                if (Primary != null)
                    Primary.SetActive(false);
                if (Secondary != null)
                    Secondary.SetActive(false);
                break;
            case 1:
                CurrentWeapon = Primary;
                if (Primary != null)
                    Primary.SetActive(true);
                if (Secondary != null)
                    Secondary.SetActive(false);
                break;
            case 2:
                CurrentWeapon = Secondary;
                if (Primary != null)
                    Primary.SetActive(false);
                if (Secondary != null)
                    Secondary.SetActive(true);

                break;
            default:
                break;
        }
    }

}
