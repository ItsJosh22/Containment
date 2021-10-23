using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHolder : MonoBehaviour
{
    public Door[] doors;
    public static InteractionHolder instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }


    }
}
