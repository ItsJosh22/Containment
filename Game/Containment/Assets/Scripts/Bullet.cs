using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.VFX;
public class Bullet : NetworkBehaviour
{
    public VisualEffect v;
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whoCanDamage;

    [Range(0f,1f)]
    public float bounce;
    public bool useGravity;

    public int explosionDamage;
    public float explosionRange;

    public int maxCollisions;
    public float maxLifeTime;
    public bool explodeOnTouch = true;

    int collisons;
    PhysicMaterial physics_mat;

    Destroy dd;
    Vector3 temp;
    private void Start()
    {
        SetUp();
    }
    private void Update()
    {
        if (collisons > maxCollisions)
        {
            Explode();
        }
        maxLifeTime -= Time.deltaTime;
        if (maxLifeTime <= 0)
        {

            Explode();
        }

    }

    private void OnDestroy()
    {
       VisualEffect t = Instantiate(v, transform.position, Quaternion.identity);
        t.transform.up = temp;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("bullet"))
        {
            return;
        }
        collisons++;
        
        temp = collision.GetContact(0).normal;
        if (collision.collider.CompareTag("Enemy") && explodeOnTouch)
        {
            //collision.GetContact(0).normal

            Explode();
        }
    }

    
    void Explode()
    {
       
        if (v != null)
        {
            
            //Instantiate(explosion, transform.position, Quaternion.identity);
        }
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whoCanDamage);
        for (int i = 0; i < enemies.Length; i++)
        {
            //enemies[i].getcompoent<enemys or what everScrip>().TakeDamage(explosionDamage);
        }
        delay();
        //Invoke("delay", 0.05f);
    }
    void delay()
    {
        
       
        Destroy(gameObject);
    }
    void SetUp()
    {
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounce;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;
        GetComponent<SphereCollider>().material = physics_mat;

        rb.useGravity = useGravity;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, explosionRange);
    }
}
