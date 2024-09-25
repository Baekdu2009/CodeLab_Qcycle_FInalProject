using UnityEngine;

public class LineRendererExample : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Vector3[] points; // 점 배열

    void Start()
    {
        // LineRenderer 컴포넌트 초기화
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // LineRenderer 속성 설정
        lineRenderer.startWidth = 0f;
        lineRenderer.endWidth = 0f;

        // points 배열이 null인 경우 초기화
        if (points == null || points.Length == 0)
        {
            points = new Vector3[0]; // 빈 배열로 초기화
        }

        // 선의 점 수 설정
        lineRenderer.positionCount = points.Length;

        // 시작점과 끝점 설정
        // UpdateLine();
    }

    public void UpdateLine()
    {
        if (points != null && points.Length > 0)
        {
            for (int i = 0; i < points.Length; i++)
            {
                lineRenderer.SetPosition(i, points[i]);
            }
        }
    }

    public void UpdateLine(Vector3[] vector)
    {
        if (vector == null || vector.Length == 0)
        {
            lineRenderer.positionCount = 0; // 점 수를 0으로 설정
            return; // 함수 종료
        }

        lineRenderer.positionCount = vector.Length; // 점 수 설정
        for (int i = 0; i < vector.Length; i++)
        {
            lineRenderer.SetPosition(i, vector[i]);
        }
    }
}
