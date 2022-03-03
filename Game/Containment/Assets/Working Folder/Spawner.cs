using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Spawner : NetworkBehaviour
{

    public float maxSpawns;


    [SyncVar]
    public bool once = true;
    public GameObject enemy;
    public List<GameObject> Enemies;
    // Update is called once per frame
  

    public override void OnStartServer()
    {
        base.OnStartServer();
        float x = transform.position.x;
        float z = transform.position.z;

        for (int i = 0; i < maxSpawns; i++)
        {
           float t1 = Random.Range(x - 10, x + 10);
            float t2 = Random.Range(z - 10, z + 10);
            int t = Random.Range(0, 360);
           
            spawn(t1, t2,t);

        }

        foreach  (GameObject g in Enemies)
        {
            g.transform.position = Vector3.zero;
        }

    }

    
    
    public void move()
    {
        if (once)
        {

        foreach (GameObject item in Enemies)
        {
            item.GetComponent<Sleeper>().ResetPos();
        }
            once = false;
        }

    }
    void spawn(float x, float z,int t)
    {
        
        GameObject e = Instantiate(enemy,new  Vector3(x, 0, z), Quaternion.identity);
        Enemies.Add(e);
        e.SetActive(true);
        NetworkServer.Spawn(e);
    }    

}
