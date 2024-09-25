using UnityEngine;

public class AGVMovementAuto : MonoBehaviour
{
    public LineRendererExample lineRendererExample; // LineRendererExample 스크립트를 참조
    public float speed = 2.0f; // 이동 속도
    public float rotationSpeed = 200f; // 회전 속도
    private int currentPointIndex = 0; // 현재 목표 점 인덱스
    private bool movingForward = true; // 이동 방향 추적
    private bool isRotating = false; // 회전 중인지 확인

    void Start()
    {
        // AGV를 첫 번째 점 위치로 초기화
        if (lineRendererExample.points.Length > 0)
        {
            transform.position = lineRendererExample.points[0].position;
        }
    }

    void Update()
    {
        if (lineRendererExample.points.Length > 0)
        {
            MoveAlongLine();
        }
    }

    void MoveAlongLine()
    {
        // 현재 목표 점 가져오기
        Transform targetPoint = lineRendererExample.points[currentPointIndex];

        // 목표 점을 바라보도록 회전
        if (isRotating)
        {
            Vector3 direction = (targetPoint.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            // 회전이 완료되면 이동 시작
            if (Quaternion.Angle(transform.rotation, lookRotation) < 1f)
            {
                isRotating = false; // 회전 완료
            }
        }
        else
        {
            // AGV와 목표 점 사이의 거리 계산
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, step);

            // 목표 점에 도달하면 다음 점으로 이동
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
            {
                isRotating = true; // 회전 시작

                if (movingForward)
                {
                    currentPointIndex++;

                    // 마지막 점에 도달했으면 방향을 반전
                    if (currentPointIndex >= lineRendererExample.points.Length)
                    {
                        currentPointIndex = lineRendererExample.points.Length - 2; // 마지막 점에서 이전 점으로 설정
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
