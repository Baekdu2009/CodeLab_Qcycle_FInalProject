using UnityEngine;

public class LineRendererExample : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform[] points; // �� �迭


    void Start()
    {
        // LineRenderer ������Ʈ �ʱ�ȭ
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // LineRenderer �Ӽ� ����
        lineRenderer.startWidth = 0f;
        lineRenderer.endWidth = 0f;
        //lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        //lineRenderer.startColor = Color.red;
        //lineRenderer.endColor = Color.red;

        // ���� �� �� ����
        lineRenderer.positionCount = points.Length;

        // �������� ���� ����
        UpdateLine();
    }

    void UpdateLine()
    {
        if (points != null && points.Length > 0)
        {
            for (int i = 0; i < points.Length; i++)
            {
                lineRenderer.SetPosition(i, points[i].position);
            }
        }
    }
}