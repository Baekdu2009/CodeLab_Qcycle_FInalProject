using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineAGV : MonoBehaviour
{
    public LineRenderer lineRenderer; // LineRenderer 참조
    public float speed = 2.0f; // 이동 속도
    private List<int> pointIndices = new List<int>(); // 지나간 점 인덱스 리스트
    private int currentPointIndex = 0; // 현재 점 인덱스
    private float t = 0; // 선을 따라 이동하는 비율
    private bool isMovingForward = true; // 이동 방향


    void Start()
    {
        // 초기 점 인덱스를 리스트에 추가
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            pointIndices.Add(i);
        }
    }

    void Update()
    {
        if (lineRenderer.positionCount > 1) // LineRenderer에 점이 있는 경우
        {
            // 현재 점과 다음 점을 가져옴
            Vector3 startPos = lineRenderer.GetPosition(pointIndices[currentPointIndex]);
            Vector3 endPos = lineRenderer.GetPosition(pointIndices[(currentPointIndex + (isMovingForward ? 1 : -1) + lineRenderer.positionCount) % lineRenderer.positionCount]);

            // 선의 두 점 사이에서 오브젝트 이동
            t += Time.deltaTime * speed / Vector3.Distance(startPos, endPos); // 비율 계산
            transform.position = Vector3.Lerp(startPos, endPos, t); // 선을 따라 이동

            // 다음 점으로 이동
            if (t >= 1)
            {
                t = 0; // 비율 초기화

                // 이동 방향에 따라 인덱스 변경
                if (isMovingForward)
                {
                    currentPointIndex++; // 다음 점으로 이동

                    // 마지막 점에 도달했을 때
                    if (currentPointIndex >= pointIndices.Count - 1)
                    {
                        isMovingForward = false; // 반대 방향으로 이동
                    }
                }
                else
                {
                    currentPointIndex--; // 이전 점으로 이동

                    // 처음 점에 도달했을 때
                    if (currentPointIndex <= 0)
                    {
                        isMovingForward = true; // 다시 앞으로 이동


                    }
                }
            }
        }
    }
}