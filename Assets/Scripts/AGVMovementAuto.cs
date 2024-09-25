using UnityEngine;

public class AGVMovementAuto : AGVMoving
{
    private LineRendererExample lineRendererAuto;
    private int currentPointIndex = 0; // 현재 목표 점 인덱스
    private bool movingForward = true; // 이동 방향 추적
    private bool isRotating = false; // 회전 중인지 확인

    void Start()
    {
        AGVMovementManual manual = GetComponent<AGVMovementManual>();
        if (manual != null)
        {
            lineRendererAuto = manual.Line; // AGVMovementManual의 LineRendererExample 가져오기
            MoveOrigin(); // AGV를 첫 번째 점 위치로 초기화
        }
        else
        {
            Debug.LogError("AGVMovementManual 컴포넌트를 찾을 수 없습니다.");
        }
    }


    void Update()
    {
        // MoveAuto(); // 자동으로 이동
    }

    public void MoveOrigin()
    {
        // AGV를 첫 번째 점 위치로 초기화
        if (lineRendererAuto != null && lineRendererAuto.points.Length > 0)
        {
            transform.position = lineRendererAuto.points[0];
        }
        else
        {
            Debug.LogError("LineRendererExample이 초기화되지 않았거나 점이 없습니다.");
        }
    }


    public void MoveAuto()
    {
        if (lineRendererAuto != null && lineRendererAuto.points.Length > 0)
        {
            print("이동 시작");
            MoveAlongLine(); // 자동으로 이동
        }
        else
        {
            Debug.LogError("LineRendererExample이 초기화되지 않았거나 점이 없습니다.");
        }
    }


    void MoveAlongLine()
    {
        // 현재 목표 점 가져오기
        if (lineRendererAuto.points.Length == 0) return; // 점이 없으면 종료

        Vector3 targetPoint = lineRendererAuto.points[currentPointIndex];

        // 목표 점을 바라보도록 회전
        if (isRotating)
        {
            Vector3 direction = (targetPoint - transform.position).normalized;
            if (direction.magnitude > 0.01f) // 방향 벡터가 0이 아닐 때만 회전
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

                // 회전이 완료되면 이동 시작
                if (Quaternion.Angle(transform.rotation, lookRotation) < 1f)
                {
                    isRotating = false; // 회전 완료
                }
            }
        }
        else
        {
            // AGV와 목표 점 사이의 거리 계산
            float step = moveSpeed * Time.deltaTime; // moveSpeed는 AGVMoving에서 상속받음
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, step);

            // 목표 점에 도달하면 다음 점으로 이동
            if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
            {
                isRotating = true; // 회전 시작

                if (movingForward)
                {
                    currentPointIndex++;

                    // 마지막 점에 도달했으면 방향을 반전
                    if (currentPointIndex >= lineRendererAuto.points.Length)
                    {
                        currentPointIndex = lineRendererAuto.points.Length - 2; // 마지막 점에서 이전 점으로 설정
                        movingForward = false; // 방향을 반전
                    }
                }
                else
                {
                    currentPointIndex--;

                    // 처음 점에 도달했으면 방향을 반전
                    if (currentPointIndex < 0)
                    {
                        currentPointIndex = 1; // 처음 점에서 다음 점으로 설정
                        movingForward = true; // 방향을 반전
                    }
                }
            }
        }
    }
}