using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guninfo : MonoBehaviour
{
    public int playerId;
    public bool taken;
    public void TakeGun(int _id)
    {
        if (!taken)
        {
            //serversend
            playerId = _id;
            gameObject.SetActive(false);
        }
    }
    public void ReturnGun()
    {

    }
}
