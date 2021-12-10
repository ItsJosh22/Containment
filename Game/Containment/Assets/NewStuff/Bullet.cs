using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Bullet : NetworkBehaviour
{
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

    private void OnCollisionEnter(Collision collision)
    {
        collisons++;
        if (collision.collider.CompareTag("Enemy") && explodeOnTouch)
        {
            Explode();
        }
    }

    
    void Explode()
    {
        Debug.Log("2");
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whoCanDamage);
        for (int i = 0; i < enemies.Length; i++)
        {
            //enemies[i].getcompoent<enemys or what everScrip>().TakeDamage(explosionDamage);
        }
        Invoke("delay", 0.05f);
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
