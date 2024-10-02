using UnityEngine;
using UnityEngine.AI;

public class AGVController : MonoBehaviour
{
    public Transform[] targets; // ��ǥ ���� �迭
    private NavMeshAgent agent;
    private int currentTargetIndex = 0; // ���� ��ǥ ���� �ε���
    public float rayDistance = 5f; // Raycast �Ÿ�
    public float avoidanceDistance = 0.2f; // ȸ�� �Ÿ�
    private bool isStopping = false; // ���߰� �ִ��� ����
    private bool lastTargetStop = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MoveToTarget();
    }

    void Update()
    {
        // ��ֹ� ����
        DetectObstacles();

        if (!isStopping && !lastTargetStop)
        {
            // ��ǥ ������ �����ߴ��� Ȯ��
            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                Debug.Log("��ǥ ������ �����߽��ϴ�.");
                currentTargetIndex++;

                if (currentTargetIndex < targets.Length)
                {
                    MoveToTarget();
                }
                else
                {
                    Debug.Log("��� ��ǥ ������ �����߽��ϴ�.");
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
        // ���� Raycast
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward) * rayDistance;

        if (Physics.Raycast(transform.position, forward, out hit, rayDistance))
        {
            if (hit.collider != null && hit.collider.CompareTag("Person")) // "Person" �±� Ȯ��
            {
                // ��ֹ� ���� �� ����
                isStopping = true;
                agent.isStopped = true; // NavMeshAgent ����
                Debug.DrawRay(transform.position, forward, Color.red); // Raycast �ð�ȭ
            }
            else if (hit.collider != null && hit.collider.CompareTag("Obstacle")) // "Obstacle" �±� Ȯ��
            {
                // ��ֹ� ���� �� ȸ�� ����
                Vector3 avoidanceDirection = Vector3.Reflect(forward, hit.normal);
                Vector3 newDestination = transform.position + avoidanceDirection.normalized * avoidanceDistance;
                agent.SetDestination(newDestination);
                Debug.DrawRay(transform.position, forward, Color.yellow); // ȸ�� �ð�ȭ
            }
        }
        else
        {
            // ��ֹ��� ������ �ٽ� �̵�
            if (isStopping)
            {
                isStopping = false; // ���� ���� ����
                agent.isStopped = false; // NavMeshAgent �簳
                MoveToTarget(); // ��ǥ �������� �̵�
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
