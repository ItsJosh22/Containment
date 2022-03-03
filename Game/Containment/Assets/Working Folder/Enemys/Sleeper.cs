using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Sleeper : BaseEnemy
{
    public SkinnedMeshRenderer skin;

    public PlayerScript ClosestPlayer;


    float distance = 99999999;
    int e;
    [SyncVar]
    public States curState;
    public float WakeUpTime = 10;
    public float WakeUpDistance = 10;
    public Transform head;
    public enum States
    {
        Idle,
        Wakeingup,
        Chase,
    }


    private void OnEnable()
    {
        curState = States.Idle;
        Health = MaxHealth;
    }



    private void Update()
    {
        //if (isServer == false)
        //{
        //    return;
        //}


        CheckPlayerDistances();

        switch (curState)
        {
            case States.Idle:
                skin.material.color = Color.green;
                break;
            case States.Wakeingup:
                skin.material.color = Color.yellow;
                WakeUpTime -= Time.fixedDeltaTime;
                if (ClosestPlayer == null)
                {
                    CheckPlayerDistances();
                }
                head.LookAt(ClosestPlayer.transform);





                if (WakeUpTime <= 0)
                {
                    curState = States.Chase;
                }
                break;
            case States.Chase:
                skin.material.color = Color.red;
                if (ClosestPlayer == null)
                {
                    CheckPlayerDistances();
                }
                head.LookAt(ClosestPlayer.transform);
                playerInAttackRange = Vector3.Distance(ClosestPlayer.transform.position, transform.position) < AttackRange;




                if (playerInAttackRange)
                {
                    AttackPlayer();
                }
                else
                {
                    ChasePlayer();
                }

                break;
            default:
                break;
        }
    }


    public void ChasePlayer()
    {
        transform.LookAt(ClosestPlayer.transform);
        agent.SetDestination(ClosestPlayer.transform.position);
        //chase();
    }

    //[Server]
    //public void chase()
    //{
    //    transform.LookAt(ClosestPlayer.transform);
    //    agent.SetDestination(ClosestPlayer.transform.position);
    //}

    public void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(ClosestPlayer.transform);

        if (!allreadyAttacked)
        {
            // melee code
            ClosestPlayer.cmdTakeDamge(Damage);

            allreadyAttacked = true;
            Invoke(nameof(resetAttack), timeBetweenAttacks);
        }
    }

    void resetAttack()
    {
        allreadyAttacked = false;
    }
    public override void TakeDamage(float dmg)
    {
        curState = States.Wakeingup;

        AwakenMasters();
        base.TakeDamage(dmg);
    }

    public void AwakenMasters()
    {
        if (curState == States.Chase)
        {
            return;
        }
        Collider[] NearEnemies = Physics.OverlapSphere(transform.position, WakeUpDistance);

        foreach (Collider c in NearEnemies)
        {
            if (c.TryGetComponent(out Sleeper s))
            {

                s.curState = States.Wakeingup;
            }
        }

    }

    public void CheckPlayerDistances()
    {
        for (int i = 0; i < PlayerList.Instance.players.Count; i++)
        {
            if (Vector3.Distance(PlayerList.Instance.players[i].transform.position, transform.position) < distance)
            {
                ClosestPlayer = PlayerList.Instance.players[i];
                distance = Vector3.Distance(PlayerList.Instance.players[i].transform.position, transform.position);
            }
        }
        if (ClosestPlayer != null)
        {

            distance = Vector3.Distance(ClosestPlayer.transform.position, transform.position);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, WakeUpDistance);
    }
}
