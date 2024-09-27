using UnityEngine;


public class RayGizmo : MonoBehaviour
{
    Canvas gameobject;

    void Update()
    {
        // ���� �߻縦 ���ϴ� ����
        if (Input.GetMouseButtonDown(0)) // ���� ���콺 Ŭ�� ��
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Hit Object: " + hit.collider.name); // ���� ��ü�� �̸� ���
                // ������ �������� ������ ����
                DrawRayGizmo(ray.origin, ray.origin + ray.direction * 100);
            }
        }
    }

    private void DrawRayGizmo(Vector3 start, Vector3 end)
    {
        Debug.DrawRay(start, end, Color.green, 1);
    }

    private void OnDrawGizmos()
    {
        // Gizmos�� �� �信 �׸��� ���� ���
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * 100); // ��� �׷��� �ð�ȭ
        }
    }
}
