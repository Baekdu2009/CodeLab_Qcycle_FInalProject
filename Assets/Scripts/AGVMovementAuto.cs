using UnityEngine;

public class AGVMovementAuto : AGVMoving
{
    private LineRendererExample lineRendererAuto;
    private int currentPointIndex = 0; // ���� ��ǥ �� �ε���
    private bool movingForward = true; // �̵� ���� ����
    private bool isRotating = false; // ȸ�� ������ Ȯ��

    void Start()
    {
        AGVMovementManual manual = GetComponent<AGVMovementManual>();
        if (manual != null)
        {
            lineRendererAuto = manual.Line; // AGVMovementManual�� LineRendererExample ��������
            MoveOrigin(); // AGV�� ù ��° �� ��ġ�� �ʱ�ȭ
        }
        else
        {
            Debug.LogError("AGVMovementManual ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }


    void Update()
    {
        // MoveAuto(); // �ڵ����� �̵�
    }

    public void MoveOrigin()
    {
        // AGV�� ù ��° �� ��ġ�� �ʱ�ȭ
        if (lineRendererAuto != null && lineRendererAuto.points.Length > 0)
        {
            transform.position = lineRendererAuto.points[0];
        }
        else
        {
            Debug.LogError("LineRendererExample�� �ʱ�ȭ���� �ʾҰų� ���� �����ϴ�.");
        }
    }


    public void MoveAuto()
    {
        if (lineRendererAuto != null && lineRendererAuto.points.Length > 0)
        {
            print("�̵� ����");
            MoveAlongLine(); // �ڵ����� �̵�
        }
        else
        {
            Debug.LogError("LineRendererExample�� �ʱ�ȭ���� �ʾҰų� ���� �����ϴ�.");
        }
    }


    void MoveAlongLine()
    {
        // ���� ��ǥ �� ��������
        if (lineRendererAuto.points.Length == 0) return; // ���� ������ ����

        Vector3 targetPoint = lineRendererAuto.points[currentPointIndex];

        // ��ǥ ���� �ٶ󺸵��� ȸ��
        if (isRotating)
        {
            Vector3 direction = (targetPoint - transform.position).normalized;
            if (direction.magnitude > 0.01f) // ���� ���Ͱ� 0�� �ƴ� ���� ȸ��
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

                // ȸ���� �Ϸ�Ǹ� �̵� ����
                if (Quaternion.Angle(transform.rotation, lookRotation) < 1f)
                {
                    isRotating = false; // ȸ�� �Ϸ�
                }
            }
        }
        else
        {
            // AGV�� ��ǥ �� ������ �Ÿ� ���
            float step = moveSpeed * Time.deltaTime; // moveSpeed�� AGVMoving���� ��ӹ���
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, step);

            // ��ǥ ���� �����ϸ� ���� ������ �̵�
            if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
            {
                isRotating = true; // ȸ�� ����

                if (movingForward)
                {
                    currentPointIndex++;

                    // ������ ���� ���������� ������ ����
                    if (currentPointIndex >= lineRendererAuto.points.Length)
                    {
                        currentPointIndex = lineRendererAuto.points.Length - 2; // ������ ������ ���� ������ ����
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