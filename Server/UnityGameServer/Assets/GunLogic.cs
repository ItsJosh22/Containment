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
    [SerializeField] int magSize;
    [SerializeField] int bulletsPerPress;
    [SerializeField] int bulletsLeft;
    public int bulletsShot = 6;

    public bool allowButtonHold;
    bool reloading;
    bool shooting;
    bool canBeShot;

    public GameObject Viewpoint;
    

    public RaycastHit hit;
  //  public LayerMask Enemy;

   // [Header("GameObjects")]
   // public GameObject muzzleFlash;
   // public GameObject bulletMark;
  //  public Text ammocount;
  //  public Image ReloadUI;

    private void Awake()
    {
        bulletsLeft = magSize;
        canBeShot = true;
        
    }

   



    

    public void Shoot(Vector3 _direction)
    {
        

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        Vector3 direction = _direction + new Vector3(x, y, x);

        if (Physics.Raycast(Viewpoint.transform.position, direction, out hit, range))
        {

            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<Enemy>().TakeDamage(damage);
            }
            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<Player>().TakeDamage(damage);
            }
        }
         Debug.DrawLine(Viewpoint.transform.position, hit.point, Color.red, 1000);
        // get object normal and make it face that
        ServerSend.Spawnimpact(hit.point, Quaternion.Euler(0, 0, 0));
        // Instantiate(bulletMark, hit.point, Quaternion.Euler(0, 180, 0));
        //  Instantiate(muzzleFlash, bulletOrigin.position,Quaternion.identity);

        bulletsLeft--;
        bulletsShot--;
        
        Invoke("ResetShot", delay);

        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            StartCoroutine(rof());
            // Invoke("Shoot", rateOfFire);
            Shoot(_direction);
        }


    }

    void ResetShot()
    {
        canBeShot = true;
    }

    public void Reload()
    {
        reloading = true;
        Invoke("Reloaded", reloadtime);
    }
    void Reloaded()
    {
        bulletsLeft = magSize;
        reloading = false;
      
    }

    IEnumerator rof()
    {
        yield return new WaitForSeconds(rateOfFire);
    }


}
