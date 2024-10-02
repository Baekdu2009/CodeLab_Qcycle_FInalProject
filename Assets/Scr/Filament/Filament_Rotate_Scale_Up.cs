using System;
using System.Collections;
using UnityEngine;




public class Filament_Rotate_Scale_Up : MonoBehaviour
{
    public float fixedYPosition; // 고정할 Y 위치
    private float currentRotation = 0f; // 현재 회전 각도


    public float rotationSpeed = 50f; // 회전 속도
    public float scaleIncreaseAmount = 0.1f; // 한 바퀴 돌 때 증가할 스케일 양
    private bool isRotating = true; // 회전 여부
    private Vector3 initialScale; // 초기 스케일




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
    }


    // 2초 뒤에 삭제
    public IEnumerator delayTime(float waitTime) // 
    {
        if (!isRotating)   
        {
            yield return new WaitForSeconds(waitTime);  // 2초 기다림
            Destroy(gameObject);
        }

    }

   
}


// 7.5112 1.9223 2.5838

