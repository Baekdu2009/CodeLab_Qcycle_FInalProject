using UnityEngine;

public class LineRendererMake : MonoBehaviour
{
    LineRenderer lineRenderer;
    public float lineWidth = 0.2f;
    private Transform[] transformPos;
    public Vector3[] points; // 점 배열

    void Start()
    {
        // LineRenderer 컴포넌트 초기화
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // LineRenderer 속성 설정
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

    }
    
    public void UpdateLine(Vector3[] vector)
    {
        lineRenderer.positionCount = vector.Length; // 점 수 설정

        if (vector == null || vector.Length == 0)
        {
            lineRenderer.positionCount = 0; // 점 수를 0으로 설정
            return; // 함수 종료
        }

        for (int i = 0; i < vector.Length; i++)
        {
            lineRenderer.SetPosition(i, vector[i]);
        }
    }
    public void UpdateLine(Transform[] locate)
    {
        lineRenderer.positionCount = locate.Length; // 점 수 설정

        if (locate == null || locate.Length == 0)
        {
            lineRenderer.positionCount = 0; // 점 수를 0으로 설정
            return; // 함수 종료
        }

        for (int i = 0; i < locate.Length; i++)
        {
            lineRenderer.SetPosition(i, locate[i].position);
        }
    }
}
