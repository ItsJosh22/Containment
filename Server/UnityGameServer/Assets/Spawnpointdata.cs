using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Spawnpointdata : MonoBehaviour
{
    [SerializeField] GameObject[] currentspawns;
    [SerializeField] bool[] taken;
    [SerializeField] int[] id;
    public static Spawnpointdata instance;
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

      
    }


   


    private void Start()
    {
        if (currentspawns.Length == 0)
        {
            currentspawns = GameObject.FindGameObjectsWithTag("SpawnPoint");
            Array.Sort(currentspawns, SpawnpointSort);
            taken = new bool[currentspawns.Length];
            id = new int[currentspawns.Length];
        }
    }

    public void UpdateSpawnpoints()
    {
        for (int i = 0; i < currentspawns.Length; i++)
        {
            taken[i] = currentspawns[i].GetComponent<SpawnPoint>().taken;
            id[i] = currentspawns[i].GetComponent<SpawnPoint>().playerID;
        }
    }




    private void OnLevelWasLoaded(int level)
    {
        GameObject[] newSpawns = GameObject.FindGameObjectsWithTag("SpawnPoint");
        for (int i = 0; i < newSpawns.Length; i++)
        {
        Debug.Log(newSpawns[i].transform.position);

        }
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Array.Sort(newSpawns, SpawnpointSort);
        for (int i = 0; i < newSpawns.Length; i++)
        {
            newSpawns[i].GetComponent<SpawnPoint>().taken = taken[i];
            newSpawns[i].GetComponent<SpawnPoint>().playerID = id[i];
            
        }

        for (int i = 0; i < players.Length; i++)
        {
            for (int j = 0; j < newSpawns.Length; j++)
            {
                if (players[i].GetComponent<Player>().id == newSpawns[j].GetComponent<SpawnPoint>().playerID)
                {
                    players[i].GetComponent<Player>().spawnPoint = newSpawns[j];
                }
            }
        }

        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<Player>().MoveToSpawn();
        }

        currentspawns = newSpawns;

    }



    int SpawnpointSort(GameObject a, GameObject b)
    {
        return a.name.CompareTo(b.name);
    }
}
