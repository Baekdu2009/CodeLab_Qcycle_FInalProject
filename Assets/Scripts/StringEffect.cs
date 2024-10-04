using UnityEngine;

public class StringEffect : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private Vector3[] positions = new Vector3[2];

    void Start()
    {
        positions[0] = new Vector3(0, 0, 0);
        positions[1] = new Vector3(0, 0, 0); // �ʱ� ��ġ ����
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }

    void Update()
    {
        // ���⿡ ���� ������ ������ �߰�
        if (Input.GetKey(KeyCode.Space)) // �����̽��ٸ� ������
        {
            positions[1].y += Time.deltaTime; // ���� �̴� ȿ��
            lineRenderer.SetPositions(positions);
        }
    }
}
