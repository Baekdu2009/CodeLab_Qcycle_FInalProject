using UnityEngine;
using UnityEngine.AI;

public class AGVController : MonoBehaviour
{
    public Transform[] targets; // ��ǥ ���� �迭
    private NavMeshAgent agent;
    private int currentTargetIndex = 0; // ���� ��ǥ ���� �ε���

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MoveToTarget();
    }

    void Update()
    {
        // ��ǥ ������ �����ߴ��� Ȯ��
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            Debug.Log("��ǥ ������ �����߽��ϴ�.");
            currentTargetIndex++; // ���� ��ǥ�� �̵�

            // ��� ��ǥ ������ �����ߴ��� Ȯ��
            if (currentTargetIndex < targets.Length)
            {
                MoveToTarget(); // ���� ��ǥ�� �̵�
            }
            else
            {
                Debug.Log("��� ��ǥ ������ �����߽��ϴ�.");
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

    // ���ο� ��ǥ ������ �����ϴ� �޼���
    public void SetNewTarget(Transform[] newTargets)
    {
        targets = newTargets;
        currentTargetIndex = 0; // �ε��� �ʱ�ȭ
        MoveToTarget();
    }
}
