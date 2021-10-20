using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartLevel : MonoBehaviour
{
    public string LevelName;
    public float Starttime;
    public float delay;
    bool entered = false;

    private void Start()
    {
        delay = Starttime;
    }

    private void Update()
    {
        if (entered == true)
        {
            delay -= Time.deltaTime;
        }
        if (delay <= 0)
        {
            ChangeLevel();
        }
    }

    void ChangeLevel()
    {
        SceneChanger.instance.Change(LevelName);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Enter");
        if (other.tag == "Player")
        {
            entered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit");
        if (other.tag == "Player")
        {
            delay = Starttime;
            entered = false;
        }
    }
}
