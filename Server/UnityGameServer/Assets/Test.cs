using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Test : MonoBehaviour
{
    public static Test instance;

    public GameObject playerPrefab;
    public GameObject projectilePrefab;
    public GameObject enemyPrefab;
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

    
    private void OnApplicationQuit()
    {
        Server.Stop();

    }

    public Player InstantiatePlayer()
    {
        return Instantiate(playerPrefab, new Vector3(0f, 0, 0f), Quaternion.identity).GetComponent<Player>();
    }


    public Enemy InstantiateEnemy(Vector3 pos)
    {
        return Instantiate(enemyPrefab, pos, Quaternion.identity).GetComponent<Enemy>();
    }


    public Projectile InstantiateProjectile(Transform _shootOrigin)
    {
        return Instantiate(projectilePrefab, _shootOrigin.position + _shootOrigin.forward * 0.7f, Quaternion.identity).GetComponent<Projectile>();
    }



}
