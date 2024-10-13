using UnityEngine;

public class BoxingRobot : RobotArmControl
{

    protected override void OnValidate()
    {
        base.OnValidate();

        // 부모 클래스의 motors 배열 사용
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
            array = new T[length]; // 배열 초기화
        }
    }

    protected override void Start()
    {
        base.Start(); // 부모 클래스의 Start 호출
    }

    private void Update()
    {
        
    }

    
}
