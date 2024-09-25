using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AGVMoving : MonoBehaviour
{
    public float moveSpeed = 2f; // 이동 속도
    public float rotationSpeed = 200f; // 회전 속도

    private bool isPosSave = false;
    private LineRendererExample lineRenderer;
    public List<Vector3> savingPosition = new List<Vector3>();
    private int currentPointIndex = 0; // 현재 목표 점 인덱스
    private bool movingForward = true; // 이동 방향 추적
    private bool isRotating = false; // 회전 중인지 확인

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRendererExample>(); // LineRendererExample 인스턴스 추가
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
            lineRenderer.GetComponent<LineRenderer>().startColor = Color.red;
            lineRenderer.GetComponent<LineRenderer>().endColor = Color.red;

            print("라인 생성 완료");
        }
    }

    public void MoveOrigin()
    {
        // AGV를 첫 번째 점 위치로 초기화
        if (lineRenderer.points.Length > 0)
        {
            transform.position = lineRenderer.points[0];
        }
        else
        {
            Debug.LogError("LineRendererExample이 초기화되지 않았거나 점이 없습니다.");
        }
    }

    public void MoveAuto()
    {
        if (lineRenderer.points.Length > 0)
        {
            StartCoroutine(MoveAlongLine()); // 자동으로 이동
        }
        else
        {
            Debug.LogError("LineRendererExample이 초기화되지 않았거나 점이 없습니다.");
        }
    }

    private IEnumerator MoveAlongLine()
    {
        while (true) // 무한 루프, 필요에 따라 종료 조건 추가
        {
            // 현재 목표 점 가져오기
            if (lineRenderer.points.Length == 0) yield break; // 점이 없으면 종료

            Vector3 targetPoint = lineRenderer.points[currentPointIndex];

            // 목표 점을 바라보도록 회전
            if (isRotating)
            {
                Vector3 direction = (targetPoint - transform.position).normalized;
                if (direction.magnitude > 0.01f) // 방향 벡터가 0이 아닐 때만 회전
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

                    // 회전이 완료되면 이동 시작
                    if (Quaternion.Angle(transform.rotation, lookRotation) < 1f)
                    {
                        isRotating = false; // 회전 완료
                    }
                }
            }
            else
            {
                // AGV와 목표 점 사이의 거리 계산
                float step = moveSpeed * Time.deltaTime; // moveSpeed는 AGVMoving에서 상속받음
                transform.position = Vector3.MoveTowards(transform.position, targetPoint, step);

                // 목표 점에 도달하면 다음 점으로 이동
                if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
                {
                    isRotating = true; // 회전 시작

                    if (movingForward)
                    {
                        currentPointIndex++;

                        // 마지막 점에 도달했으면 방향을 반전
                        if (currentPointIndex >= lineRenderer.points.Length)
                        {
                            currentPointIndex = lineRenderer.points.Length - 2; // 마지막 점에서 이전 점으로 설정
                            movingForward = false; // 방향을 반전
                        }
                    }
                    else
                    {
                        currentPointIndex--;

                        // 처음 점에 도달했으면 방향을 반전
                        if (currentPointIndex < 0)
                        {
                            currentPointIndex = 1; // 처음 점에서 다음 점으로 설정
                            movingForward = true; // 방향을 반전
                        }
                    }
                }
            }

            yield return new WaitForEndOfFrame(); // 다음 프레임까지 대기
        }
    }

}
