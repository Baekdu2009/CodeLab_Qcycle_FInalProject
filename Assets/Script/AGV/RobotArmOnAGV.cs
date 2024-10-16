using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;

public class RobotArmOnAGV : RobotArmControl
{
    // public GameObject robotArmPlate;
    public GameObject rightGripper;
    public GameObject leftGripper;
    public bool gripperWorking;
    public Transform plateLocation;
    public Image pickingCheck;
    public TMP_Text pickngTxt;

    // [HideInInspector]
    AGVSmall robotAGV;
    PrinterCode printer;
    public GameObject printingObject;
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
        RobotArmUIUpdate();
        GripperRotate();
        // PlateRotate();
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

    //private void PlateRotate()
    //{
    //    Quaternion zeroRotation = Quaternion.Euler(0, 0, 0);
    //    Quaternion plateRotation = Quaternion.Euler(0, 0, 135);

    //    if (plateOn && gripperOn)
    //    {
    //        robotArmPlate.transform.localRotation = Quaternion.Slerp(robotArmPlate.transform.localRotation, plateRotation, plateRotSpeed * Time.deltaTime);
    //    }
    //    else
    //    {
    //        robotArmPlate.transform.localRotation = Quaternion.Slerp(robotArmPlate.transform.localRotation, zeroRotation, plateRotSpeed * Time.deltaTime);
    //    }
    //}

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
            isRunning = true;

            if (printer == null || printer.visibleObject == null) return;


            if (steps != null)
            {
                StartCoroutine(RunSteps());
            }

            gripperWorking = true;

            PIckUpPrintingObject();

            plateOn = true;

            if (printingObject != null)
            {
                StopCoroutine(RunSteps());
                isRunning = false;
                gripperWorking = false;
                plateOn = false;
            }
        }
    }

    private void PIckUpPrintingObject()
    {
        printer.visibleObject.transform.SetParent(null);
        printingObject = printer.visibleObject;

        printingObject.transform.position = plateLocation.position;
        printingObject.transform.SetParent(plateLocation.transform);
        Rigidbody rb = printingObject.GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.isKinematic = true;
    }

    private void RobotArmUIUpdate()
    {
        if (printingObject != null)
        {
            pickingCheck.color = Color.green;
            pickngTxt.text = "Pick Object";
            pickngTxt.color = Color.green;
        }
        else
        {
            pickingCheck.color = Color.yellow;
            pickngTxt.text = "No Object";
            pickngTxt.color = Color.yellow;
        }
    }

    public void PickDownPrintingObject()
    {
        printingObject.transform.SetParent(null);
        Destroy(printingObject);
        printingObject= null;
    }
}
