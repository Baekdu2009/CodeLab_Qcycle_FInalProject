using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ScrewBelt : MonoBehaviour
{
    public Transform filletPosition;


    [HideInInspector]
    public bool isWorking;
    float rotSpeed;

    private void Update()
    {
        RotateBelt();
    }

    private void RotateBelt()
    {
        if (isWorking)
        {
            rotSpeed = 200f;
        }
        else
        {
            rotSpeed = 0f;
        }
        transform.Rotate(Vector3.forward, rotSpeed * Time.deltaTime);
    }
}
