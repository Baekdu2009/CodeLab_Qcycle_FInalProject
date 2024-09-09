using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UIElements;

public class PrinterController : MonoBehaviour
{
    public GameObject printerNozzle; // 노즐 범위: (x, y, z) = (-0.5 ~ 0.5 , 0.6 ~ 1.15, -0.4 ~ 0.4)
    Transform nozzleOriginPos;
    public float nozzleSpeed = 1;

    [Range(-0.5f, 0.5f)]
    public float xPosRange; // x 범위
    [Range(0.6f, 1.15f)]
    public float yPosRange; // y 범위
    [Range(-0.4f, 0.4f)]
    public float zPosRange; // z 범위

    private Vector3 positionRange;
    private bool movingRight = true; // x 방향으로 이동하는 방향을 추적
    private bool movingForward = true; // z 방향으로 이동하는 방향을 추적

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

    // x 및 z 방향에 대해서 왕복 운동
    void NozzlexPosMoving()
    {
        Vector3 currentPosition = printerNozzle.transform.position;

        // y 방향은 고정
        currentPosition.y = positionRange.y;

        // x 방향 왕복 운동
        if (movingRight)
        {
            currentPosition.x += nozzleSpeed * Time.deltaTime; // 오른쪽으로 이동
            if (currentPosition.x >= positionRange.x) // 범위를 초과하면 방향 전환
            {
                movingRight = false;
            }
        }
        else
        {
            currentPosition.x -= nozzleSpeed * Time.deltaTime; // 왼쪽으로 이동
            if (currentPosition.x <= -positionRange.x) // 범위를 초과하면 방향 전환
            {
                movingRight = true;
            }
        }

        // z 방향 왕복 운동
        if (movingForward)
        {
            currentPosition.z += nozzleSpeed * Time.deltaTime; // 앞으로 이동
            if (currentPosition.z >= positionRange.z) // 범위를 초과하면 방향 전환
            {
                movingForward = false;
            }
        }
        else
        {
            currentPosition.z -= nozzleSpeed * Time.deltaTime; // 뒤로 이동
            if (currentPosition.z <= -positionRange.z) // 범위를 초과하면 방향 전환
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

    // 프린터 노즐이 입력된 범위 내에서 움직인다.
    //void NozzleMoving(GameObject obj)
    //{
        
    //    obj.transform.position = new Vector3(
    //        Mathf.Clamp(xPos, nozzlePosition[0], nozzlePosition[1]),
    //        Mathf.Clamp(yPos, nozzlePosition[2], nozzlePosition[3]),
    //        Mathf.Clamp(zPos, nozzlePosition[4], nozzlePosition[5])
    //        );

    //}
    // 프린터 노즐의 움직임 범위를 입력한다.
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
    //            Debug.LogError("입력된 범위가 유효하지 않습니다.");
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("입력된 값이 유효하지 않습니다.");
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
