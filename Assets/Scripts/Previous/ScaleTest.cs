using UnityEngine;

public class ScaleTest : MonoBehaviour
{
    public Transform[] positions; // 스케일과 회전을 적용할 위치 배열
    private LineRenderer lineRenderer; // LineRenderer 컴포넌트

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        // LineRenderer가 없을 경우 경고 메시지 출력
        if (lineRenderer == null)
        {
            Debug.LogWarning("LineRenderer 컴포넌트가 없습니다. 추가해주세요.");
            return; // 컴포넌트가 없으면 더 이상 진행하지 않음
        }

        // LineRenderer 속성 설정
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material.color = Color.red;

        // 초기 선 그리기
        LineMake();
    }

    void Update()
    {
        // 매 프레임마다 필요한 작업을 여기에 추가
    }

    public void BtnClick()
    {
        LineMake(); // 버튼 클릭 시 선을 그리기
    }

    void LineMake()
    {
        if (lineRenderer != null && positions.Length > 0)
        {
            lineRenderer.positionCount = positions.Length; // 포지션 수 설정
            for (int i = 0; i < positions.Length; i++)
            {
                lineRenderer.SetPosition(i, positions[i].position); // 위치 설정
            }
        }
    }
}
