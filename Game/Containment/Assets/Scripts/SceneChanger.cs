using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    public static SceneChanger instance;

    public string levelname;
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
  

    public void leveltoChange(string _levelname)
    {
        levelname = _levelname;
        PauseMenu.instance.StartChange();
    }

    public void Changing()
    {
        StartCoroutine(LoadScene());
        PauseMenu.instance.FinishChange();
    }

    private IEnumerator LoadScene()
    {

        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(levelname, LoadSceneMode.Single);

        while (!asyncLoadLevel.isDone)
            yield return null;

        yield return new WaitForEndOfFrame();
    }

}
