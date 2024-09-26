using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AGVMovementManual : MonoBehaviour
{
    bool isPosSave = false;

    private LineRendererMake lineRenderer;
    public LineRendererMake Line { get => lineRenderer;}
    public List<Vector3> savingPosition = new List<Vector3>();
    private int currentTargetIndex = 0; // 현재 목표 위치 인덱스
    float moveSpeed;
    float rotationSpeed;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRendererMake>(); // LineRendererExample 인스턴스 추가
    }

    void Update()
    {
        AGVMoveByKey(); // 키 입력에 따른 이동
        PositionCheck();
    }

    void AGVMoveByKey()
    {
        // W 키를 눌렀을 때 앞으로 이동
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            isPosSave = false;
        }

        // A 키를 눌렀을 때 좌측으로 회전
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
            isPosSave = false;
        }

        // D 키를 눌렀을 때 우측으로 회전
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            isPosSave = false;
        }
    }

    public void PositionCheck()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isPosSave)
        {
            print($"현재 위치: {transform.position}");
            print("위치를 저장하려면 스페이스바를 다시 누르세요.");
            isPosSave = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isPosSave)
        {
            Vector3 currentPos = transform.position;
            savingPosition.Add(currentPos);
            print(currentPos.ToString());
            print("위치가 저장되었습니다");
            isPosSave = false;
        }
    }

    public void RouteCreate()
    {
        if (savingPosition.Count > 0)
        {
            // points 배열을 savingPosition의 크기로 초기화
            lineRenderer.points = new Vector3[savingPosition.Count];

            // savingPosition의 값을 points 배열에 할당
            for (int i = 0; i < savingPosition.Count; i++)
            {
                lineRenderer.points[i] = savingPosition[i];
            }

            // LineRenderer 업데이트
            lineRenderer.UpdateLine(lineRenderer.points); // UpdateLine 메서드 호출
            lineRenderer.GetComponent<LineRenderer>().startColor = Color.yellow;
            lineRenderer.GetComponent<LineRenderer>().endColor = Color.yellow;

            print("라인 생성 완료");
        }
    }
}