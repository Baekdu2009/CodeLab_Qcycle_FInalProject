using UnityEngine;

public class AGVMovementAuto : MonoBehaviour
{
    public LineRendererExample lineRendererExample; // LineRendererExample ��ũ��Ʈ�� ����
    public float speed = 2.0f; // �̵� �ӵ�
    public float rotationSpeed = 200f; // ȸ�� �ӵ�
    private int currentPointIndex = 0; // ���� ��ǥ �� �ε���
    private bool movingForward = true; // �̵� ���� ����
    private bool isRotating = false; // ȸ�� ������ Ȯ��

    void Start()
    {
        // AGV�� ù ��° �� ��ġ�� �ʱ�ȭ
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
        // ���� ��ǥ �� ��������
        Transform targetPoint = lineRendererExample.points[currentPointIndex];

        // ��ǥ ���� �ٶ󺸵��� ȸ��
        if (isRotating)
        {
            Vector3 direction = (targetPoint.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            // ȸ���� �Ϸ�Ǹ� �̵� ����
            if (Quaternion.Angle(transform.rotation, lookRotation) < 1f)
            {
                isRotating = false; // ȸ�� �Ϸ�
            }
        }
        else
        {
            // AGV�� ��ǥ �� ������ �Ÿ� ���
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, step);

            // ��ǥ ���� �����ϸ� ���� ������ �̵�
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
            {
                isRotating = true; // ȸ�� ����

                if (movingForward)
                {
                    currentPointIndex++;

                    // ������ ���� ���������� ������ ����
                    if (currentPointIndex >= lineRendererExample.points.Length)
                    {
                        currentPointIndex = lineRendererExample.points.Length - 2; // ������ ������ ���� ������ ����
                        movingForward = false; // ������ ����
                    }
                }
                else
                {
                    currentPointIndex--;

                    // ó�� ���� ���������� ������ ����
                    if (currentPointIndex < 0)
                    {
                        currentPointIndex = 1; // ó�� ������ ���� ������ ����
                        movingForward = true; // ������ ����
                    }
                }
            }
        }
    }
}
