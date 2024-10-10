using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEditor.SceneManagement;
using UnityEditor;

public class AGVControl : MonoBehaviour
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

    private LineRendererMake lineMake = new LineRendererMake();
    public int currentTargetIndex = 0;     // 현재 목표 포지션 인덱스

    private void Start()
    {
        lineMake = GetComponent<LineRendererMake>();

        if(lineMake != null )
            lineMake.UpdateLine(movingPositions);
    }

    public void MoveAlongPath()
    {
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
            }
        }
    }

    public void AGVMove(Transform targetPos)
    {
        Vector3 direction = (targetPos.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.position = Vector3.MoveTowards(transform.position, targetPos.position, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotSpeed * Time.deltaTime);
        isMoving = true;
    }

    public void AGVMove()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        isMoving = true;
    }

    public void AGVRotate(bool isRight)
    {
        // 시계방향이 +1, 반시계방향이 -1
        if (isRight)
            transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
        else
            transform.Rotate(Vector3.up, -rotSpeed * Time.deltaTime);

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
