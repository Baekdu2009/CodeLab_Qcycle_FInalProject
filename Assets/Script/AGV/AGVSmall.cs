using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGVSmall : AGVControl
{
    public GameObject[] printerSet; // 프린터 세트
    public List<PrinterCode> printers; // 프린터 코드 리스트
    public List<Transform> printerLocation; // 프린터 위치 저장 리스트

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

            // 카트와 AGV의 방향 비교
            float angleDifference = Quaternion.Angle(transform.rotation, targetToMove.rotation);

            if (AGVtoCartDistance() < 0.01f && angleDifference < 1f)
            {

            }
        }
    }

    private void FindPrinterObject()
    {
        foreach (GameObject obj in printerSet) // GameObject로 반복
        {
            // 자식 오브젝트에서 이름이 "AGVLocate"인 Transform 찾기
            foreach (Transform child in obj.transform)
            {
                if (child.name.Contains("AGVLocate") && !printerLocation.Contains(child)) // 중복 체크
                {
                    printerLocation.Add(child); // Transform을 리스트에 추가
                }

                // 자식 오브젝트에서 PrinterCode 컴포넌트 찾기
                PrinterCode printerCode = child.GetComponent<PrinterCode>();
                if (printerCode != null && !printers.Contains(printerCode)) // 중복 체크
                {
                    printers.Add(printerCode); // PrinterCode를 리스트에 추가
                }
            }
        }
    }
}
