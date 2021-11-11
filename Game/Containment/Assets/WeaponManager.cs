using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public int wepId;
    public string GunName;

    public GunLogic logic;
    public int slot;
    public GameObject rack;
    public bool taken;
    public int playerId;
     Vector3 startpos;
     Quaternion startRot;


    private void Start()
    {
        startpos = transform.position;
        startRot = transform.rotation;
        GameManager.instance.addGun(this, wepId);
        logic.enabled = false;
    }

    public void TakeWeapon(Transform _newPos,GameObject _shootOrigin)
    {
        taken = true;
       logic.enabled = true;
        logic.Viewpoint = _shootOrigin;

      
        transform.parent = _newPos;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        gameObject.SetActive(false);
       // ClientSend.PlayerPickedupGun(wepId, _shootOrigin.GetComponentInParent<PlayerManager>().id);

    }

    public int Slot()
    {
        return slot;
    }

    public void ReturnWep()
    {
        taken = false;
        logic.enabled = false;
        transform.parent = rack.transform;
        transform.position = startpos;
        transform.rotation = startRot;
        gameObject.SetActive(true);
      //  ClientSend.FreeWeapon(wepId);
    }


    public void ClientTakeWeapon(Transform _newPos)
    {
        taken = true;
        transform.parent = _newPos;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

    }
    
    



}
