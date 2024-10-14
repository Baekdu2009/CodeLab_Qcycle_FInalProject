using UnityEngine;

public class BoxingRobot : RobotArmControl
{

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
            array = new T[length]; // �迭 �ʱ�ȭ
        }
    }

    protected override void Start()
    {
        base.Start(); // �θ� Ŭ������ Start ȣ��
    }

    private void Update()
    {
        
    }

    
}
