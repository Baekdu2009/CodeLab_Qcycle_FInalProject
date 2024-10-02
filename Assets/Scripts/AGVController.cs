using UnityEngine;
using UnityEngine.AI;

public class AGVController : MonoBehaviour
{
    public Transform[] targets; // 목표 지점 배열
    private NavMeshAgent agent;
    private int currentTargetIndex = 0; // 현재 목표 지점 인덱스
    public float rayDistance = 5f; // Raycast 거리
    public float avoidanceDistance = 0.2f; // 회피 거리
    private bool isStopping = false; // 멈추고 있는지 여부
    private bool lastTargetStop = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MoveToTarget();
    }

    void Update()
    {
        // 장애물 감지
        DetectObstacles();

        if (!isStopping && !lastTargetStop)
        {
            // 목표 지점에 도착했는지 확인
            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                Debug.Log("목표 지점에 도착했습니다.");
                currentTargetIndex++;

                if (currentTargetIndex < targets.Length)
                {
                    MoveToTarget();
                }
                else
                {
                    Debug.Log("모든 목표 지점에 도착했습니다.");
                    lastTargetStop = true;
                }
            }
        }
    }

    void MoveToTarget()
    {
        if (targets != null && currentTargetIndex < targets.Length)
        {
            agent.SetDestination(targets[currentTargetIndex].position);
        }
    }

    void DetectObstacles()
    {
        // 전방 Raycast
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward) * rayDistance;

        if (Physics.Raycast(transform.position, forward, out hit, rayDistance))
        {
            if (hit.collider != null && hit.collider.CompareTag("Person")) // "Person" 태그 확인
            {
                // 장애물 감지 시 멈춤
                isStopping = true;
                agent.isStopped = true; // NavMeshAgent 정지
                Debug.DrawRay(transform.position, forward, Color.red); // Raycast 시각화
            }
            else if (hit.collider != null && hit.collider.CompareTag("Obstacle")) // "Obstacle" 태그 확인
            {
                // 장애물 감지 시 회피 로직
                Vector3 avoidanceDirection = Vector3.Reflect(forward, hit.normal);
                Vector3 newDestination = transform.position + avoidanceDirection.normalized * avoidanceDistance;
                agent.SetDestination(newDestination);
                Debug.DrawRay(transform.position, forward, Color.yellow); // 회피 시각화
            }
        }
        else
        {
            // 장애물이 없으면 다시 이동
            if (isStopping)
            {
                isStopping = false; // 멈춤 상태 해제
                agent.isStopped = false; // NavMeshAgent 재개
                MoveToTarget(); // 목표 지점으로 이동
            }
        }
    }

    public void SetNewTarget(Transform[] newTargets)
    {
        targets = newTargets;
        currentTargetIndex = 0;
        MoveToTarget();
    }
}
