using UnityEngine;

public class LineRendererMakeTransform : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform[] points; // Transform �迭�� ����

    void Start()
    {
        // LineRenderer ������Ʈ �ʱ�ȭ
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // LineRenderer �Ӽ� ����
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

        // points �迭�� null�� ��� �ʱ�ȭ
        if (points == null || points.Length == 0)
        {
            points = new Transform[0]; // �� �迭�� �ʱ�ȭ
        }

        // ���� �� �� ����
        lineRenderer.positionCount = points.Length;

        // �������� ���� ����
        UpdateLine();
    }

    public void UpdateLine()
    {
        if (points != null && points.Length > 0)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i] != null) // Transform�� null�� �ƴ� ���
                {
                    lineRenderer.SetPosition(i, points[i].position); // �� Transform�� ��ġ ����
                }
            }
        }
    }

    public void UpdateLine(Transform[] transforms)
    {
        if (transforms == null || transforms.Length == 0)
        {
            lineRenderer.positionCount = 0; // �� ���� 0���� ����
            return; // �Լ� ����
        }

        lineRenderer.positionCount = transforms.Length; // �� �� ����
        for (int i = 0; i < transforms.Length; i++)
        {
            if (transforms[i] != null) // Transform�� null�� �ƴ� ���
            {
                lineRenderer.SetPosition(i, transforms[i].position); // �� Transform�� ��ġ ����
            }
        }
    }
}
