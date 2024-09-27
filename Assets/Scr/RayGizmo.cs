using UnityEngine;


public class RayGizmo : MonoBehaviour
{
    Canvas gameobject;

    void Update()
    {
        // 레이 발사를 원하는 조건
        if (Input.GetMouseButtonDown(0)) // 왼쪽 마우스 클릭 시
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Hit Object: " + hit.collider.name); // 맞은 물체의 이름 출력
                // 레이의 시작점과 끝점을 저장
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
        // Gizmos를 씬 뷰에 그리기 위해 사용
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * 100); // 길게 그려서 시각화
        }
    }
}
