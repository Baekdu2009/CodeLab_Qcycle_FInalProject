using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEditor.SceneManagement;

public class AGVControl : MonoBehaviour
{
    [Header("AGV ����")]
    public Transform[] movingPositions;     // �̵� ��ġ ������
    public Transform chargingLocation;      // ���� ��ġ ������
    
    public float moveSpeed = 2f;            // �̵��ӵ�
    public float rotSpeed = 200f;           // ȸ���ӵ�
    public float rayDistance = 5f;          // Raycast �Ÿ�
    public float avoidanceDistance = 0.2f;  // ȸ�� �Ÿ�
    public float batteryCapacity = 100f;    // ���͸� �뷮

    public bool isMoving;                   // ������ ����
    public bool isStopping;                 // ���� ����
    public bool isStandby;                  // ��� ����
    public bool isNeedtoCharge;             // ���� �ʿ� ����

    private LineRendererMake lineMake = new LineRendererMake();
    private int currentTargetIndex = 0;     // ���� ��ǥ ������ �ε���

    private void Start()
    {
        lineMake.UpdateLine(movingPositions);
    }

    public void MoveAlongPath()
    {
        if (currentTargetIndex < movingPositions.Length)
        {
            // ��ǥ ��ġ�� �̵�
            AGVMove(movingPositions[currentTargetIndex]);

            // ��ǥ ��ġ�� �����ߴ��� Ȯ��
            if (Vector3.Distance(transform.position, movingPositions[currentTargetIndex].position) < 0.01f)
            {
                currentTargetIndex++; // ���� ��ǥ�� �̵�
            }
        }
        else
        {
            // ��� ��ǥ ��ġ�� ������ ���
            isStandby = true;
            currentTargetIndex = 0;
        }
    }

    public void AGVMove(Transform targetPos)
    {
        if (!isStopping)
            transform.position = Vector3.MoveTowards(transform.position, targetPos.position, moveSpeed * Time.deltaTime);
    }

    public void AGVMove()
    {
        if (!isStopping)
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    public void AGVRotate(bool isRight)
    {
        // �ð������ +1, �ݽð������ -1
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
            if (hit.collider != null && hit.collider.CompareTag("Person"))      // Person �±� Ȯ��
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
        if (batteryCapacity <= 0)
            AGVMove(chargingLocation);
    }
}
