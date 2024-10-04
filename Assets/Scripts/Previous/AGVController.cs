using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class AGVController : MonoBehaviour
{
    public LineRenderer lineRenderer; // LineRenderer
    public NavMeshSurface navMeshSurface; // NavMeshSurface
    public float moveSpeed = 2f; // AGV �̵� �ӵ�
    public float returnDistance = 0.2f; // LineRenderer�� ���ƿ��� �Ÿ�
    public float updateDistance = 2f; // NavMeshSurface ��ġ ������Ʈ �Ÿ�
    public Transform[] targetPos; // ��ǥ ��ġ �迭

    private int currentTargetIndex = 0; // ���� ��ǥ �ε���

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

            // ��ֹ� ȸ�� ����
            AvoidObstacles();

            // AGV�� LineRenderer�� ��ġ���� ���� �Ÿ� �̻� ��� ���
            if (Vector3.Distance(transform.position, targetPosition) > returnDistance)
            {
                // LineRenderer ������ ���ƿ���
                transform.position += direction * moveSpeed * Time.deltaTime;
            }
            else
            {
                // ��ǥ ��ġ�� ���������� ���� ��ǥ�� �̵�
                currentTargetIndex++;
            }
        }
    }

    void UpdateNavMeshSurface()
    {
        if (Vector3.Distance(transform.position, navMeshSurface.transform.position) > updateDistance)
        {
            navMeshSurface.transform.position = transform.position;
            navMeshSurface.BuildNavMesh(); // NavMesh ����ũ
        }
    }

    void AvoidObstacles()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
        {
            // ��ֹ��� �����Ǹ� ������ ����
            Vector3 newDirection = Vector3.Reflect(transform.forward, hit.normal);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }
}
