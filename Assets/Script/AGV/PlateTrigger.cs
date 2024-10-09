using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlateTrigger : MonoBehaviour
{
    private AGVCart agvCart;

    private void Start()
    {
        agvCart = GetComponentInParent<AGVCart>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Box")
        {
            agvCart.IncrementColliderCount();
        }
    }
}
