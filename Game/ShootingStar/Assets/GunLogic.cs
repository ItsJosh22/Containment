using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunLogic : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float damage;
    [SerializeField] float delay;
    [SerializeField] float spread;
    [SerializeField] float range;
    [SerializeField] float reloadtime;
    [SerializeField] float rateOfFire;
    [SerializeField] int magSize;
    [SerializeField] int bulletsPerPress;
    [SerializeField] int bulletsLeft;
    int bulletsShot;

    public bool allowButtonHold;
    bool reloading;
    bool shooting;
    bool canBeShot;

    public Camera camera;
    public Transform bulletOrigin;
    public RaycastHit hit;
    public LayerMask Enemy;

    [Header("GameObjects")]
    public GameObject muzzleFlash;
    public GameObject bulletMark;
    private void Awake()
    {
        bulletsLeft = magSize;
        canBeShot = true;
    }

    private void Update()
    {
        Inputs();
    
        //Add ui that sets mag size
    }



    void Inputs()
    {
        if (allowButtonHold)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magSize && !reloading)
        {
            Reload();
        }

        if (canBeShot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerPress;
            Shoot();
        }

    }

    void Shoot()
    {
        canBeShot = false;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        Vector3 direction = camera.transform.forward + new Vector3(x, y, 0);

        if (Physics.Raycast(camera.transform.position, direction, out hit, range, Enemy))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                //Call Enemies Damage func
            }
        }

      //  Instantiate(bulletMark, hit.point, Quaternion.Euler(0, 180, 0));
      //  Instantiate(muzzleFlash, bulletOrigin.position,Quaternion.identity);

        bulletsLeft--;
        bulletsShot--;
        Invoke("ResetShot", delay);

        if (bulletsShot >0 && bulletsLeft > 0)
        {
            Invoke("Shoot", rateOfFire);
        }


    }

    void ResetShot()
    {
        canBeShot = true;
    }

    void Reload()
    {
        reloading = true;
        Invoke("Reloaded", reloadtime);
    }
    void Reloaded()
    {
        bulletsLeft = magSize;
        reloading = false;
    }

}
