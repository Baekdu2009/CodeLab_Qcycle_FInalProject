using UnityEngine;
using UnityEngine.AI;

public class AGVController : MonoBehaviour
{
    public Transform[] targets; // 목표 지점 배열
    private NavMeshAgent agent;
    private int currentTargetIndex = 0; // 현재 목표 지점 인덱스

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MoveToTarget();
    }

    void Update()
    {
        // 목표 지점에 도착했는지 확인
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            Debug.Log("목표 지점에 도착했습니다.");
            currentTargetIndex++; // 다음 목표로 이동

            // 모든 목표 지점에 도달했는지 확인
            if (currentTargetIndex < targets.Length)
            {
                MoveToTarget(); // 다음 목표로 이동
            }
            else
            {
                Debug.Log("모든 목표 지점에 도착했습니다.");
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

    // 새로운 목표 지점을 설정하는 메서드
    public void SetNewTarget(Transform[] newTargets)
    {
        targets = newTargets;
        currentTargetIndex = 0; // 인덱스 초기화
        MoveToTarget();
    }
}
