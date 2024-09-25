using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AGVMovementManual : MonoBehaviour
{
    public float moveSpeed = 5f; // 이동 속도
    public float rotationSpeed = 200f; // 회전 속도
    bool isPosSave = false;

    private LineRendererExample lineRenderer;
    public List<Vector3> savingPosition = new List<Vector3>();
    private int currentTargetIndex = 0; // 현재 목표 위치 인덱스

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRendererExample>(); // LineRendererExample 인스턴스 추가
    }

    void Update()
    {
        if (currentTargetIndex < savingPosition.Count)
        {
            MoveWithRoute(); // 경로를 따라 이동
        }
        else
        {
            AGVMoveByKey(); // 키 입력에 따른 이동
        }

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

    public void MoveWithRoute()
    {
        if (currentTargetIndex < savingPosition.Count)
        {
            Vector3 targetPosition = savingPosition[currentTargetIndex];
            Vector3 direction = (targetPosition - transform.position).normalized;

            // 회전
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            // 이동
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 목표 위치에 도달했는지 확인
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                currentTargetIndex++; // 다음 목표 위치로 이동
            }
        }
    }

    public void RouteCreate()
    {
        if (savingPosition.Count > 0)
        {
            lineRenderer.UpdateLine(savingPosition.ToArray()); // List를 배열로 변환하여 호출
            lineRenderer.GetComponent<LineRenderer>().startColor = Color.yellow;
            lineRenderer.GetComponent<LineRenderer>().endColor = Color.yellow;
        }
    }
}
