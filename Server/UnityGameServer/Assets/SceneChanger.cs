using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    public string levelname;

    public static SceneChanger instance;
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
    public void Change(string _levelname)
    {
        levelname = _levelname;
        ServerSend.LevelChanging(_levelname);
        StartCoroutine(LoadScene());
        ServerSend.ClientLevelChange(true);
    }


    private IEnumerator LoadScene()
    {
   
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(levelname);
        
        while (!asyncLoadLevel.isDone)
            yield return null;
        
        yield return new WaitForEndOfFrame();
    }
}
