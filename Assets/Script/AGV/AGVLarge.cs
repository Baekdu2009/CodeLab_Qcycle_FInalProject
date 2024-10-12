using UnityEngine;
using System.Collections.Generic;

public class AGVLarge : AGVControl
{
    public AGVCart[] movingCarts; // AGV가 이동할 카트 배열
    public bool fullSignalInput;   // 신호 상태를 나타내는 변수
    public GameObject[] pinObject; // PIN 오브젝트 배열

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
            AGVMove(targetToMove);

            // 카트와 AGV의 방향 비교
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
            if (obj.name.Contains("PIN")) // 이름에 'PIN'이 포함된 경우
            {
                pinList.Add(obj); // 리스트에 추가
            }
        }

        pinObject = pinList.ToArray(); // 리스트를 배열로 변환하여 저장
    }
}
