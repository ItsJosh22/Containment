using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public bool taken = false;
    public int playerID;
    

    public void playerLeft()
    {
        taken = false;
        playerID = 0;
        Spawnpointdata.instance.UpdateSpawnpoints();
    }



}
