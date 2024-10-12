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
                break; // 신호가 들어오면 이동 시작
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
                if (!printerLocation.Contains(child)) // 중복 체크
                {
                    printerLocation.Add(child); // Transform을 리스트에 추가
                }

                PrinterCode printerCode = child.GetComponent<PrinterCode>();
                if (printerCode != null && !printers.Contains(printerCode)) // 중복 체크
                {
                    printers.Add(printerCode); // PrinterCode를 리스트에 추가
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
