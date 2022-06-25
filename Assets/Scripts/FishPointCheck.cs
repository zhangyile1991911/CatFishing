using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishPointCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CatBase cb = other.GetComponent<CatBase>();
        if(cb != null)
        {
            cb.CurrentState = new CatBaseStateReady(cb);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CatBase cb = other.GetComponent<CatBase>();
        if (cb != null)
        {
            cb.CurrentState = new CatBaseStateIdle(cb);
        }
    }
}
