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
    public void UpdateLine(Transform[] locate)
    {
        if (locate == null || locate.Length == 0)
        {
            lineRenderer.positionCount = 0; // �� ���� 0���� ����
            return; // �Լ� ����
        }

        lineRenderer.positionCount = locate.Length; // �� �� ����
        for (int i = 0; i < locate.Length; i++)
        {
            lineRenderer.SetPosition(i, locate[i].position);
        }
    }
}
