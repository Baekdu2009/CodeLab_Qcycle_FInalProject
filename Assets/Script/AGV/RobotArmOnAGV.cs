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

    bool gripperOn;
    bool plateOn;
    float gripperRotSpeed = 5f;
    float plateRotSpeed = 5f;

    protected override void OnValidate()
    {
        base.OnValidate();

        // �θ� Ŭ������ motors �迭 ���
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
        GripperRotate();
        PlateRotate();
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


}
