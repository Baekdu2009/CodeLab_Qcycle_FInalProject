using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AGVMovementManual : MonoBehaviour
{
    public float moveSpeed = 5f; // �̵� �ӵ�
    public float rotationSpeed = 200f; // ȸ�� �ӵ�
    bool isPosSave = false;

    private LineRendererExample lineRenderer;
    public List<Vector3> savingPosition = new List<Vector3>();
    private int currentTargetIndex = 0; // ���� ��ǥ ��ġ �ε���

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRendererExample>(); // LineRendererExample �ν��Ͻ� �߰�
    }

    void Update()
    {
        if (currentTargetIndex < savingPosition.Count)
        {
            MoveWithRoute(); // ��θ� ���� �̵�
        }
        else
        {
            AGVMoveByKey(); // Ű �Է¿� ���� �̵�
        }

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

    public void MoveWithRoute()
    {
        if (currentTargetIndex < savingPosition.Count)
        {
            Vector3 targetPosition = savingPosition[currentTargetIndex];
            Vector3 direction = (targetPosition - transform.position).normalized;

            // ȸ��
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            // �̵�
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // ��ǥ ��ġ�� �����ߴ��� Ȯ��
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                currentTargetIndex++; // ���� ��ǥ ��ġ�� �̵�
            }
        }
    }

    public void RouteCreate()
    {
        if (savingPosition.Count > 0)
        {
            lineRenderer.UpdateLine(savingPosition.ToArray()); // List�� �迭�� ��ȯ�Ͽ� ȣ��
            lineRenderer.GetComponent<LineRenderer>().startColor = Color.yellow;
            lineRenderer.GetComponent<LineRenderer>().endColor = Color.yellow;
        }
    }
}
