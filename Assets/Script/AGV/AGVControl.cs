using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine.UIElements;

public class AGVControl : MonoBehaviour
{
    [Header("AGV 제어")]
    public List<Transform> movingPositions = new List<Transform>();
    public GameObject Canvas;
    // public Transform chargingPosition;      // 충전 위치 포지션

    public float moveSpeed = 2f;            // 이동속도
    public float rotSpeed = 200f;           // 회전속도
    public float rayDistance = 5f;          // Raycast 거리
    public float avoidanceDistance = 0.2f;  // 회피 거리
    public float batteryCapacity = 100f;    // 배터리 용량

    public bool isMoving;                   // 움직임 여부
    public bool isRotating;                // 회전 여부

    private LineRendererMake lineMake = new LineRendererMake();
    public int currentTargetIndex = 0;     // 현재 목표 포지션 인덱스

    private void Start()
    {
        lineMake = GetComponent<LineRendererMake>();

        if (lineMake != null)
            lineMake.UpdateLine(movingPositions);
    }

    public void MoveByPoint()
    {
        if (isMoving && currentTargetIndex < movingPositions.Count)
        {

            // 목표 위치로 이동
            AGVMoveAndRotate(movingPositions[currentTargetIndex]);

            // 목표 위치에 도달했는지 확인
            if (Vector3.Distance(transform.position, movingPositions[currentTargetIndex].position) < 0.01f)
            {
                currentTargetIndex++; // 다음 목표로 이동

                if (currentTargetIndex >= movingPositions.Count)
                {
                    isMoving = false; // 마지막 목표에 도달했으므로 비활성화
                }
            }
        }
    }

    private void MakePathForAGV()
    {
        if (lineMake != null)
            lineMake.UpdateLine(movingPositions);
    }

    public void MovebyPath()
    {
        if (movingPositions != null)
        {
            MakePathForAGV();

            DetectObstacles();

            if (isMoving)
            {

                if (currentTargetIndex < movingPositions.Count)
                {
                    // 목표 위치로 이동
                    AGVMove(movingPositions[currentTargetIndex]);

                    // 목표 위치에 도달했는지 확인
                    if (Vector3.Distance(transform.position, movingPositions[currentTargetIndex].position) < 0.01f)
                    {
                        currentTargetIndex++; // 다음 목표로 이동
                    }
                }
                else
                {
                    // 모든 목표 위치에 도달한 경우
                    currentTargetIndex = 0;
                    movingPositions.Clear();
                }
            }
        }
    }

    public void AGVMoveAndRotate(Transform targetPos)
    {
        if (isMoving)
        {
            AGVMove(targetPos);

            // 목표 위치에 도달했는지 확인
            if (Vector3.Distance(transform.position, targetPos.position) < 0.1f)
            {
                isMoving = false;
                isRotating = true; // 이동이 완료되면 회전 시작
            }
        }

        if (isRotating)
        {
            AGVRotate(targetPos);

            // 목표 회전값에 도달했는지 확인
            if (Quaternion.Angle(transform.rotation, targetPos.rotation) < 0.1f)
            {
                isRotating = false; // 회전 완료
            }
        }
    }

    public void AGVMove(Transform targetPos)
    {
        Vector3 direction = (targetPos.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetPos.position, moveSpeed * Time.deltaTime);
        }
    }

    public void AGVRotate(Transform targetPos)
    {
        Quaternion targetAngle = targetPos.rotation;
        Quaternion currentAngle = transform.rotation;

        if (currentAngle != targetAngle)
        {
            transform.rotation = Quaternion.RotateTowards(currentAngle, targetAngle, rotSpeed * Time.deltaTime);
        }
    }

    public bool IsFacingTarget(Transform target, float angleThreshold = 0.0001f)
    {
        float angleDifference = Quaternion.Angle(transform.rotation, target.rotation);
        return angleDifference < angleThreshold;
    }

    public void AGVRotate(bool isRight)
    {
        // 시계방향이 +1, 반시계방향이 -1
        if (isRight)
            transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
        else
            transform.Rotate(Vector3.up, -rotSpeed * Time.deltaTime);
    }

    public float GetDistanceToTarget(Transform target)
    {
        return Vector3.Distance(transform.position, target.position);
    }

    public List<GameObject> FindObjectsByName(string namePattern)
    {
        var foundObjects = new List<GameObject>();

        var allObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None); // 모든 게임 오브젝트 검색

        foreach (var obj in allObjects)
        {
            if (obj.name.Contains(namePattern))
            {
                foundObjects.Add(obj);
            }
        }

        return foundObjects;
    }
    public void DetectObstacles()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward) * rayDistance;

        if (Physics.Raycast(transform.position, forward, out hit, rayDistance))
        {
            if (hit.collider != null && hit.collider.CompareTag("Person"))      // Person 태그 확인
            {
                isMoving = false;
            }
        }
        else
        {
            isMoving = true;
        }
    }
}