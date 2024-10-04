using UnityEngine;

public class ScaleTest : MonoBehaviour
{
    public Transform[] positions; // �����ϰ� ȸ���� ������ ��ġ �迭
    private LineRenderer lineRenderer; // LineRenderer ������Ʈ

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        // LineRenderer�� ���� ��� ��� �޽��� ���
        if (lineRenderer == null)
        {
            Debug.LogWarning("LineRenderer ������Ʈ�� �����ϴ�. �߰����ּ���.");
            return; // ������Ʈ�� ������ �� �̻� �������� ����
        }

        // LineRenderer �Ӽ� ����
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material.color = Color.red;

        // �ʱ� �� �׸���
        LineMake();
    }

    void Update()
    {
        // �� �����Ӹ��� �ʿ��� �۾��� ���⿡ �߰�
    }

    public void BtnClick()
    {
        LineMake(); // ��ư Ŭ�� �� ���� �׸���
    }

    void LineMake()
    {
        if (lineRenderer != null && positions.Length > 0)
        {
            lineRenderer.positionCount = positions.Length; // ������ �� ����
            for (int i = 0; i < positions.Length; i++)
            {
                lineRenderer.SetPosition(i, positions[i].position); // ��ġ ����
            }
        }
    }
}
