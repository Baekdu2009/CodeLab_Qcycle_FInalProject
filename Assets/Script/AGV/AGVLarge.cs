using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class AGVLarge : AGVControl
{
    public AGVCart[] movingCarts; // AGV�� �̵��� īƮ �迭
    public bool fullSignalInput;   // ��ȣ ���¸� ��Ÿ���� ����
    public GameObject[] pinObject; // PIN ������Ʈ �迭

    public Transform targetToMove;
    bool isAGVLocateToCart;
    float pinMoveSpeed = 1f;
    bool speedControl;
    bool pinDown;

    private void Start()
    {
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
        if (targetToMove != null)
        {

            targetToMove.SetParent(transform); // targetToMove�� AGVLarge�� �ڽ����� ����
            

            if (targetToMove.CompareTag("storageCart"))
            {
                if (!speedControl)
                {
                    speedControl = true;
                    StartCoroutine(MoveTostoragePosition());
                }
            }
            if (targetToMove.CompareTag("BoxCart"))
            {
                if (!speedControl)
                {
                    speedControl = true;

                    StartCoroutine(MoveToBoxPosition());
                }
            }
        }


    }

    private IEnumerator MoveTostoragePosition()
    {
        PinMove(true);

        // �̵� ��� �ʱ�ȭ
        movingPositions.Clear();

        foreach (var position in storagePosition)
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
            yield return StartCoroutine(UnparentAndPinDown(targetToMove)); // īƮ���� �θ� ���� ����

        }
    }
    private IEnumerator MoveToBoxPosition()
    {

        PinMove(true);
        movingPositions.Clear();

        foreach (var position in boxPosition)
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

            yield return StartCoroutine(UnparentAndPinDown(targetToMove)); // īƮ���� �θ� ���� ����

        }
    }




    private IEnumerator UnparentAndPinDown(Transform cartTransform)
    {
        // īƮ�� ����� ���·� ���� ����
      
        PinMove(false);
       
        // ���� �������� ���� ���
        yield return new WaitForSeconds(0.5f); // �� ������ �ð��� ��ٸ� (�ð� ���� �ʿ�)


        cartTransform.SetParent(null); // �θ� ���� ����
        AGVCart cart = cartTransform.GetComponent<AGVCart>();
        // AGVControl agvControl = GetComponent<AGVControl>();
        if (cart != null)
        {
            cart.SetAGVCallState(false); // AGVCall ���¸� false�� ����
                                         // isRotating = false;
        }

        isMoving = false;
        isRotating = false;
        speedControl = false;
        currentTargetIndex = 0;
        movingPositions.Clear();
        yield return null; // StartCoroutine(ReturnToInitialPosition()); // �ʱ� ��ġ�� ���ư���
       

        if (cart.isAGVCallOn) // īƮ�� �ٽ� AGV ȣ�� �������� Ȯ��
        {
            
            AGVtoCartMove(); // AGV�� �����̰� ��
        }

    }

    private IEnumerator ReturnToInitialPosition()
    {
        foreach (var position in originalPosition)
        {
            movingPositions.Add(position);
        }
        // Debug.Log("Moving Positions Count: " + movingPositions.Count); // �߰��� �α�

        while (currentTargetIndex < movingPositions.Count)
        {
            isMoving = true;
            
            // Debug.Log("�̵�");
            // Debug.Log("Current Target Index: " + currentTargetIndex); // ���� �ε��� Ȯ��
            MoveAlongPath();
            // isRotating = true;

            // Debug.Log("Current Position: " + transform.position);
            // Debug.Log("Target Position: " + movingPositions[currentTargetIndex].position);
            yield return null;// transform.rotation = Quaternion.Euler(0, 0, 0); // ���� �����ӱ��� ���
        }
        movingPositions.Clear();
        isRotating = false;
        isMoving = false;
        targetToMove = null; // ��ǥ īƮ �ʱ�ȭ
       
        
    }



    private void PinMove(bool isCartConnected)
    {
        foreach (var obj in pinObject)
        {
            float originalY = obj.transform.position.y;
            print("PinMove" + isCartConnected);
            if (isCartConnected)
            {
                obj.transform.localPosition += Vector3.up * 0.19f;
            }
            else
            {
                obj.transform.position -= Vector3.up * 0.19f;
            }
        }
    }

    private void FindPinObjects()
    {
        var pinObjects = FindObjectsByName("PIN");
        pinObject = pinObjects.ToArray(); // �迭�� ��ȯ�Ͽ� ����
    }
}
