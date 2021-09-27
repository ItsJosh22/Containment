using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRack : MonoBehaviour
{

    public GameObject Gun;


        
    public void EquipedGun()
    {
        Gun.SetActive(false);
    }



}
