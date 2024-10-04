using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class AGVControl : MonoBehaviour
{
    [Header("AGV ����")]
    public Transform[] movingPositions;
    
    public float moveSpeed = 2f;            // �̵��ӵ�
    public float rotSpeed = 200f;           // ȸ���ӵ�
    public float rayDistance = 5f;          // Raycast �Ÿ�
    public float avoidanceDistance = 0.2f;  // ȸ�� �Ÿ�
    public bool isMoving;                   // ������ ����
    public bool isStopping;                 // ���� ����
    public bool isStandby;                  // ��� ����

    private void Start()
    {

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

    }

    public void AGVtoCharge()
    {

    }
}
