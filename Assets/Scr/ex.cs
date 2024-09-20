/*using UnityEngine;
using System.Collections;

public class MoveAlongLine : MonoBehaviour
{
    Vector3 point1 = new Vector3(0, 0, 0);     // 원점
    Vector3 point2 = Vector3.right * 100;      // X축 방향으로 100단위만큼 이동한 점을 나타냄
    RaycastHit hitinfo;                        // 초기화가 되어있지 않음, 충돌 정보를 저장

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        // RaycastHit hitinfo;
        Debug.Log(point2);
        Debug.DrawRay(transform.position, point2, Color.red);           // transform.position은 현재 스크립트가 부착된 오브젝트의 위치를 나타냄

        if (Physics.Raycast(transform.position, point2, out hitinfo))
        {
            Debug.Log("충돌객체 있음");
            if (hitinfo.collider.tag == "target")
                hitinfo.transform.Rotate(Time.deltaTime * 45, Time.deltaTime * 45, Time.deltaTime);
        }

    }

}*/
/*using UnityEngine;

public class RayDrawer : MonoBehaviour
{
    void Update()
    {
        // 기본 위치
        Vector3 position = transform.position;

        // 앞, 뒤, 왼쪽, 오른쪽 레이
        Debug.DrawRay(position, transform.forward, Color.red); // 앞
        Debug.DrawRay(position, -transform.forward, Color.blue); // 뒤
        Debug.DrawRay(position, -transform.right, Color.green); // 왼쪽
        Debug.DrawRay(position, transform.right, Color.yellow); // 오른쪽

        // 대각선 레이
        Debug.DrawRay(position, (transform.forward + transform.right).normalized, Color.magenta); // 앞 오른쪽
        Debug.DrawRay(position, (transform.forward - transform.right).normalized, Color.cyan); // 앞 왼쪽
        Debug.DrawRay(position, (-transform.forward + transform.right).normalized, Color.white); // 뒤 오른쪽
        Debug.DrawRay(position, (-transform.forward - transform.right).normalized, Color.gray); // 뒤 왼쪽
    }
}*//*
using UnityEngine;

public class MoveAndStop : MonoBehaviour
{
    public Vector3 Senser1;
    public Vector3 Senser2;
    public Vector3 Senser3;
    public Vector3 Senser4;
    public float moveSpeed = 5f; // 이동 속도
    private bool isMoving = true; // 이동 여부
    private bool isColliding = false; // 충돌 상태
    private float waitTime = 1f; // 대기 시간
    private float timer = 0f; // 타이머

    void Update()
    {
        Vector3 position = transform.position;

        // DrawRay를 이용해서 선에서 센서 작용
        Debug.DrawRay(position, transform.forward, Color.blue);     // 앞
        Debug.DrawRay(position, -transform.forward, Color.blue);    // 뒤
        Debug.DrawRay(position, transform.right, Color.blue);       // 오른쪽
        Debug.DrawRay(position, -transform.right, Color.blue);      // 왼쪽

        // 오브젝트 이용 대각선
        Debug.DrawRay(position, Senser1, Color.blue);
        Debug.DrawRay(position, Senser2, Color.blue);
        Debug.DrawRay(position, Senser3, Color.blue);
        Debug.DrawRay(position, Senser4, Color.blue);

        // 각 방향에 대해 레이캐스트 충돌 체크
        CheckCollision(transform.right);
        CheckCollision(transform.forward);
        CheckCollision(-transform.forward);
        CheckCollision(-transform.right);
    }

    private void CheckCollision(Vector3 direction)
    {
        RaycastHit hitinfo; // 지역 변수로 선언

        // 레이캐스트를 통해 충돌 체크
        if (Physics.Raycast(transform.position, direction, out hitinfo))
        {
            // 충돌이 있을 경우
            if (hitinfo.collider.CompareTag("target"))
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
    }
}*/
/*using UnityEngine;
using System.Collections;

public class asd : MonoBehaviour
{
    RaycastHit hitinfo;
    float detectionDistance = 5f; // 감지 거리 설정
    private float moveSpeed = 5f; // 이동 속도
    public bool isMoving = true; // 이동 여부
    public bool isColliding = false; // 충돌 상태
    public float waitTime = 1f; // 대기 시간
    public float timer = 0f; // 타이머

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
        // Debug.DrawRay(position, (transform.forward + transform.right).normalized, Color.blue);     
        // Debug.DrawRay(position, (transform.forward - transform.right).normalized, Color.blue);    
        // Debug.DrawRay(position, (-transform.forward + transform.right).normalized, Color.blue);    
        // Debug.DrawRay(position, (-transform.forward - transform.right).normalized, Color.blue);      

        // 각 방향에 대해 레이캐스트 충돌 체크
        CheckCollision(transform.right);
        CheckCollision(transform.forward);
        CheckCollision(-transform.forward);
        CheckCollision(-transform.right);


        *//*// RaycastHit hitinfo;
        Debug.Log(point2);
        Debug.DrawRay(transform.position, point2, Color.blue);
        *//*

        // 물체 이동
        if (isMoving)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }

        // 각 센서에서 충돌 체크
        CheckDiagonalSensors();

    }

    private void CheckCollision(Vector3 direction)
    {
        RaycastHit hitinfo; // 지역 변수로 선언

        // 레이캐스트를 통해 충돌 체크
        if (Physics.Raycast(transform.position, direction, out hitinfo, detectionDistance))
        {
            // 충돌이 있을 경우
            if (hitinfo.collider.CompareTag("target"))
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
    }
    private void CheckDiagonalSensors()
    {
        // 센서 오브젝트 찾기
        GameObject senser1 = GameObject.Find("Senser1");
        GameObject senser2 = GameObject.Find("Senser2");
        GameObject senser3 = GameObject.Find("Senser3");
        GameObject senser4 = GameObject.Find("Senser4");

        // 센서에서 충돌 체크
        if (senser1 != null)
            CheckCollision2((senser1.transform.position - transform.position).normalized);
        if (senser2 != null)
            CheckCollision2((senser2.transform.position - transform.position).normalized);
        if (senser3 != null)
            CheckCollision2((senser3.transform.position - transform.position).normalized);
        if (senser4 != null)
            CheckCollision2((senser4.transform.position - transform.position).normalized);
    }

    private void CheckCollision2(Vector3 sensorPosition)
    {
        RaycastHit hitinfo; // 지역 변수로 선언

        // 레이캐스트를 통해 충돌 체크
        if (Physics.Raycast(sensorPosition, transform.forward, out hitinfo))
        {
            // 충돌이 있을 경우
            if (hitinfo.collider.CompareTag("target"))
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

*/
using System;
using System.Collections;
using UnityEngine;

public class CloneObject : MonoBehaviour
{
    public float fixedYPosition; // 고정할 Y 위치
    public float rotationSpeed = 50f; // 회전 속도
    public float scaleIncreaseAmount = 0.1f; // 한 바퀴 돌 때 증가할 스케일 양
    private float currentRotation = 0f; // 현재 회전 각도
    private bool isRotating = true; // 회전 여부
    private Vector3 initialScale; // 초기 스케일
    public GameObject objectToClone; // 복제할 오브젝트
    public float cloneInterval = 1.0f; // 복제 간격
    private float timer;

    void Start()
    {
        initialScale = transform.localScale; // 초기 스케일 저장

    }

    void Update()
    {
        // Y축 위치를 고정
        transform.position = new Vector3(transform.position.x, fixedYPosition, transform.position.z);

        // 회전 중일 때만 회전
        if (isRotating)
        {
            // 시계방향으로 회전 (Y축 기준)
            float rotationThisFrame = rotationSpeed * Time.deltaTime;
            transform.Rotate(0, rotationThisFrame, 0);

            // 현재 회전 각도 업데이트
            currentRotation += rotationThisFrame;

            // 한 바퀴 (360도) 돌았을 때 스케일 증가
            if (currentRotation >= 360f)
            {
                currentRotation -= 360f; // 각도를 0으로 리셋
                transform.localScale += new Vector3(scaleIncreaseAmount, 0, scaleIncreaseAmount); // 스케일 증가
            }

            // 크기가 커지면 멈춤후 없어짐
            if (transform.localScale.x >= initialScale.x * 2f)
            {
                isRotating = false; // 회전 중지
                StartCoroutine(delayTime(2.0f)); // 지연 시간 2초
            }
        }
        timer += Time.deltaTime;

        if (timer >= cloneInterval)
        {
            Clone();
            timer = 0; // 타이머 초기화
        }
    }

    void Clone()
    {
        // 새로운 오브젝트 생성
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        Quaternion rotation = Quaternion.Euler(90, 0, 0); // x축으로 90도 회전
        Instantiate(objectToClone, newPosition, rotation);
    }
    public IEnumerator delayTime(float waitTime) // 
    {
        if (!isRotating)
        {
            yield return new WaitForSeconds(waitTime);  // 2초 기다림
            Destroy(gameObject);
        }

    }
}