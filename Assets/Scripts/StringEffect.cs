using UnityEngine;

public class StringEffect : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private Vector3[] positions = new Vector3[2];

    void Start()
    {
        positions[0] = new Vector3(0, 0, 0);
        positions[1] = new Vector3(0, 0, 0); // 초기 위치 설정
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }

    void Update()
    {
        // 여기에 실이 뽑히는 로직을 추가
        if (Input.GetKey(KeyCode.Space)) // 스페이스바를 누르면
        {
            positions[1].y += Time.deltaTime; // 실을 뽑는 효과
            lineRenderer.SetPositions(positions);
        }
    }
}
