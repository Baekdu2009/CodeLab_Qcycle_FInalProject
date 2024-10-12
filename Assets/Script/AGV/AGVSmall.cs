using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGVSmall : AGVControl
{
    public GameObject[] printerSet; // ������ ��Ʈ
    public List<PrinterCode> printers; // ������ �ڵ� ����Ʈ
    public List<Transform> printerLocation; // ������ ��ġ ���� ����Ʈ

    public bool printerSignalInput;
    Transform targetToMove;
    RobotArmControl agvRobotArm;

    private void Start()
    {
        FindPrinterObject();
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

            // īƮ�� AGV�� ���� ��
            float angleDifference = Quaternion.Angle(transform.rotation, targetToMove.rotation);

            if (AGVtoCartDistance() < 0.01f && angleDifference < 1f)
            {

            }
        }
    }

    private void FindPrinterObject()
    {
        foreach (GameObject obj in printerSet) // GameObject�� �ݺ�
        {
            // �ڽ� ������Ʈ���� �̸��� "AGVLocate"�� Transform ã��
            foreach (Transform child in obj.transform)
            {
                if (child.name.Contains("AGVLocate") && !printerLocation.Contains(child)) // �ߺ� üũ
                {
                    printerLocation.Add(child); // Transform�� ����Ʈ�� �߰�
                }

                // �ڽ� ������Ʈ���� PrinterCode ������Ʈ ã��
                PrinterCode printerCode = child.GetComponent<PrinterCode>();
                if (printerCode != null && !printers.Contains(printerCode)) // �ߺ� üũ
                {
                    printers.Add(printerCode); // PrinterCode�� ����Ʈ�� �߰�
                }
            }
        }
    }
}
