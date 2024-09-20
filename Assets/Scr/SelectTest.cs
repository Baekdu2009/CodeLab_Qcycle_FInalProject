using UnityEngine;

public class SelectTest : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // 현재 마우스 포인터 위치(Input.mousePosition)에서 화면 공간의 점을 월드 공간으로 변환하여 Ray를 생성합니다.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Ray 시각화
            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.blue);
            // Debug.DrawRay(Vector3 start, Vector3 direction, Color color, float duration = 0.0f, bool depthTest = true);
        }
    }
}
