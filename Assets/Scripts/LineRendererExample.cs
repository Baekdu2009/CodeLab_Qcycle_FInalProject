using UnityEngine;

public class LineRendererExample : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Vector3[] points; // �� �迭

    void Start()
    {
        // LineRenderer ������Ʈ �ʱ�ȭ
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // LineRenderer �Ӽ� ����
        lineRenderer.startWidth = 0f;
        lineRenderer.endWidth = 0f;

        // points �迭�� null�� ��� �ʱ�ȭ
        if (points == null || points.Length == 0)
        {
            points = new Vector3[0]; // �� �迭�� �ʱ�ȭ
        }

        // ���� �� �� ����
        lineRenderer.positionCount = points.Length;

        // �������� ���� ����
        // UpdateLine();
    }

    public void UpdateLine()
    {
        if (points != null && points.Length > 0)
        {
            for (int i = 0; i < points.Length; i++)
            {
                lineRenderer.SetPosition(i, points[i]);
            }
        }
    }

    public void UpdateLine(Vector3[] vector)
    {
        if (vector == null || vector.Length == 0)
        {
            lineRenderer.positionCount = 0; // �� ���� 0���� ����
            return; // �Լ� ����
        }

        lineRenderer.positionCount = vector.Length; // �� �� ����
        for (int i = 0; i < vector.Length; i++)
        {
            lineRenderer.SetPosition(i, vector[i]);
        }
    }
}
