using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEditor.SceneManagement;
using UnityEditor;

public class AGVControl_Prev : MonoBehaviour
{
    [Header("AGV 제어")]
    public List<Transform> movingPositions = new List<Transform>();
    public Transform chargingPosition;      // 충전 위치 포지션
    
    public float moveSpeed = 2f;            // 이동속도
    public float rotSpeed = 200f;           // 회전속도
    public float rayDistance = 5f;          // Raycast 거리
    public float avoidanceDistance = 0.2f;  // 회피 거리
    public float batteryCapacity = 100f;    // 배터리 용량

    public bool isMoving;                   // 움직임 여부
    public bool isStopping;                 // 멈춤 여부
    public bool isStandby;                  // 대기 여부
    public bool isNeedtoCharge;             // 충전 필요 여부


    [Header("AGV Road")]
    public List<Transform> PrinterPosition;
    public List<Transform> originalPosition;
    public List<Transform> storagePosition;
    public List<Transform> boxPosition;


    private LineRendererMake lineMake = new LineRendererMake();
    public int currentTargetIndex = 0;     // 현재 목표 포지션 인덱스

    private void Start()
    {
        lineMake = GetComponent<LineRendererMake>();
    }

    private void MakePathForAGV()
    {
        if (lineMake != null)
            lineMake.UpdateLine(movingPositions);
    }

    public void MoveAlongPath()
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

                    isStandby = false;
                }
                else
                {
                    // 모든 목표 위치에 도달한 경우
                    isStandby = true;
                    currentTargetIndex = 0;
                    movingPositions.Clear();
                }
            }
        }
    }

    public void AGVMove(Transform targetPos)
    {
        Vector3 direction = (targetPos.position - transform.position).normalized;

        // 방향 벡터가 유효한 경우에만 회전 및 이동 수행
        if (targetPos != null)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetPos.position, moveSpeed * Time.deltaTime);
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    public void AGVMove()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        isMoving = true;
    }

    public void AGVRotate(Transform targetPos)
    {
        Quaternion rotationOfTarget = targetPos.rotation;
        float angle = GetAngleToTarget(targetPos);
        
        float rotateSpeed;

        if (transform.rotation != rotationOfTarget)
        {
            rotateSpeed = 200f;
        }
        else
        {
            rotateSpeed = 0f;
        }
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
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

    public float GetAngleToTarget(Transform target)
    {
        return Quaternion.Angle(transform.rotation, target.rotation);
    }

    public bool IsFacingTarget(Transform target, float angleThreshold = 1f)
    {
        float angleDifference = Quaternion.Angle(transform.rotation, target.rotation);
        return angleDifference < angleThreshold;
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
            else
            {
                isMoving = true;
            }
        }
    }

    public void AGVStandBy()
    {
        if (isStandby)
        {
            isMoving = false;
        }
    }

    public void AGVtoCharge()
    {
        if (isNeedtoCharge)
            AGVMove(chargingPosition);
    }
}
