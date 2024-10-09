using UnityEngine;

public class AGVLarge : AGVControl
{
    public AGVCart[] movingCarts; // AGV가 이동할 카트 배열
    public bool fullSignalInput;   // 신호 상태를 나타내는 변수

    private void Start()
    {
        movingPositions.Add(transform);
        
    }

    private void Update()
    {
        CartSignalCheck(); // 카트 신호 체크
        
        if (isMoving)
        {
            MoveAlongPath();   // 경로 따라 이동
        }
    }

    private void CartSignalCheck()
    {
        if (!isMoving)
        {
            fullSignalInput = false; // 신호 초기화

            for (int i = 0; i < movingCarts.Length; i++)
            {
                if (movingCarts[i].isAGVCallOn) // 카트의 신호가 켜져 있는지 확인
                {
                    fullSignalInput = true; // 신호가 하나라도 켜지면 true로 설정
                    movingPositions.Add(movingCarts[i].transform); // 현재 목표 위치 설정
                    isMoving = true; // AGV 이동 상태 설정
                    break; // 첫 번째 카트만 처리하고 루프 종료
                }
            }
        }
    }
}
