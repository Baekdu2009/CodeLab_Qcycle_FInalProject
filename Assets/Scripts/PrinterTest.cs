using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UIElements;

public class PrinterController : MonoBehaviour
{
    public GameObject printerNozzle; // ���� ����: (x, y, z) = (-0.5 ~ 0.5 , 0.6 ~ 1.15, -0.4 ~ 0.4)
    Transform nozzleOriginPos;
    public float nozzleSpeed = 1;

    [Range(-0.5f, 0.5f)]
    public float xPosRange; // x ����
    [Range(0.6f, 1.15f)]
    public float yPosRange; // y ����
    [Range(-0.4f, 0.4f)]
    public float zPosRange; // z ����

    private Vector3 positionRange;
    private bool movingRight = true; // x �������� �̵��ϴ� ������ ����
    private bool movingForward = true; // z �������� �̵��ϴ� ������ ����

    void Start()
    {
        nozzleOriginPos = printerNozzle.transform;
        Debug.Log(nozzleOriginPos.position.ToString());
        positionRange = new Vector3(xPosRange, yPosRange, zPosRange);
    }

    void Update()
    {
        NozzlexPosMoving();
    }

    // x �� z ���⿡ ���ؼ� �պ� �
    void NozzlexPosMoving()
    {
        Vector3 currentPosition = printerNozzle.transform.position;

        // y ������ ����
        currentPosition.y = positionRange.y;

        // x ���� �պ� �
        if (movingRight)
        {
            currentPosition.x += nozzleSpeed * Time.deltaTime; // ���������� �̵�
            if (currentPosition.x >= positionRange.x) // ������ �ʰ��ϸ� ���� ��ȯ
            {
                movingRight = false;
            }
        }
        else
        {
            currentPosition.x -= nozzleSpeed * Time.deltaTime; // �������� �̵�
            if (currentPosition.x <= -positionRange.x) // ������ �ʰ��ϸ� ���� ��ȯ
            {
                movingRight = true;
            }
        }

        // z ���� �պ� �
        if (movingForward)
        {
            currentPosition.z += nozzleSpeed * Time.deltaTime; // ������ �̵�
            if (currentPosition.z >= positionRange.z) // ������ �ʰ��ϸ� ���� ��ȯ
            {
                movingForward = false;
            }
        }
        else
        {
            currentPosition.z -= nozzleSpeed * Time.deltaTime; // �ڷ� �̵�
            if (currentPosition.z <= -positionRange.z) // ������ �ʰ��ϸ� ���� ��ȯ
            {
                movingForward = true;
            }
        }
    }

    //public void OnBtnOperation()
    //{
    //    if (printerNozzle.transform.position == nozzleOriginPos.position)
    //    {
    //        NozzleMoving(printerNozzle);
    //    }
    //}

    // ������ ������ �Էµ� ���� ������ �����δ�.
    //void NozzleMoving(GameObject obj)
    //{
        
    //    obj.transform.position = new Vector3(
    //        Mathf.Clamp(xPos, nozzlePosition[0], nozzlePosition[1]),
    //        Mathf.Clamp(yPos, nozzlePosition[2], nozzlePosition[3]),
    //        Mathf.Clamp(zPos, nozzlePosition[4], nozzlePosition[5])
    //        );

    //}
    // ������ ������ ������ ������ �Է��Ѵ�.
    //void MovingPositionInput()
    //{
    //    if (float.TryParse(xPosMinInput.text, out float xMin) && float.TryParse(xPosMaxInput.text, out float xMax) &&
    //        float.TryParse(yPosMinInput.text, out float yMin) && float.TryParse(yPosMaxInput.text, out float yMax) &&
    //        float.TryParse(zPosMinInput.text, out float zMin) && float.TryParse(zPosMaxInput.text, out float zMax))
    //    {
    //        if (xMin < xMax && yMin < yMax && zMin < zMax)
    //        {
    //            nozzlePosition[0] = xMin;
    //            nozzlePosition[1] = xMax;
    //            nozzlePosition[2] = yMin;
    //            nozzlePosition[3] = yMax;
    //            nozzlePosition[4] = zMin;
    //            nozzlePosition[5] = zMax;
    //        }
    //        else
    //        {
    //            Debug.LogError("�Էµ� ������ ��ȿ���� �ʽ��ϴ�.");
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("�Էµ� ���� ��ȿ���� �ʽ��ϴ�.");
    //    }
    //}

    //void BtnControlMoving()
    //{
        
    //    if (Input.GetKey(KeyCode.UpArrow))
    //    {
    //        yPosRange += nozzleSpeed * Time.deltaTime;
    //    }
    //    else if (Input.GetKey(KeyCode.DownArrow))
    //    {
    //        yPosRange -= nozzleSpeed * Time.deltaTime;
    //    }
    //    if (Input.GetKey(KeyCode.W))
    //    {
    //        xPosRange -= nozzleSpeed * Time.deltaTime;
    //    }
    //    else if (Input.GetKey(KeyCode.S))
    //    {
    //        xPosRange += nozzleSpeed * Time.deltaTime;
    //    }
    //    if (Input.GetKey(KeyCode.A))
    //    {
    //        zPosRange -= nozzleSpeed * Time.deltaTime;
    //    }
    //    else if (Input.GetKey(KeyCode.D))
    //    {
    //        zPosRange += nozzleSpeed * Time.deltaTime;
    //    }
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        printerNozzle.transform.position = nozzleOriginPos.position;
    //    }

    //    printerNozzle.transform.position = new Vector3(xPosRange, yPosRange, zPosRange);
    //}
}
