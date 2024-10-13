using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class RobotArmOnAGV : RobotArmControl
{
    public GameObject robotArmPlate;
    public GameObject rightGripper;
    public GameObject leftGripper;
    public bool gripperWorking;
    public Transform plateLocation;

    AGVSmall robotAGV;
    PrinterCode printer;
    GameObject printingObject;
    bool gripperOn;
    bool plateOn;
    public bool printerSignal;
    float gripperRotSpeed = 5f;
    float plateRotSpeed = 5f;

    protected override void OnValidate()
    {
        base.OnValidate();

        robotAGV = GetComponentInParent<AGVSmall>();
        int currentLength = motors != null ? motors.Length : 0;

        // 배열 초기화 메서드 호출
        //InitializeArray(ref minAngles, currentLength);
        //InitializeArray(ref maxAngles, currentLength);
        InitializeArray(ref rotationAxes, currentLength);
    }

    private void InitializeArray<T>(ref T[] array, int length)
    {
        if (array == null || array.Length != length)
        {
            array = new T[length];
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        printerSignal = robotAGV.printerSignalInput;

        GripperRotate();
        PlateRotate();
        PullOutPrintingObject();
    }

    private void GripperRotate()
    {
        // 기준 각도
        Quaternion zeroRotation = Quaternion.Euler(0, 0, 0);
        Quaternion rightRotation = Quaternion.Euler(0, -25, 0);
        Quaternion leftRotation = Quaternion.Euler(0, 25, 0);

        if (gripperWorking)
        {
            rightGripper.transform.localRotation = Quaternion.Slerp(rightGripper.transform.localRotation, rightRotation, gripperRotSpeed * Time.deltaTime);
            leftGripper.transform.localRotation = Quaternion.Slerp(leftGripper.transform.localRotation, leftRotation, gripperRotSpeed * Time.deltaTime);
            gripperOn = true;
        }
        else
        {
            rightGripper.transform.localRotation = Quaternion.Slerp(rightGripper.transform.localRotation, zeroRotation, gripperRotSpeed * Time.deltaTime);
            leftGripper.transform.localRotation = Quaternion.Slerp(leftGripper.transform.localRotation, zeroRotation, gripperRotSpeed * Time.deltaTime);
            gripperOn = false;
        }
    }

    private void PlateRotate()
    {
        Quaternion zeroRotation = Quaternion.Euler(0, 0, 0);
        Quaternion plateRotation = Quaternion.Euler(0, 0, 135);

        if (plateOn && gripperOn)
        {
            robotArmPlate.transform.localRotation = Quaternion.Slerp(robotArmPlate.transform.localRotation, plateRotation, plateRotSpeed * Time.deltaTime);
        }
        else
        {
            robotArmPlate.transform.localRotation = Quaternion.Slerp(robotArmPlate.transform.localRotation, zeroRotation, plateRotSpeed * Time.deltaTime);
        }
    }

    private void PrinterSignalStart()
    {
        if (printerSignal)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    public void PullOutPrintingObject()
    {
        // 프린터 신호가 있고 AGV가 프린터 위치에 도착했을 때만 실행
        if (printerSignal && robotAGV.printerLocationArrived)
        {
            printer = robotAGV.targetPrinter;

            // Null 체크
            if (printer == null || printer.visibleObject == null) return;

            // 이미 물체가 꺼내졌다면 종료
            if (printingObject != null) return;

            DetachPrintingObject();
        }
    }

    private void DetachPrintingObject()
    {
        // 출력 물체 분리
        printer.visibleObject.transform.SetParent(null, true);
        printingObject = printer.visibleObject;

        // 물체 위치 설정
        printingObject.transform.position = plateLocation.position;
        printingObject.transform.SetParent(plateLocation.transform, true);
    }
}
