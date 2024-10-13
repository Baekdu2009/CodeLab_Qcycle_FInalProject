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
        bool signalDetected = false; // ��ȣ ���� �÷���

        for (int i = 0; i < printers.Count; i++)
        {
            if (printers[i].isFinished)
            {
                targetPrinter = printers[i];
                printerSignalInput = true;
                targetToMove = printerLocation[i];
                isMoving = true;
                signalDetected = true; // ��ȣ ����
                break; // ��ȣ�� �����Ǹ� ���� ����
            }
        }

        // ��ȣ�� �������� ���� ���
        if (!signalDetected)
        {
            printerSignalInput = false;
            targetToMove = null; // targetToMove�� null�� �����Ͽ� �����ϰ� ó��
        }
    }


    float CalculateDistance()
    {
        return Vector3.Distance(transform.position, targetToMove.position);
    }

    private void AGVtoPrinterMove()
    {
        if (printerSignalInput)
        {
            if (GetDistanceToTarget(targetToMove) > 0.01f || !IsFacingTarget(targetToMove))
            {
                AGVMove(targetToMove);
            }
            else if (GetDistanceToTarget(targetToMove) < 0.01f && IsFacingTarget(targetToMove))
            {
                printerLocationArrived = true;
                // �߰�: �κ� �ȿ� ��ü ������ ��û
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
