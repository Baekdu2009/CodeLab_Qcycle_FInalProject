using System;
using UnityEngine;

public class BoxSensor : MonoBehaviour
{
    [Header("Boxing")]
    [Tooltip("박스 한면을 접는 용도")]
    [SerializeField] GameObject boxHander; // BoxHander 오브젝트
    [SerializeField] GameObject UpRight; // UpRight 오브젝트
    [SerializeField] GameObject UpLeft; // UpLeft 오브젝트
    [SerializeField] GameObject UpFront; // UpFront 오브젝트
    [SerializeField] GameObject UpBack; // UpBack 오브젝트
    [Space(10f)]
    [SerializeField] GameObject BoxTape;


    private bool hasRotated = false; // 회전 여부를 추적하는 변수
    private Quaternion initialRotation; // 초기 회전 값 저장
    private bool hasTaping = false; // 테이핑 여부를 추적하는 변수
    private void OnTriggerEnter(Collider other)
    {
        if (!hasRotated && other.CompareTag("Box"))
        {
            hasRotated = true; // 회전이 수행되었음을 표시

            // boxHander를 x축으로 -90도 회전
            boxHander.transform.Rotate(-90, 0, 0);

            // UpRight를 y축으로 90도 회전
            UpRight.transform.Rotate(0, 90, 0);
            UpFront.transform.Rotate(0, -90, 0);
            UpBack.transform.Rotate(0, 90, 0);

            // UpLeft 회전 처리
            RotateUpLeftIfColliding();
        }
    }

    private void RotateUpLeftIfColliding()
    {
        // boxHander와 UpLeft의 충돌 감지
        Collider[] colliders = Physics.OverlapBox(boxHander.transform.position, boxHander.transform.localScale / 2, boxHander.transform.rotation);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject == UpLeft)
            {
                // UpLeft가 boxHander와 충돌했을 경우 x축으로 90도 회전
                UpLeft.transform.Rotate(0, 90, 0);
                break;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            hasRotated = false;
            // boxHander.transform.Rotate(45, 0, 0);
            boxHander.transform.rotation = initialRotation; // boxHander 초기 값 불러오기
        }
    }
}

