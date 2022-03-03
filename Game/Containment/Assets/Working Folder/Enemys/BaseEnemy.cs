using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;
public class BaseEnemy : NetworkBehaviour
{
    public float MaxHealth;
    public float Health;
    public float Damage;
    public NavMeshAgent agent;
    public Vector3 SavedSpawnpoint;

    
    public float timeBetweenAttacks;
   protected bool allreadyAttacked;
    public float AttackRange;
    public bool playerInAttackRange;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        SavedSpawnpoint = transform.position;
        Health = MaxHealth;
    }
    

    public void ResetPos()
    {
        transform.position = SavedSpawnpoint;
        
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void TakeDamage(float dmg)
    {
        Health -= dmg;
        if (Health <= 0)
        {
            Death();
        }
    }
    public void Death()
    {
       gameObject.SetActive(false);
    }
}
