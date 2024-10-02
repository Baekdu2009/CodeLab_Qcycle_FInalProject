using UnityEngine;
using UnityEngine.AI;

public class ObstacleMoving : MonoBehaviour
{
    public float speed = 5f; // �̵� �ӵ�
    public float moveDistance = 5f; // �պ� ��� �Ÿ�
    private Vector3 startPosition; // ���� ��ġ
    private Vector3 targetPosition; // ��ǥ ��ġ
    private bool movingToTarget = true; // �̵� ����
    private NavMeshObstacle navMeshObstacle;

    void Start()
    {
        // ��ֹ��� ���� ��ġ ����
        startPosition = transform.position;
        targetPosition = startPosition + transform.forward * moveDistance; // ��ǥ ��ġ ����
        navMeshObstacle = GetComponent<NavMeshObstacle>(); // NavMeshObstacle ������Ʈ ��������
    }

    void Update()
    {
        MoveObstacles();
    }

    void MoveObstacles()
    {
        // ���� ��ġ�� ��ǥ ��ġ ���� �Ÿ� ���
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        // ��ǥ ��ġ�� �����ߴ��� Ȯ��
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // �̵� ���� ����
            movingToTarget = !movingToTarget;
            targetPosition = movingToTarget ? startPosition + transform.forward * moveDistance : startPosition;
        }
    }
}
