using UnityEngine;

public class LineRendererMake : MonoBehaviour
{
    LineRenderer lineRenderer;
    public float lineWidth = 0.2f;
    private Transform[] transformPos;
    public Vector3[] points; // �� �迭

    void Start()
    {
        // LineRenderer ������Ʈ �ʱ�ȭ
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // LineRenderer �Ӽ� ����
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

    }
    
    public void UpdateLine(Vector3[] vector)
    {
        lineRenderer.positionCount = vector.Length; // �� �� ����

        if (vector == null || vector.Length == 0)
        {
            lineRenderer.positionCount = 0; // �� ���� 0���� ����
            return; // �Լ� ����
        }

        for (int i = 0; i < vector.Length; i++)
        {
            lineRenderer.SetPosition(i, vector[i]);
        }
    }
    public void UpdateLine(Transform[] locate)
    {
        lineRenderer.positionCount = locate.Length; // �� �� ����

        if (locate == null || locate.Length == 0)
        {
            lineRenderer.positionCount = 0; // �� ���� 0���� ����
            return; // �Լ� ����
        }

        for (int i = 0; i < locate.Length; i++)
        {
            lineRenderer.SetPosition(i, locate[i].position);
        }
    }
}
