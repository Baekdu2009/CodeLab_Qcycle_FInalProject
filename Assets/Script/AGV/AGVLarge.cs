using UnityEngine;
using System.Collections.Generic;

public class AGVLarge : AGVControl
{
    public AGVCart[] movingCarts; // AGV�� �̵��� īƮ �迭
    public bool fullSignalInput;   // ��ȣ ���¸� ��Ÿ���� ����
    public GameObject[] pinObject; // PIN ������Ʈ �迭

    public Transform targetToMove;
    bool isAGVLocateToCart;
    public bool isCartConnected;
    float pinMoveSpeed = 1f;

    private void Start()
    {
        movingPositions.Add(transform);
        FindPinObjects();
    }

    private void Update()
    {
        if (!fullSignalInput)
        {
            targetToMove = null;
        }

        CartSignalCheck();
        AGVtoCartMove();
    }

    private void CartSignalCheck()
    {
        for (int i = 0; i < movingCarts.Length; i++)
        {
            if (movingCarts[i].isAGVCallOn) // īƮ�� ��ȣ�� ���� �ִ��� Ȯ��
            {
                fullSignalInput = true;
                targetToMove = movingCarts[i].transform;
                isMoving = true;
                break; // ù ��° īƮ�� ó���ϰ� ���� ����
            }
            else
            {
                fullSignalInput = false;
            }
        }
    }

    float AGVtoCartDistance()
    {
        return Vector3.Distance(transform.position, targetToMove.position);
    }

    private void AGVtoCartMove()
    {
        if (fullSignalInput)
        {
            AGVMove(targetToMove);

            // īƮ�� AGV�� ���� ��
            float angleDifference = Quaternion.Angle(transform.rotation, targetToMove.rotation);

            if (AGVtoCartDistance() < 0.01f && angleDifference < 1f)
            {
                CartConnect();
            }
        }
    }

    private void CartConnect()
    {
        PinMove();
    }

    private void PinMove()
    {
        foreach (var obj in pinObject)
        {
            float originalY = obj.transform.position.y;
            float movingY = isCartConnected ? -0.18f : 0;

            Vector3 currentPos = obj.transform.position;
            
            if (Mathf.Abs(currentPos.y - movingY) > 0.01f)
            {
                Vector3 pinTargetPos = new Vector3(currentPos.x, movingY, currentPos.z);
                obj.transform.position = Vector3.MoveTowards(currentPos, pinTargetPos, pinMoveSpeed * Time.deltaTime);
            }
            else
            {
                obj.transform.position = new Vector3(currentPos.x, movingY, currentPos.z);
            }
        }
    }

    private void FindPinObjects()
    {
        var pinList = new List<GameObject>();
        var allObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (var obj in allObjects)
        {
            if (obj.name.Contains("PIN")) // �̸��� 'PIN'�� ���Ե� ���
            {
                pinList.Add(obj); // ����Ʈ�� �߰�
            }
        }

        pinObject = pinList.ToArray(); // ����Ʈ�� �迭�� ��ȯ�Ͽ� ����
    }
}
