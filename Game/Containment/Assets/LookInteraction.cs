using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookInteraction : MonoBehaviour
{
    public Transform origin;
    [Range(0, 5)]
    public float range;
    RaycastHit hit;

 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            
            if (Physics.Raycast(origin.position, origin.forward, out hit, range))
            {
                if (hit.collider.isTrigger && hit.collider.TryGetComponent(out Door _door))
                {
                  
                   _door.Interact();
                }
                if (hit.collider.isTrigger && hit.collider.TryGetComponent(out Button _button))
                {

                }
            }
        }

       
    }
}
