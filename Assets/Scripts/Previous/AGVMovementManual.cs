using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AGVMovementManual : MonoBehaviour
{
    bool isPosSave = false;

    private LineRendererMake lineRenderer;
    public LineRendererMake Line { get => lineRenderer;}
    public List<Vector3> savingPosition = new List<Vector3>();
    private int currentTargetIndex = 0; // ���� ��ǥ ��ġ �ε���
    float moveSpeed;
    float rotationSpeed;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRendererMake>(); // LineRendererExample �ν��Ͻ� �߰�
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
            lineRenderer.GetComponent<LineRenderer>().startColor = Color.yellow;
            lineRenderer.GetComponent<LineRenderer>().endColor = Color.yellow;

            print("���� ���� �Ϸ�");
        }
    }
}