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

    bool printerSignalInput;
    bool moveToHood;
    Transform targetToMove;
    RobotArmControl agvRobotArm;

    private void Start()
    {
        FindPrinterObject();
        HoodLocationSetting();
    }

    private void Update()
    {
        if (!printerSignalInput)
        {
            targetToMove = null;
        }

        PrinterSignalCheck();
        AGVtoPrinterMove();
    }

    private void PrinterSignalCheck()
    {
        for (int i = 0; i < printers.Count; i++)
        {
            if (printers[i].finishSignal)
            {
                printerSignalInput = true;
                targetToMove = printerLocation[i];
                isMoving = true;
                break; // ��ȣ�� ������ �̵� ����
            }
            else
            {
                printerSignalInput = false;
            }
        }
        
    }
    float AGVtoCartDistance()
    {
        return Vector3.Distance(transform.position, targetToMove.position);
    }

    private void AGVtoPrinterMove()
    {
        if (printerSignalInput)
        {
            AGVMove(targetToMove);

            if (GetDistanceToTarget(targetToMove) < 0.01f && IsFacingTarget(targetToMove))
            {
                
            }
        }
    }

    private void FindPrinterObject()
    {
        var printerObjects = FindObjectsByName("AGVLocate");
        foreach (GameObject obj in printerObjects)
        {
            foreach (Transform child in obj.transform)
            {
                if (!printerLocation.Contains(child)) // �ߺ� üũ
                {
                    printerLocation.Add(child); // Transform�� ����Ʈ�� �߰�
                }

                PrinterCode printerCode = child.GetComponent<PrinterCode>();
                if (printerCode != null && !printers.Contains(printerCode)) // �ߺ� üũ
                {
                    printers.Add(printerCode); // PrinterCode�� ����Ʈ�� �߰�
                }
            }
        }
    }

    private void HoodLocationSetting()
    {
        foreach (GameObject obj in hood)
        {
            foreach(Transform child in obj.transform)
            {
                if (child.name.Contains("Locate") && !hoodLocation.Contains(child))
                {
                    hoodLocation.Add(child);
                }
            }
        }
    }
}
