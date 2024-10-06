using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CameraPlayerMove : MonoBehaviour
{

    public float moveSpeed;
    public float rotSpeed;
    public Camera characterCamera;

    float xRot;
    float yRot;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        CameraMove();
        CameraRotate();
    }

    private void CameraMove()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Vector3 direction = (transform.forward * v) + (transform.right * h);

        this.transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void CameraRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");

        xRot += mouseX * rotSpeed * Time.deltaTime;
        Mathf.Clamp(xRot, -90, 90);

        Quaternion rot = Quaternion.Euler(-yRot, xRot, 0);

        transform.rotation = rot;
    }
}
