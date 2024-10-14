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
            
            if (targetToMove.CompareTag("PrintCart"))
            {
                StartCoroutine(MoveToPrinterPositions());
                // ReadRouteAndMove();
            }
        }


    }

    private IEnumerator MoveToPrinterPositions()
    {
        // 이동 경로 초기화
        movingPositions.Clear();

        foreach (var position in PrinterPosition)
        {
            movingPositions.Add(position); // PrinterPosition을 이동 경로에 추가
        }

        while (currentTargetIndex < movingPositions.Count)
        {
            MoveAlongPath(); // 경로를 따라 이동
            yield return null; // 다음 프레임까지 대기
        }

        if (targetToMove != null)
        {
            yield return UnparentAndPinDown(targetToMove); // 카트와의 부모 관계 해제
        }
    }




    private IEnumerator UnparentAndPinDown(Transform cartTransform)
    {
        PinMove(true); // 카트가 연결된 상태로 핀을 내림

        // 핀이 내려가는 동안 대기
        yield return new WaitForSeconds(0.5f); // 핀 내리는 시간을 기다림 (시간 조정 필요)


        cartTransform.SetParent(null); // 부모 관계 해제

        targetToMove = null; // 목표 카트 초기화
        AGVCart cart = cartTransform.GetComponent<AGVCart>();
        if (cart != null)
        {
            cart.SetAGVCallState(false); // AGVCall 상태를 false로 설정
        }
        yield return StartCoroutine(ReturnToInitialPosition()); // 초기 위치로 돌아가기
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
        yield return null; // 다음 프레임까지 대기
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