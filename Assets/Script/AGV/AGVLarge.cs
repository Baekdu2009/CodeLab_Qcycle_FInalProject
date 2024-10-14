using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class AGVLarge : AGVControl
{
    public AGVCart[] movingCarts; // AGV가 이동할 카트 배열
    public bool fullSignalInput;   // 신호 상태를 나타내는 변수
    public GameObject[] pinObject; // PIN 오브젝트 배열

    public Quaternion initialrotation;
    public Vector3 initialPosition;
    public Transform targetToMove;
    bool isAGVLocateToCart;
    // public bool isCartConnected;
    float pinMoveSpeed = 1f;

    private void Start()
    {
        initialPosition = transform.position; // 초기 위치 저장
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
            if (movingCarts[i].isAGVCallOn) // 카트의 신호가 켜져 있는지 확인
            {
                fullSignalInput = true;
                targetToMove = movingCarts[i].transform;
                isMoving = true;
                break; // 첫 번째 카트만 처리하고 루프 종료
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
        // 카트를 AGVLarge의 자식으로 설정
        PinMove(isCartConnected: false);

        if (targetToMove != null)
        {
            targetToMove.SetParent(transform); // targetToMove를 AGVLarge의 자식으로 설정
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
            yield return UnparentAndPinDown(targetToMove); // 카트와의 부모 관계 해제
        }
    }


    private IEnumerator UnparentAndPinDown(Transform cartTransform)
    {
        PinMove(true); // Pin을 내림

        cartTransform.SetParent(null); // 부모 관계 해제
        movingPositions.Clear(); // 이동 경로 초기화
        
        targetToMove = null; // 목표 카트 초기화
        yield return StartCoroutine(ReturnToInitialPosition()); // 초기 위치로 돌아가기
    }

    private IEnumerator ReturnToInitialPosition()
    {
        // 초기 위치로 부드럽게 이동
        while (Vector3.Distance(transform.position, initialPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
            yield return null; // 다음 프레임까지 대기
        }
        transform.position = initialPosition; // 최종 위치 설정
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
        pinObject = pinObjects.ToArray(); // 배열로 변환하여 저장
    }
}

