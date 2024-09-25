using UnityEngine;

public class LineRendererExample : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform[] points; // 점 배열


    void Start()
    {
        // LineRenderer 컴포넌트 초기화
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // LineRenderer 속성 설정
        lineRenderer.startWidth = 0f;
        lineRenderer.endWidth = 0f;
        //lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        //lineRenderer.startColor = Color.red;
        //lineRenderer.endColor = Color.red;

        // 선의 점 수 설정
        lineRenderer.positionCount = points.Length;

        // 시작점과 끝점 설정
        UpdateLine();
    }

    void UpdateLine()
    {
        if (points != null && points.Length > 0)
        {
            for (int i = 0; i < points.Length; i++)
            {
                lineRenderer.SetPosition(i, points[i].position);
            }
        }
    }
}