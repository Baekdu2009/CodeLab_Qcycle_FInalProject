using UnityEngine;

public class LineRendererMakeTransform : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform[] points; // Transform 배열로 변경

    void Start()
    {
        // LineRenderer 컴포넌트 초기화
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // LineRenderer 속성 설정
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

        // points 배열이 null인 경우 초기화
        if (points == null || points.Length == 0)
        {
            points = new Transform[0]; // 빈 배열로 초기화
        }

        // 선의 점 수 설정
        lineRenderer.positionCount = points.Length;

        // 시작점과 끝점 설정
        UpdateLine();
    }

    public void UpdateLine()
    {
        if (points != null && points.Length > 0)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i] != null) // Transform이 null이 아닐 경우
                {
                    lineRenderer.SetPosition(i, points[i].position); // 각 Transform의 위치 설정
                }
            }
        }
    }

    public void UpdateLine(Transform[] transforms)
    {
        if (transforms == null || transforms.Length == 0)
        {
            lineRenderer.positionCount = 0; // 점 수를 0으로 설정
            return; // 함수 종료
        }

        lineRenderer.positionCount = transforms.Length; // 점 수 설정
        for (int i = 0; i < transforms.Length; i++)
        {
            if (transforms[i] != null) // Transform이 null이 아닐 경우
            {
                lineRenderer.SetPosition(i, transforms[i].position); // 각 Transform의 위치 설정
            }
        }
    }
}
