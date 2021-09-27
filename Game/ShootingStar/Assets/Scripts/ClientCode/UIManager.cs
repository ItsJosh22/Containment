using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public InputField usernameField;


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

    public void ConnectToLocal()
    {
       // Client.instance.ip = "127.0.0.1";
        Debug.Log($"Trying to connect to {Client.instance.ip}");
        startMenu.SetActive(false);
        usernameField.interactable = false;
        Client.instance.ConnectToServer();
    
    }

    public void ConnectToServer()
    {
        //Client.instance.ip = "24.170.248.139";
        Debug.Log($"Trying to connect to {Client.instance.ip}");
        startMenu.SetActive(false);
        usernameField.interactable = false;
        Client.instance.ConnectToServer();

    }

}
