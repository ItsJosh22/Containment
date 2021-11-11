using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookInteraction : MonoBehaviour
{
    public PlayerManager p;
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
                if (hit.collider.isTrigger && hit.collider.TryGetComponent(out WeaponManager _wep))
                {
                    int s = _wep.Slot();

                    switch (s)
                    {
                        case 0:
                            if (_wep.taken != true)
                            {
                                if (p.Primary == null)
                                {

                                    p.Primary = _wep.gameObject;
                                    _wep.TakeWeapon(p.gunPos, p.shootOrigin);
                                }
                                else if (p.Primary.GetComponent<WeaponManager>().GunName != _wep.GunName)
                                {
                                    p.Primary.GetComponent<WeaponManager>().ReturnWep();
                                    p.Primary = _wep.gameObject;
                                    _wep.TakeWeapon(p.gunPos, p.shootOrigin);
                                }

                            }
                            break;
                        case 1:
                            if (_wep.taken != true)
                            {
                                if (p.Secondary == null)
                                {

                                    p.Secondary = _wep.gameObject;
                                    _wep.TakeWeapon(p.gunPos, p.shootOrigin);
                                }
                                else if (p.Secondary.GetComponent<WeaponManager>().GunName != _wep.GunName)
                                {
                                    p.Secondary.GetComponent<WeaponManager>().ReturnWep();
                                    p.Secondary = _wep.gameObject;
                                    _wep.TakeWeapon(p.gunPos, p.shootOrigin);
                                }

                            }
                            break;
                        default:
                            break;
                    }


                }
            }
        }



    }





}
