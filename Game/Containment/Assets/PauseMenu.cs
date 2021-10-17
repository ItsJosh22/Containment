using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject PlayerUI;
    public GameObject Pausemenu;
    public GameObject OptionsMenu;
    public GameObject ServerWindow;

    
    private void Start()
    {
        ServerWindow.SetActive(false);
        OptionsMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (PlayerUI.activeInHierarchy == true)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                PlayerUI.SetActive( false);
                Pausemenu.SetActive(true);

            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                PlayerUI.SetActive(true);
                Pausemenu.SetActive(false);
                ServerWindow.SetActive(false);
                OptionsMenu.SetActive(false);
            }
        }
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        PlayerUI.SetActive(true);
        Pausemenu.SetActive(false);
        ServerWindow.SetActive(false);
        OptionsMenu.SetActive(false);
    }
    public void Options()
    {
        OptionsMenu.SetActive(true);
    }
    public void Server()
    {
        ServerWindow.SetActive(true);
    }
    public void Disconnect()
    {
        Client.instance.Disconnect();
       
        Destroy(this.transform.parent.gameObject);
    }    

}
