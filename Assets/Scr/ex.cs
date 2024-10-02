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
    [SerializeField] float gravity = -9.81f; // �߷� �� ����
    [SerializeField] float rotationSpeed = 720f;
    [SerializeField] float acceleration = 0.1f; // ���ӵ�
    [SerializeField] float deceleration = 0.1f; // ����
    private CharacterController controller;
    private Vector3 velocity;  // �ӵ� ����
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
        // �̵� ó��(CharacterController�� Move �޼��� ���)
        controller.Move(moveDir * Time.deltaTime * speed);
        

        
    }

    void HandleMovementInput()
    {

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 movDir = new Vector3(h, 0, v).normalized;

        // �ε巯�� ���Ӱ� ����
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
        // �߷� ó��
        if (controller.isGrounded)
        {
            velocity.y = 0; // ���� ���� �� y �ӵ� �ʱ�ȭ
        }
        else
        {
            velocity.y += gravity * Time.deltaTime; // �߷� ����
        }

        // �߷¿��� speed ���� x
        // controller.Move(velocity * Time.deltaTime);
    }

}
*/