using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegMovements : MonoBehaviour
{
    public Transform target;
    public Transform ForwardPoint;
    public Transform Backwardspoint;
    public float distance;
    public float distance2;
    public bool reversesStart;
    void Start()
    {
        if (reversesStart)
        {
           // target.position = Backwardspoint.position;
        }
        else
        {
            target.position = ForwardPoint.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(target.position, ForwardPoint.position);
        distance2 = Vector3.Distance(target.position, Backwardspoint.position);
        if (distance2 > 16)
        {
            target.position = Backwardspoint.position;
        }
        else if (distance > 16)
        {
            target.position = ForwardPoint.position;
        }
        
        

    }
}
