using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    public PlayerScript player;
    public Camera cam;
    public float sensitivity = 100;
    public float clampangle = 85;

    private float verticalRotation;
    private float horizontalRotation;

    Portal[] portals;

    void Awake()
    {
        portals = FindObjectsOfType<Portal>();
    }


    private void Start()
    {
        
        verticalRotation = transform.localEulerAngles.x;
        horizontalRotation = player.transform.eulerAngles.y;
        ToggleCursorMode();
    }

    private void Update()
    {
        //if (!player.isLocalPlayer)
        //{
        //    return;
        //}
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorMode();
        }
        if (Cursor.lockState == CursorLockMode.Locked)
        {
        Look();

        }
       // Debug.DrawRay(transform.position, transform.forward * 2, Color.green);
    }

    private void Look()
    {
        float _mouseVertical = -Input.GetAxis("Mouse Y");
        float _mouseHorizontal = Input.GetAxis("Mouse X");

        verticalRotation += _mouseVertical * sensitivity * Time.deltaTime;
        horizontalRotation += _mouseHorizontal * sensitivity * Time.deltaTime;

        verticalRotation = Mathf.Clamp(verticalRotation, -clampangle, clampangle);

        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        player.transform.rotation = Quaternion.Euler(0f, horizontalRotation, 0f);

    }

    private void ToggleCursorMode()
    {
        Cursor.visible = !Cursor.visible;
        if (Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }


    void LateUpdate()
    {

        for (int i = 0; i < portals.Length; i++)
        {
            portals[i].PrePortalRender();
        }
        for (int i = 0; i < portals.Length; i++)
        {
            portals[i].Render();
        }

        for (int i = 0; i < portals.Length; i++)
        {
            portals[i].PostPortalRender();
        }

    }

}
