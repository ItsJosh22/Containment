using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public GameObject Active;
    public GameObject[] GunHolder;

    private void Awake()
    {
        foreach (var item in GunHolder)
        {
            if (item != null)
            {
                item.SetActive(false);
            }
        }
    }

    public void PickedWeapon(GameObject g)
    {
        for (int i = 0; i < GunHolder.Length; i++)
        {
            if (GunHolder[i] == g)
            {
                Debug.Log("test");
            }
        }
        Debug.Log("Finished");
    }

}
