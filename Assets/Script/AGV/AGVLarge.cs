using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class AGVLarge : AGVControl
{
    public AGVCart[] movingCarts; // AGV�� �̵��� īƮ �迭
    public bool fullSignalInput;   // ��ȣ ���¸� ��Ÿ���� ����
    public GameObject[] pinObject; // PIN ������Ʈ �迭

    public Quaternion initialrotation;
    public Vector3 initialPosition;
    public Transform targetToMove;
    bool isAGVLocateToCart;
    // public bool isCartConnected;
    float pinMoveSpeed = 1f;

    private void Start()
    {
        initialPosition = transform.position; // �ʱ� ��ġ ����
        initialrotation = transform.rotation;
        // movingPositions.Add(transform);
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
            if (GetDistanceToTarget(targetToMove) > 0.01f || !IsFacingTarget(targetToMove))
            {
                AGVMoveAndRotate(targetToMove);
            }
            else if (GetDistanceToTarget(targetToMove) < 0.01f && IsFacingTarget(targetToMove))
            {
                CartConnect();
            }
        }
    }

    private void CartConnect()
    {
        // īƮ�� AGVLarge�� �ڽ����� ����
        PinMove(isCartConnected: false);

        if (targetToMove != null)
        {
            targetToMove.SetParent(transform); // targetToMove�� AGVLarge�� �ڽ����� ����
            ReadRoutAndMove();
        }

    }
    private void ReadRoutAndMove()
    {
        AGVCart cart = targetToMove.GetComponent<AGVCart>();
        if (cart != null)
        {
            List<Vector3> route = cart.GetRoute();
            movingPositions.Clear();
            foreach (Vector3 point in route)
            {
                Transform tempTransform = new GameObject("RoutePoint").transform;
                tempTransform.position = point;
                movingPositions.Add(tempTransform);
            }
            StartCoroutine(FollowRoute());
        }
    }


    private IEnumerator FollowRoute()
    {
        while (currentTargetIndex < movingPositions.Count)
        {
            MoveAlongPath();
            yield return null;
        }

        if (targetToMove != null)
        {
            yield return UnparentAndPinDown(targetToMove); // īƮ���� �θ� ���� ����
        }
    }


    private IEnumerator UnparentAndPinDown(Transform cartTransform)
    {
        PinMove(true); // Pin�� ����

        cartTransform.SetParent(null); // �θ� ���� ����
        movingPositions.Clear(); // �̵� ��� �ʱ�ȭ
        
        targetToMove = null; // ��ǥ īƮ �ʱ�ȭ
        yield return StartCoroutine(ReturnToInitialPosition()); // �ʱ� ��ġ�� ���ư���
    }

    private IEnumerator ReturnToInitialPosition()
    {
        // �ʱ� ��ġ�� �ε巴�� �̵�
        while (Vector3.Distance(transform.position, initialPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
            yield return null; // ���� �����ӱ��� ���
        }
        transform.position = initialPosition; // ���� ��ġ ����
        transform.rotation = initialrotation;

        currentTargetIndex = 0;
    }



    private void PinMove(bool isCartConnected)
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
        var pinObjects = FindObjectsByName("PIN");
        pinObject = pinObjects.ToArray(); // �迭�� ��ȯ�Ͽ� ����
    }
}

