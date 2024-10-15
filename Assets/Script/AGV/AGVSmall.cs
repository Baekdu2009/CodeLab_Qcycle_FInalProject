using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGVSmall : AGVControl
{
    public GameObject[] printerSet; // ������ ��Ʈ
    public GameObject[] hood;       // �ĵ�
    public List<PrinterCode> printers; // ������ �ڵ� ����Ʈ
    public List<Transform> printerLocation; // ������ ��ġ ���� ����Ʈ
    public List<Transform> hoodLocation;    // �ĵ� ��ġ ���� ����Ʈ
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
            else if (printers[i].isFinished && RobotArmOnAGV.printingObject == null) // �ĵ忡�� �۾� �� �����ͷ� ���ư��� ���
            {
                moveToHood = true; // �ĵ忡�� �̵��� �غ�
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
                MoveAlongPath();
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
        else if (moveToHood) // �ĵ�� �̵��ؾ� �ϴ� ���
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
            MoveAlongPath();
        }

        // �Ÿ� üũ�� ���� ���� Ȯ��
        if (GetDistanceToTarget(targetToMove) < 0.01f && RobotArmOnAGV.printingObject != null)
        {
            Destroy(RobotArmOnAGV.printingObject); // ������ ��ü �ı�
            targetToMove = null;
            moveToHood = false; // �ĵ� �̵� �Ϸ�
            isMoving = false;
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
