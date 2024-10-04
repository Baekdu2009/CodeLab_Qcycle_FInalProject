using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class AGVController : MonoBehaviour
{
    public LineRenderer lineRenderer; // LineRenderer
    public NavMeshSurface navMeshSurface; // NavMeshSurface
    public float moveSpeed = 2f; // AGV 이동 속도
    public float returnDistance = 0.2f; // LineRenderer로 돌아오는 거리
    public float updateDistance = 2f; // NavMeshSurface 위치 업데이트 거리
    public Transform[] targetPos; // 목표 위치 배열

    private int currentTargetIndex = 0; // 현재 목표 인덱스

    void Update()
    {
        MoveAGV();
        UpdateNavMeshSurface();
    }

    void MoveAGV()
    {
        if (currentTargetIndex < lineRenderer.positionCount)
        {
            Vector3 targetPosition = lineRenderer.GetPosition(currentTargetIndex);
            Vector3 direction = (targetPosition - transform.position).normalized;

            // 장애물 회피 로직
            AvoidObstacles();

            // AGV가 LineRenderer의 위치에서 일정 거리 이상 벗어난 경우
            if (Vector3.Distance(transform.position, targetPosition) > returnDistance)
            {
                // LineRenderer 쪽으로 돌아오기
                transform.position += direction * moveSpeed * Time.deltaTime;
            }
            else
            {
                // 목표 위치에 도달했으면 다음 목표로 이동
                currentTargetIndex++;
            }
        }
    }

    void UpdateNavMeshSurface()
    {
        if (Vector3.Distance(transform.position, navMeshSurface.transform.position) > updateDistance)
        {
            navMeshSurface.transform.position = transform.position;
            navMeshSurface.BuildNavMesh(); // NavMesh 베이크
        }
    }

    void AvoidObstacles()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
        {
            // 장애물이 감지되면 방향을 변경
            Vector3 newDirection = Vector3.Reflect(transform.forward, hit.normal);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }
}
