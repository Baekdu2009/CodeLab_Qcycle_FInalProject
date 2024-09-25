using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AGVMoving : MonoBehaviour
{
    public float moveSpeed = 2f; // �̵� �ӵ�
    public float rotationSpeed = 200f; // ȸ�� �ӵ�

    private bool isPosSave = false;
    private LineRendererExample lineRenderer;
    public List<Vector3> savingPosition = new List<Vector3>();
    private int currentPointIndex = 0; // ���� ��ǥ �� �ε���
    private bool movingForward = true; // �̵� ���� ����
    private bool isRotating = false; // ȸ�� ������ Ȯ��

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRendererExample>(); // LineRendererExample �ν��Ͻ� �߰�
    }

    void Update()
    {
        AGVMoveByKey(); // Ű �Է¿� ���� �̵�
        PositionCheck();
    }

    void AGVMoveByKey()
    {
        // W Ű�� ������ �� ������ �̵�
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            isPosSave = false;
        }

        // A Ű�� ������ �� �������� ȸ��
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
            isPosSave = false;
        }

        // D Ű�� ������ �� �������� ȸ��
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            isPosSave = false;
        }
    }

    public void PositionCheck()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isPosSave)
        {
            print($"���� ��ġ: {transform.position}");
            print("��ġ�� �����Ϸ��� �����̽��ٸ� �ٽ� ��������.");
            isPosSave = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isPosSave)
        {
            Vector3 currentPos = transform.position;
            savingPosition.Add(currentPos);
            print(currentPos.ToString());
            print("��ġ�� ����Ǿ����ϴ�");
            isPosSave = false;
        }
    }

    public void RouteCreate()
    {
        if (savingPosition.Count > 0)
        {
            // points �迭�� savingPosition�� ũ��� �ʱ�ȭ
            lineRenderer.points = new Vector3[savingPosition.Count];

            // savingPosition�� ���� points �迭�� �Ҵ�
            for (int i = 0; i < savingPosition.Count; i++)
            {
                lineRenderer.points[i] = savingPosition[i];
            }

            // LineRenderer ������Ʈ
            lineRenderer.UpdateLine(lineRenderer.points); // UpdateLine �޼��� ȣ��
            lineRenderer.GetComponent<LineRenderer>().startColor = Color.red;
            lineRenderer.GetComponent<LineRenderer>().endColor = Color.red;

            print("���� ���� �Ϸ�");
        }
    }

    public void MoveOrigin()
    {
        // AGV�� ù ��° �� ��ġ�� �ʱ�ȭ
        if (lineRenderer.points.Length > 0)
        {
            transform.position = lineRenderer.points[0];
        }
        else
        {
            Debug.LogError("LineRendererExample�� �ʱ�ȭ���� �ʾҰų� ���� �����ϴ�.");
        }
    }

    public void MoveAuto()
    {
        if (lineRenderer.points.Length > 0)
        {
            StartCoroutine(MoveAlongLine()); // �ڵ����� �̵�
        }
        else
        {
            Debug.LogError("LineRendererExample�� �ʱ�ȭ���� �ʾҰų� ���� �����ϴ�.");
        }
    }

    private IEnumerator MoveAlongLine()
    {
        while (true) // ���� ����, �ʿ信 ���� ���� ���� �߰�
        {
            // ���� ��ǥ �� ��������
            if (lineRenderer.points.Length == 0) yield break; // ���� ������ ����

            Vector3 targetPoint = lineRenderer.points[currentPointIndex];

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
                        if (currentPointIndex >= lineRenderer.points.Length)
                        {
                            currentPointIndex = lineRenderer.points.Length - 2; // ������ ������ ���� ������ ����
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

            yield return new WaitForEndOfFrame(); // ���� �����ӱ��� ���
        }
    }

}
