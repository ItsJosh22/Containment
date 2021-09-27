using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{

    public enum Primarys { Rifle, Shotgun }
    public enum Secondarys { Pistol, Knife }

    public Primarys CurrentPrimary;
    public Secondarys CurrentSecondary;

    [SerializeField] Transform cam;

    RaycastHit hit;

    private void FixedUpdate()
    {
        if (Physics.Raycast(cam.position,cam.TransformDirection(Vector3.forward),out hit, 5f))
        {
            if (hit.collider.tag == "Weapon Pickup")
            {

            }
            Debug.DrawRay(cam.position, cam.TransformDirection(Vector3.forward) * 5f, Color.yellow);
        }
    }



}
