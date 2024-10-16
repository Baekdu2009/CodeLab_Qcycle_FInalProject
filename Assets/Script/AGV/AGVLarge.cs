using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.UI;

public class AGVLarge : AGVControl
{
    public AGVCart[] movingCarts; // AGV가 이동할 카트 배열
    public bool fullSignalInput;   // 신호 상태를 나타내는 변수
    public GameObject[] pinObject; // PIN 오브젝트 배열
    public Image movingCheck;
    public TMP_Text movingTxt;
    public Image carryingCheck;
    public TMP_Text carryingTxt;

    [Header("AGV Road")]
    public List<Transform> originalPosition;
    public List<Transform> storagePosition;
    public List<Transform> boxPosition;

    [Header("Carts")]
    public GameObject boxCartPrefab;
    public GameObject storageCartPrefab;

    public Transform targetToMove;
    bool isAGVLocateToCart;
    float pinMoveSpeed = 1f;
    bool speedControl;
    bool pinConnected;

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
        LargeAGVUIUpdate();
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
        if (targetToMove != null)
        {

            targetToMove.SetParent(transform); // targetToMove를 AGVLarge의 자식으로 설정

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

        // 이동 경로 초기화
        movingPositions.Clear();

        foreach (var position in storagePosition)
        {
            movingPositions.Add(position);
        }

        while (currentTargetIndex < movingPositions.Count)
        {
            MoveByPoint(); // 경로를 따라 이동

           yield return null; // 다음 프레임까지 대기
        }

        if (targetToMove != null)
        {
            yield return StartCoroutine(UnparentAndPinDown(targetToMove)); // 카트와의 부모 관계 해제
        }
    }

    private IEnumerator MoveToBoxPosition()
    {
        PinMove(true);
        movingPositions.Clear();

        foreach (var position in boxPosition)
        {
            movingPositions.Add(position);
        }

        while (currentTargetIndex < movingPositions.Count)
        {
            MoveByPoint(); // 경로를 따라 이동
            isRotating = false;
            yield return null; // 다음 프레임까지 대기
        }

        if (targetToMove != null)
        {

            yield return StartCoroutine(UnparentAndPinDown(targetToMove)); // 카트와의 부모 관계 해제
        }
    }

    private IEnumerator UnparentAndPinDown(Transform cartTransform)
    {
        // 카트가 연결된 상태로 핀을 내림
      
        PinMove(false);
       
        // 핀이 내려가는 동안 대기
        yield return new WaitForSeconds(0.5f); // 핀 내리는 시간을 기다림 (시간 조정 필요)


        cartTransform.SetParent(null); // 부모 관계 해제
        AGVCart cart = cartTransform.GetComponent<AGVCart>();
        // AGVControl agvControl = GetComponent<AGVControl>();
        if (cart != null)
        {
            cart.SetAGVCallState(false); // AGVCall 상태를 false로 설정
                                         // isRotating = false;
        }

        isMoving = false;
        isRotating = false;
        speedControl = false;
        currentTargetIndex = 0;
        movingPositions.Clear();
        yield return null;
        
        StartCoroutine(ReturnToInitialPosition()); // 초기 위치로 돌아가기
       

        if (cart.isAGVCallOn) // 카트가 다시 AGV 호출 상태인지 확인
        {
            
            AGVtoCartMove(); // AGV를 움직이게 함
        }

    }

    private IEnumerator ReturnToInitialPosition()
    {
        foreach (var position in originalPosition)
        {
            movingPositions.Add(position);
        }
        // Debug.Log("Moving Positions Count: " + movingPositions.Count); // 추가된 로그

        while (currentTargetIndex < movingPositions.Count)
        {
            isMoving = true;
            
            // Debug.Log("이동");
            // Debug.Log("Current Target Index: " + currentTargetIndex); // 현재 인덱스 확인
            MoveByPoint();
            // isRotating = true;

            // Debug.Log("Current Position: " + transform.position);
            // Debug.Log("Target Position: " + movingPositions[currentTargetIndex].position);
            yield return null;// transform.rotation = Quaternion.Euler(0, 0, 0); // 다음 프레임까지 대기
        }
        movingPositions.Clear();
        isRotating = false;
        isMoving = false;
        targetToMove = null; // 목표 카트 초기화

        transform.rotation = Quaternion.identity;
    }

    private void PinMove(bool isCartConnected)
    {
        foreach (var obj in pinObject)
        {
            float originalY = obj.transform.position.y;
            // print("PinMove" + isCartConnected);
            if (isCartConnected)
            {
                obj.transform.localPosition += Vector3.up * 0.19f;
                pinConnected = true;
            }
            else
            {
                obj.transform.position -= Vector3.up * 0.19f;
                pinConnected = false;
            }
        }
    }

    private void FindPinObjects()
    {
        var pinObjects = FindObjectsByName("PIN");
        pinObject = pinObjects.ToArray(); // 배열로 변환하여 저장
    }

    private void LargeAGVUIUpdate()
    {
        if (isMoving)
        {
            movingCheck.color = Color.green;
            movingTxt.text = "Moving";
            movingTxt.color = Color.green;
        }
        else
        {
            movingCheck.color = Color.yellow;
            movingTxt.text = "Stand By";
            movingTxt.color = Color.yellow;
        }

        if (pinConnected)
        {
            carryingCheck.color = Color.green;
            carryingTxt.text = "Cart Carrying";
            carryingTxt.color = Color.green;
        }
        else
        {
            carryingCheck.color = Color.yellow;
            carryingTxt.text = "No Cart";
            carryingTxt.color = Color.yellow;
        }
    }
}
