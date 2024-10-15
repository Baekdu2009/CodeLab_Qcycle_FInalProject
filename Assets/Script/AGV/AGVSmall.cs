using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AGVSmall : AGVControl
{
    public GameObject[] printerSet; // 프린터 세트
    public GameObject[] hood;       // 후드
    public List<PrinterCode> printers; // 프린터 코드 리스트
    public List<Transform> printerLocation; // 프린터 위치 저장 리스트
    public List<Transform> hoodLocation;    // 후드 위치 저장 리스트
    public Image movingCheck;
    public TMP_Text movingTxt;
    Transform initialPos;

    [HideInInspector]
    RobotArmOnAGV RobotArmOnAGV;
    public bool printerSignalInput;
    public bool printerLocationArrived;
    public bool originMove;
    public PrinterCode targetPrinter;
    bool moveToHood;
    Transform targetToMove;

    private void Start()
    {
        initialPos = transform;
        RobotArmOnAGV = GetComponentInChildren<RobotArmOnAGV>();
        FindPrinterObject();
        HoodLocationSetting();
    }

    private void Update()
    {
        PrinterSignalCheck();
        AGVtoPrinterMove();
        AGVRobotUIUpdate();
    }

    private void PrinterSignalCheck()
    {
        bool signalDetected = false;

        for (int i = 0; i < printers.Count; i++)
        {
            if (printers[i].isFinished && !isMoving)
            {
                targetPrinter = printers[i];
                printerSignalInput = true;
                targetToMove = printerLocation[i];
                isMoving = true;
                signalDetected = true;
                break;
            }
            else if (printers[i].isFinished && RobotArmOnAGV.printingObject == null) // 후드에서 작업 후 프린터로 돌아가는 경우
            {
                moveToHood = true; // 후드에서 이동할 준비
                signalDetected = true;
                break;
            }
        }

        if (!signalDetected)
        {
            printerSignalInput = false;
            targetToMove = null;
        }
    }

    private void PathToTarget(Transform movePosition)
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
                isMoving = true;
                PathToTarget(targetToMove);
                MovebyPath();
            }
            else if (GetDistanceToTarget(targetToMove) < 0.01f)
            {
                printerLocationArrived = true;
                isMoving = false;

                if (targetPrinter != null)
                {
                    targetPrinter.isFinished = false;
                }
                printerSignalInput = false;

                RobotArmOnAGV.PullOutPrintingObject();
            }
        }
        else if (moveToHood) // 후드로 이동해야 하는 경우
        {
            MoveToHood();
        }
    }

    private void MoveToHood()
    {
        if (RobotArmOnAGV.printingObject != null)
        {
            isMoving = true;
            printerLocationArrived = false;
            targetToMove = hoodLocation[0];
            PathToTarget(targetToMove);
            MovebyPath();
        }

        // 거리 체크로 도착 여부 확인
        if (GetDistanceToTarget(targetToMove) < 0.01f && RobotArmOnAGV.printingObject != null)
        {
            Destroy(RobotArmOnAGV.printingObject); // 프린팅 객체 파괴
            targetToMove = null;
            moveToHood = false; // 후드 이동 완료
            isMoving = false;
        }
    }

    private void AGVRobotUIUpdate()
    {
        if (isMoving)
        {
            movingCheck.color = Color.green;
            movingTxt.text = "Moving";
            movingTxt.color = Color.green;
        }
        else
        {
            movingCheck.color = Color.yellow;
            movingTxt.text = "Stand By";
            movingTxt.color = Color.yellow;
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