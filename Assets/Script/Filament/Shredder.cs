using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Shredder : MonoBehaviour
{
    public GameObject[] shafts;
    public bool isRunning;
    public bool isProblem = false;

    float rotSpeed;

    private void Update()
    {
        CuttingRotate();
    }

    private void CuttingRotate()
    {
        if (isRunning)
        {
            rotSpeed = 200f;
        }
        else
            rotSpeed = 0;

        float rotationAmount = rotSpeed * Time.deltaTime;

        shafts[0].transform.Rotate(Vector3.right, rotationAmount);
        shafts[1].transform.Rotate(Vector3.right, -rotationAmount);
    }
}
