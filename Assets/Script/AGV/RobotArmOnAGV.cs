using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class RobotArmOnAGV : RobotArmControl
{
    // public GameObject robotArmPlate;
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

        // �迭 �ʱ�ȭ �޼��� ȣ��
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
        // PlateRotate();
    }

    private void GripperRotate()
    {
        // ���� ����
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
        // ������ ��ȣ�� �ְ� AGV�� ������ ��ġ�� �������� ���� ����
        if (printerSignal && robotAGV.printerLocationArrived)
        {
            printer = robotAGV.targetPrinter;
            isRunning = true;

            // Null üũ
            if (printer == null || printer.visibleObject == null) return;


            if (steps != null)
            {
                StartCoroutine(RunSteps());
            }

            // �׸��۸� �۵����� ��¹��� ����
            gripperWorking = true; // �׸��� �۵�

            // ��¹� �и�
            DetachPrintingObject();

            // �÷���Ʈ�� ȸ����Ŵ
            plateOn = true; // �÷���Ʈ�� ȸ����Ű�� ���� �÷��� ����

            if (printingObject != null)
            {
                StopCoroutine(RunSteps());
                isRunning = false;
                gripperOn = false;
                plateOn = false;
            }
        }
    }

    private void DetachPrintingObject()
    {
        // ��� ��ü �и�
        printer.visibleObject.transform.SetParent(null, true);
        printingObject = printer.visibleObject;

        // ��ü ��ġ ����
        printingObject.transform.position = plateLocation.position;
        printingObject.transform.SetParent(plateLocation.transform, true);
        Rigidbody rb = printingObject.GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.isKinematic = true;

    }
}
