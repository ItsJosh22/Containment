using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform camTransform;
   

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ClientSend.PlayerShoot(camTransform.forward);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            ClientSend.PlayerThrowItem(camTransform.forward);
        }
    }

    private void FixedUpdate()
    {
        Debug.Log("Sending inputs");
        
        SendInputToServer();
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
        };
        Debug.Log(_inputs[0]);
        Debug.Log(_inputs[1]);
        Debug.Log(_inputs[2]);
        Debug.Log(_inputs[3]);
        ClientSend.PlayerMovement(_inputs);
    }

    

}
