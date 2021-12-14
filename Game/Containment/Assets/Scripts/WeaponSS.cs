using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class WeaponSS : MonoBehaviour
{
    public int Slot;
    public GameObject bullet;
    public GameObject muzzleFlash;
    public float shootForce;
    public float upwardForce;

    public float timeBetweenShooting = 1f;
    public float Spread;
    public float reloadTime;
    public float timeBetweenShots;
    public int magSize;
    public int bulletsPerTap;
    public int bulletsLeft;
    public int bulletsShot;

    public bool canChangeFireMode = true;
    public bool allowButtonHold;
    public bool readytoshoot = true;
    public bool reloading;
    public Transform muzzle;

    public VisualEffect v;



    private void Awake()
    {
        bulletsLeft = magSize;
    }

    

}
