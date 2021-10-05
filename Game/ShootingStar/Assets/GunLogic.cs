using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GunLogic : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float damage;
    [SerializeField] float delay;
    [SerializeField] float spread;
    [SerializeField] float range;
    [SerializeField] float reloadtime;
    [SerializeField] float rateOfFire;
    public int magSize;
    [SerializeField] int bulletsPerPress;
    public int bulletsLeft;
    int bulletsShot;

    public bool allowButtonHold;
    bool reloading;
    bool shooting;
    bool canBeShot;

    public GameObject Viewpoint;
    public Transform bulletOrigin;
    public RaycastHit hit;
    public LayerMask Enemy;

    [Header("GameObjects")]
    public GameObject muzzleFlash;
    public GameObject bulletMark;
    public Text ammocount;
    public Image ReloadUI;
   
    private void Awake()
    {
        bulletsLeft = magSize;
        canBeShot = true;
        ammocount.text = $"{bulletsLeft} / {magSize}";
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
            ClientSend.PlayerReloaded();
        }

        if (canBeShot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerPress;
            Shoot();
            ClientSend.PlayerShoot(Viewpoint.transform.forward,bulletsPerPress);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            allowButtonHold = !allowButtonHold;
        }
    }

    void Shoot()
    {
        canBeShot = false;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        Vector3 direction = Viewpoint.transform.forward + new Vector3(0, y, x);


        // Used only for debug
        Physics.Raycast(Viewpoint.transform.position, direction, out hit, range);
        Debug.DrawLine(Viewpoint.transform.position, hit.point, Color.red,1000);
       
        
        // get object normal and make it face that
        // Instantiate(bulletMark, hit.point, Quaternion.Euler(0, 180, 0));
        // Instantiate(muzzleFlash, bulletOrigin.position,Quaternion.identity);

        bulletsLeft--;
        bulletsShot--;
        ammocount.text = $"{bulletsLeft} / {magSize}";
        Invoke("ResetShot", delay);

        if (bulletsShot > 0 && bulletsLeft > 0)
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
        ammocount.text = $"{bulletsLeft} / {magSize}";
    }

   

}
