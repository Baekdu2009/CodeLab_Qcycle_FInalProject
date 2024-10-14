using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGVSmall : AGVControl
{
    public GameObject[] printerSet; // 프린터 세트
    public GameObject[] hood;       // 후드
    public List<PrinterCode> printers; // 프린터 코드 리스트
    public List<Transform> printerLocation; // 프린터 위치 저장 리스트
    public List<Transform> hoodLocation;    // 후드 위치 저장 리스트

    [HideInInspector]
    RobotArmOnAGV RobotArmOnAGV;
    public bool printerSignalInput;
    public bool printerLocationArrived;
    public PrinterCode targetPrinter;
    bool moveToHood;
    Transform targetToMove;

    private void Start()
    {
        RobotArmOnAGV = GetComponentInChildren<RobotArmOnAGV>();
        FindPrinterObject();
        HoodLocationSetting();
    }

    private void Update()
    {
        PrinterSignalCheck();
        AGVtoPrinterMove();
    }

    private void PrinterSignalCheck()
    {
        bool signalDetected = false; // 신호 감지 플래그

        for (int i = 0; i < printers.Count; i++)
        {
            if (printers[i].isFinished && !printerLocationArrived)
            {
                targetPrinter = printers[i];
                printerSignalInput = true;
                targetToMove = printerLocation[i];
                isMoving = true;
                signalDetected = true; // 신호 감지
                break; // 신호가 감지되면 루프 종료
            }
        }

        // 신호가 감지되지 않은 경우
        if (!signalDetected)
        {
            printerSignalInput = false;
            targetToMove = null; // targetToMove를 null로 설정하여 안전하게 처리
        }
    }

    private void PathToPrinter()
    {
        movingPositions.Clear();
        movingPositions.Add(this.transform);
        movingPositions.Add(targetToMove);
    }

    private void AGVtoPrinterMove()
    {
        if (printerSignalInput)
        {

            if (GetDistanceToTarget(targetToMove) > 0.01f)
            {
                PathToPrinter();
                MoveAlongPath();
            }
            else if (GetDistanceToTarget(targetToMove) < 0.01f)
            {
                print("도착");
                printerLocationArrived = true;

                if (targetPrinter != null)
                {
                    targetPrinter.isFinished = false;
                }
                printerSignalInput = false;
                
                RobotArmOnAGV.PullOutPrintingObject();
            }
        }
    }

    private void FindPrinterObject()
    {
        foreach (GameObject obj in printerSet)
        {
            foreach (Transform child in obj.transform)
            {
                if (child.name.Contains("PrinterLocate") && !printerLocation.Contains(child))
                {
                    printerLocation.Add(child);
                }

                PrinterCode printerCode = child.GetComponent<PrinterCode>();
                if (printerCode != null && !printers.Contains(printerCode))
                {
                    printers.Add(printerCode);
                }
            }
        }
    }

    private void HoodLocationSetting()
    {
        foreach (GameObject obj in hood)
        {
            foreach (Transform child in obj.transform)
            {
                if (child.name.Contains("Locate") && !hoodLocation.Contains(child))
                {
                    hoodLocation.Add(child);
                }
            }
        }
    }
}
