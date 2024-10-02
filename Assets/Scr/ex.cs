/*using UnityEngine;

public class ex : MonoBehaviour
{
    public Transform drawLineObject;
    public Transform drawSphereObject;
    public Transform drawCubeObject;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(drawLineObject.position, drawLineObject.position + Vector3.forward * 10);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(drawSphereObject.transform.position, 2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawCube(drawCubeObject.transform.position, Vector2.one * 2f);
    }
}*//*

using System.Collections;
using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ex : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float gravity = -9.81f; // 중력 값 설정
    [SerializeField] float rotationSpeed = 720f;
    [SerializeField] float acceleration = 0.1f; // 가속도
    [SerializeField] float deceleration = 0.1f; // 감속
    private CharacterController controller;
    private Vector3 velocity;  // 속도 벡터
    private Vector3 moveDir;
    
    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleGravity();
        HandleMovementInput();
        HandleRotation();
        // 이동 처리(CharacterController의 Move 메서드 사용)
        controller.Move(moveDir * Time.deltaTime * speed);
        

        
    }

    void HandleMovementInput()
    {

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 movDir = new Vector3(h, 0, v).normalized;

        // 부드러운 가속과 감속
        if(movDir.magnitude > 0.1f)
            moveDir = Vector3.Lerp(moveDir, movDir * speed, acceleration);
        else
            moveDir = Vector3.Lerp(moveDir,Vector3.zero, deceleration);     
    }

    private void HandleRotation()
    {
        if(moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void HandleGravity()
    {
        // 중력 처리
        if (controller.isGrounded)
        {
            velocity.y = 0; // 딸에 있을 때 y 속도 초기화
        }
        else
        {
            velocity.y += gravity * Time.deltaTime; // 중력 적용
        }

        // 중력에는 speed 적용 x
        // controller.Move(velocity * Time.deltaTime);
    }

}
*/