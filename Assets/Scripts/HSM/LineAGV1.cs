using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.Animations;

public class LineAGV1 : MonoBehaviour
{
    public static LineAGV1 instance;    // 인스턴스 생성(외부에서 사용가능 권한 부여)
    public LineRenderer lineRenderer1; // LineRenderer1 참조
    public GameObject AGV1;
    public Transform target1; // 목표 위치 (Filament)
    public Transform target2; // 목표 위치 (3D Print)
    public Button moveButton1;
    public Button moveButton2;
    public float moveSpeed = 2.0f; // 이동 속도
    public bool isMovingForward = true; // 이동 방향
    private List<int> pointIndices = new List<int>(); // 지나간 점 인덱스 리스트
    private int currentPointIndex = 0; // 현재 점 인덱스
    private float t = 0; // 선을 따라 이동하는 비율

    public void Awake()            // start 보다 먼저 실행
    {
        if (instance == null)
            instance = this;
    }


    void Start()
    {
        /*Vector3 pos;
        pos = this.gameObject.transform.position;
        // Debug.Log(pos);*/

        /*  // 버튼 클릭시 각기 다른 위치로 이동
          moveButton1.onClick.AddListener(() => MoveAGV1ToTarget(target1));
          moveButton2.onClick.AddListener(() => MoveAGV1ToTarget(target2));
  */
        // 초기 점 인덱스를 리스트에 추가
        for (int i = 0; i < lineRenderer1.positionCount; i++)
        {
            pointIndices.Add(i);
        }


    }

    void Update()
    {

        if (isMovingForward && target1 != null)
        {
            if (lineRenderer1.positionCount > 1) // LineRenderer에 점이 있는 경우
            {
                // 현재 점과 다음 점을 가져옴
                Vector3 startPos = lineRenderer1.GetPosition(pointIndices[currentPointIndex]);
                transform.LookAt(startPos);

                // 다음 점 인덱스 계산
                int nextPointIndex = currentPointIndex + 1;

                //마지막 점인지 체크
                if (nextPointIndex < pointIndices.Count)
                {
                    Vector3 endPos = lineRenderer1.GetPosition(pointIndices[nextPointIndex]);
                    transform.LookAt(endPos);

                    // 선의 두 점 사이에서 오브젝트 이동      비율 계산
                    t += Time.deltaTime * moveSpeed / Vector3.Distance(startPos, endPos);
                    transform.position = Vector3.Lerp(startPos, endPos, t); // 선 따라 이동

                    // 다음 점으로 이동
                    if (t >= 1)
                    {
                        t = 0; // 비율 초기화
                        currentPointIndex++; // 다음 점으로 이동
                    }
                }
                else
                {
                    // 마지막 점에 도달했을 때
                    isMovingForward = false; // 이동 멈춤
                    Debug.Log("AGV1 멈춤");
                }
                // 마지막 점인지 체크
                if (nextPointIndex < pointIndices.Count)
                {
                    Vector3 endPos = lineRenderer1.GetPosition(pointIndices[nextPointIndex]);
                }
            }
        }
    }
    /*public void MoveAGV1ToTarget(Transform target)
 {
     target1 = target; // 목표 위치 설정
     isMovingForward = true; // 버튼 클릭 시 이동 시작
 }*/
}