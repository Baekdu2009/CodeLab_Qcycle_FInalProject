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
            
            if (targetToMove.CompareTag("PrintCart"))
            {
                StartCoroutine(MoveToPrinterPositions());
                // ReadRouteAndMove();
            }
        }


    }

    private IEnumerator MoveToPrinterPositions()
    {
        // �̵� ��� �ʱ�ȭ
        movingPositions.Clear();

        foreach (var position in PrinterPosition)
        {
            movingPositions.Add(position); // PrinterPosition�� �̵� ��ο� �߰�
        }

        while (currentTargetIndex < movingPositions.Count)
        {
            MoveAlongPath(); // ��θ� ���� �̵�
            yield return null; // ���� �����ӱ��� ���
        }

        if (targetToMove != null)
        {
            yield return UnparentAndPinDown(targetToMove); // īƮ���� �θ� ���� ����
        }
    }




    private IEnumerator UnparentAndPinDown(Transform cartTransform)
    {
        PinMove(true); // īƮ�� ����� ���·� ���� ����

        // ���� �������� ���� ���
        yield return new WaitForSeconds(0.5f); // �� ������ �ð��� ��ٸ� (�ð� ���� �ʿ�)


        cartTransform.SetParent(null); // �θ� ���� ����

        targetToMove = null; // ��ǥ īƮ �ʱ�ȭ
        AGVCart cart = cartTransform.GetComponent<AGVCart>();
        if (cart != null)
        {
            cart.SetAGVCallState(false); // AGVCall ���¸� false�� ����
        }
        yield return StartCoroutine(ReturnToInitialPosition()); // �ʱ� ��ġ�� ���ư���
    }

    private IEnumerator ReturnToInitialPosition()
    {
        movingPositions.Clear();

        foreach (var position in originalPosition)
        {
            movingPositions.Add(position);
        }

        while (currentTargetIndex < movingPositions.Count)
        {
            MoveAlongPath();
        }
        yield return null; // ���� �����ӱ��� ���
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