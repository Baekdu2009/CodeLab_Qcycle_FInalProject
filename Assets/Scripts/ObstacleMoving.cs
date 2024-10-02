using UnityEngine;
using UnityEngine.AI;

public class ObstacleMoving : MonoBehaviour
{
    public float speed = 5f; // 이동 속도
    public float moveDistance = 5f; // 왕복 운동할 거리
    private Vector3 startPosition; // 시작 위치
    private Vector3 targetPosition; // 목표 위치
    private bool movingToTarget = true; // 이동 방향
    private NavMeshObstacle navMeshObstacle;

    void Start()
    {
        // 장애물의 시작 위치 저장
        startPosition = transform.position;
        targetPosition = startPosition + transform.forward * moveDistance; // 목표 위치 설정
        navMeshObstacle = GetComponent<NavMeshObstacle>(); // NavMeshObstacle 컴포넌트 가져오기
    }

    void Update()
    {
        MoveObstacles();
    }

    void MoveObstacles()
    {
        // 현재 위치와 목표 위치 간의 거리 계산
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        // 목표 위치에 도달했는지 확인
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // 이동 방향 반전
            movingToTarget = !movingToTarget;
            targetPosition = movingToTarget ? startPosition + transform.forward * moveDistance : startPosition;
        }
    }
}
