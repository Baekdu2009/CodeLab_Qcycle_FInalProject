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
    [Header("AGV ����")]
    public List<Transform> movingPositions = new List<Transform>();
    public GameObject Canvas;
    // public Transform chargingPosition;      // ���� ��ġ ������

    public float moveSpeed = 2f;            // �̵��ӵ�
    public float rotSpeed = 200f;           // ȸ���ӵ�
    public float rayDistance = 5f;          // Raycast �Ÿ�
    public float avoidanceDistance = 0.2f;  // ȸ�� �Ÿ�
    public float batteryCapacity = 100f;    // ���͸� �뷮

    public bool isMoving;                   // ������ ����
    public bool isRotating;                // ȸ�� ����

    private LineRendererMake lineMake = new LineRendererMake();
    public int currentTargetIndex = 0;     // ���� ��ǥ ������ �ε���

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

            // ��ǥ ��ġ�� �̵�
            AGVMoveAndRotate(movingPositions[currentTargetIndex]);

            // ��ǥ ��ġ�� �����ߴ��� Ȯ��
            if (Vector3.Distance(transform.position, movingPositions[currentTargetIndex].position) < 0.01f)
            {
                currentTargetIndex++; // ���� ��ǥ�� �̵�

                if (currentTargetIndex >= movingPositions.Count)
                {
                    isMoving = false; // ������ ��ǥ�� ���������Ƿ� ��Ȱ��ȭ
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

            // ��ǥ ��ġ�� �����ߴ��� Ȯ��
            if (Vector3.Distance(transform.position, targetPos.position) < 0.1f)
            {
                isMoving = false;
                isRotating = true; // �̵��� �Ϸ�Ǹ� ȸ�� ����
            }
        }

        if (isRotating)
        {
            AGVRotate(targetPos);

            // ��ǥ ȸ������ �����ߴ��� Ȯ��
            if (Quaternion.Angle(transform.rotation, targetPos.rotation) < 0.1f)
            {
                isRotating = false; // ȸ�� �Ϸ�
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
        // �ð������ +1, �ݽð������ -1
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

        var allObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None); // ��� ���� ������Ʈ �˻�

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
}