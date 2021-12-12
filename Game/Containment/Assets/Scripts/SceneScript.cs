using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneScript : NetworkBehaviour
{
    public Text canvusStatusText;
    public PlayerScript playerScript;
    public SceneReference sceneReference;

    public Text ammoText;

    [SyncVar (hook = nameof(OnStatusTextChanged))]
    public string statusText;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    void OnStatusTextChanged(string _Old, string _New)
    {
        canvusStatusText.text = statusText;
    }

    public void ButtonSendMessage()
    {
        if (playerScript != null)
        {
            playerScript.cmdSendPlayerMessage();
        }
    }

    public void ButtonChangeScene()
    {
        if (isServer)
        {
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "Level1")
            {
                NetworkManager.singleton.ServerChangeScene("Level2");
            }
            else
            {
                NetworkManager.singleton.ServerChangeScene("Level1");
            }
        }
        else
        { 
            Debug.Log("You are not Host.");
        }
    }

    public void UIAmmo(string value)
    {
        ammoText.text = "Ammo: " + value;
    }

}
