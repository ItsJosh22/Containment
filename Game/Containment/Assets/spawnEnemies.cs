using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnEnemies : MonoBehaviour
{
    public Spawner s;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            
            s.move();
        }
    }
}
