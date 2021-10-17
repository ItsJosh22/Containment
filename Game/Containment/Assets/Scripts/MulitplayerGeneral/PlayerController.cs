using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform camTransform;
    public PlayerManager pManager;

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //   // ClientSend.PlayerShoot(camTransform.forward);
        //}
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            // ClientSend.PlayerThrowItem(camTransform.forward);
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            ClientSend.PlayerSwapWeapon(false);

        }
        if (Input.mouseScrollDelta.y > 0)
        {
            ClientSend.PlayerSwapWeapon(true);

        }
        pManager.Weapons[pManager.currentWep].GetComponent<GunLogic>().ammocount.text = $"{pManager.Weapons[pManager.currentWep].GetComponent<GunLogic>().bulletsLeft} / {pManager.Weapons[pManager.currentWep].GetComponent<GunLogic>().magSize}";

    }

    private void FixedUpdate()
    {
        // Debug.Log("Sending inputs");
        if (Cursor.lockState == CursorLockMode.Locked)
        {
        SendInputToServer();

        }
    }

    void SendInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D),
            Input.GetKey(KeyCode.Space),
            Input.GetKey(KeyCode.LeftShift),

        };

        ClientSend.PlayerMovement(_inputs);
    }



}
