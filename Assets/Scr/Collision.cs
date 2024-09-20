using UnityEngine;
using System.Collections;

public class Collision : MonoBehaviour
{
    float detectionDistance = 5f; // 감지 거리 설정

    public float moveSpeed = 5f; // 이동 속도
    private bool isMoving = true; // 이동 여부
    private bool isColliding = false; // 충돌 상태
    private float waitTime = 5f; // 대기 시간
    private float timer = 0f; // 타이머

    void Start()
    {

    }

    void Update()
    {
        Vector3 position = transform.position;


        // DrawRay를 이용해서 선에서만 센서 작용
        // 앞, 뒤, 왼쪽, 오른쪽
        Debug.DrawRay(position, transform.forward, Color.blue);     // 앞
        Debug.DrawRay(position, -transform.forward, Color.blue);    // 뒤
        Debug.DrawRay(position, transform.right, Color.blue);       // 오른쪽
        Debug.DrawRay(position, -transform.right, Color.blue);      // 왼쪽

        // DrawRay 이용 대각선
        Debug.DrawRay(position, (transform.forward + transform.right).normalized, Color.blue);
        Debug.DrawRay(position, (transform.forward - transform.right).normalized, Color.blue);
        Debug.DrawRay(position, (-transform.forward + transform.right).normalized, Color.blue);
        Debug.DrawRay(position, (-transform.forward - transform.right).normalized, Color.blue);

        // 각 방향에 대해 레이캐스트 충돌 체크
        CheckCollision(transform.right);
        CheckCollision(transform.forward);
        CheckCollision(-transform.forward);
        CheckCollision(-transform.right);
        CheckCollision2((transform.forward + transform.right).normalized);
        CheckCollision2((transform.forward - transform.right).normalized);
        CheckCollision2((-transform.forward + transform.right).normalized);
        CheckCollision2((-transform.forward - transform.right).normalized);


    }
    private void CheckCollision(Vector3 direction)
    {
        // 레이캐스트를 통해 충돌 체크
        if (Physics.Raycast(transform.position, direction, out RaycastHit hitinfo, detectionDistance))
        {
            // 충돌이 있을 경우
            if (hitinfo.collider.tag == "target")
            {
                Debug.Log("충돌객체 있음");
                isMoving = false; // 멈춤 상태로 설정
                isColliding = true; // 충돌 상태로 설정
                timer = 0f; // 타이머 초기화
            }
        }
        else
        {
            // 충돌이 없을 경우
            if (isColliding)
            {
                timer += Time.deltaTime; // 타이머 증가
                if (timer >= waitTime)
                {
                    isMoving = true; // 이동 가능 상태로 설정
                    isColliding = false; // 충돌 상태 해제
                }
            }
        }

            // 물체 이동
            if (isMoving)
            {
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
       
    }
    private void CheckCollision2(Vector3 direction)
    {
        // 레이캐스트를 통해 충돌 체크
        if (Physics.Raycast(transform.position, direction, out RaycastHit hitinfo, detectionDistance))
        {
            // 충돌이 있을 경우
            if (hitinfo.collider.tag == "target")
            {
                Debug.Log("대각선 충돌객체 있음");
                isMoving = false; // 멈춤 상태로 설정
                isColliding = true; // 충돌 상태로 설정
                timer = 0f; // 타이머 초기화
            }
        }
        else
        {
            // 충돌이 없을 경우
            if (isColliding)
            {
                timer += Time.deltaTime; // 타이머 증가
                if (timer >= waitTime)
                {
                    isMoving = true; // 이동 가능 상태로 설정
                    isColliding = false; // 충돌 상태 해제
                }
            }
        }

    }
    }






