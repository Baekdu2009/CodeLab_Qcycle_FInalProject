using System;
using UnityEngine;

public class BoxSensor : MonoBehaviour
{
    [Header("Boxing")]
    [Tooltip("박스 한면을 접는 용도")]
    [SerializeField] GameObject boxHander; // BoxHander 오브젝트

    private bool hasRotated = false; // 회전 여부를 추적하는 변수
    private Quaternion initialRotation; // 초기 회전 값 저장

    private void OnTriggerEnter(Collider other)
    {
        if (!hasRotated && other.CompareTag("Box"))
        {
            hasRotated = true; // 회전이 수행되었음을 표시

            // boxHander를 x축으로 -90도 회전
            boxHander.transform.Rotate(-90, 0, 0);
        }
    }


    private void OnTriggerExit(Collider other)
    {
            hasRotated = false;
            // boxHander.transform.Rotate(45, 0, 0);
            boxHander.transform.rotation = initialRotation; // boxHander 초기 값 불러오기
        
    }

}

