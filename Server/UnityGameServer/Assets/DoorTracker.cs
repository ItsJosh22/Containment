using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTracker : MonoBehaviour
{
    public int doorID;
    public bool activated;
    public GameObject door;
   


    public void doorInteracted(bool _active)
    {
        activated = _active;
        if (activated)
        {
            door.SetActive(false);
        }
        else
        {
            door.SetActive(true);
        }
        ServerSend.DoorInteracted(doorID,activated);


    }
   

}
