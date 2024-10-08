using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WireCutting : MonoBehaviour
{
    public GameObject wireCutting;
    public FilamentLine linemanager;
    
    public bool isWorking;
    public bool isProblem;

    float rotSpeed;

    private void Update()
    {
        CuttingRotate();
    }

    private void WorkingOn()
    {
        if (linemanager.LastPointArrive())
        {
            isWorking = true;
        }
        else
        {
            isWorking = false;
        }
    }

    private void CuttingRotate()
    {
        if (isWorking)
        {
            rotSpeed = 200f;
        }
        else
            rotSpeed = 0;
        
        float rotationAmount = -rotSpeed * Time.deltaTime;
        wireCutting.transform.Rotate(Vector3.up, rotationAmount);
    }
}
