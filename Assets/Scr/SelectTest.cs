using UnityEngine;

public class SelectTest : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // ���� ���콺 ������ ��ġ(Input.mousePosition)���� ȭ�� ������ ���� ���� �������� ��ȯ�Ͽ� Ray�� �����մϴ�.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Ray �ð�ȭ
            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.blue);
            // Debug.DrawRay(Vector3 start, Vector3 direction, Color color, float duration = 0.0f, bool depthTest = true);
        }
    }
}
