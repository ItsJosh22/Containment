using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool canBeOpened;
    public bool opened;
    public int doorID;
    public GameObject t1;

    public void Interact()
    {
        if (canBeOpened)
        {
            // play door sound
            if (!opened)
            {
                opened = true;
                t1.SetActive(false);
                // play opening door animation
                ClientSend.DoorInteracted(doorID, opened);
                Debug.Log("REEEEEE");
            }
            else
            {
                opened = false;
                t1.SetActive(true);
                // play closeing door animation
                ClientSend.DoorInteracted(doorID, opened);
                Debug.Log("no");
            }
        }
        else
        {
            // Play sound / say locked
        }
    }

    public void Unlocked()
    {
        canBeOpened = true;
    }
    public void Locked()
    {
        canBeOpened = false;
    }

    public void DoorInter(bool open)
    {
        if (open)
        {
            t1.gameObject.SetActive(false);
            
        }
        else
        {
            t1.gameObject.SetActive(true);
        }

    }

}
