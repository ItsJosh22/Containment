using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int id;
    public float health;
    public float maxhealth = 100;

    public void Initialize(int _id)
    {
        id = _id;
        health = maxhealth;
    }
    public void SetHealth(float _health)
    {
        health = _health;
        if (health <= 0f)
        {
            GameManager.enemies.Remove(id);
            Destroy(gameObject);
        }
    }


}
