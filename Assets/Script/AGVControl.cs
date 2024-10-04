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
    
    public float moveSpeed = 2f;
    public float rotSpeed = 200f;
    public float rayDistance = 5f;          // Raycast �Ÿ�
    public float avoidanceDistance = 0.2f;  // ȸ�� �Ÿ�
    public bool isStopped;                  // ���� ����

    private void Start()
    {

    }

    public void AGVMove(Transform targetPos)
    {
        if (!isStopped)
            transform.position = Vector3.MoveTowards(transform.position, targetPos.position, moveSpeed * Time.deltaTime);
    }

    public void AGVMove()
    {
        if (!isStopped)
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
                isStopped = true;
            }
        }
        else
        {
            isStopped = false;
        }
    }
}
