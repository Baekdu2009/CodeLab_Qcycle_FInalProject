using UnityEngine;
using System.Collections;

public class Collision : MonoBehaviour
{
    float detectionDistance = 5f; // ���� �Ÿ� ����

    public float moveSpeed = 5f; // �̵� �ӵ�
    private bool isMoving = true; // �̵� ����
    private bool isColliding = false; // �浹 ����
    private float waitTime = 5f; // ��� �ð�
    private float timer = 0f; // Ÿ�̸�

    void Start()
    {

    }

    void Update()
    {
        Vector3 position = transform.position;


        // DrawRay�� �̿��ؼ� �������� ���� �ۿ�
        // ��, ��, ����, ������
        Debug.DrawRay(position, transform.forward, Color.blue);     // ��
        Debug.DrawRay(position, -transform.forward, Color.blue);    // ��
        Debug.DrawRay(position, transform.right, Color.blue);       // ������
        Debug.DrawRay(position, -transform.right, Color.blue);      // ����

        // DrawRay �̿� �밢��
        Debug.DrawRay(position, (transform.forward + transform.right).normalized, Color.blue);
        Debug.DrawRay(position, (transform.forward - transform.right).normalized, Color.blue);
        Debug.DrawRay(position, (-transform.forward + transform.right).normalized, Color.blue);
        Debug.DrawRay(position, (-transform.forward - transform.right).normalized, Color.blue);

        // �� ���⿡ ���� ����ĳ��Ʈ �浹 üũ
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
        // ����ĳ��Ʈ�� ���� �浹 üũ
        if (Physics.Raycast(transform.position, direction, out RaycastHit hitinfo, detectionDistance))
        {
            // �浹�� ���� ���
            if (hitinfo.collider.tag == "target")
            {
                Debug.Log("�浹��ü ����");
                isMoving = false; // ���� ���·� ����
                isColliding = true; // �浹 ���·� ����
                timer = 0f; // Ÿ�̸� �ʱ�ȭ
            }
        }
        else
        {
            // �浹�� ���� ���
            if (isColliding)
            {
                timer += Time.deltaTime; // Ÿ�̸� ����
                if (timer >= waitTime)
                {
                    isMoving = true; // �̵� ���� ���·� ����
                    isColliding = false; // �浹 ���� ����
                }
            }
        }

            // ��ü �̵�
            if (isMoving)
            {
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
       
    }
    private void CheckCollision2(Vector3 direction)
    {
        // ����ĳ��Ʈ�� ���� �浹 üũ
        if (Physics.Raycast(transform.position, direction, out RaycastHit hitinfo, detectionDistance))
        {
            // �浹�� ���� ���
            if (hitinfo.collider.tag == "target")
            {
                Debug.Log("�밢�� �浹��ü ����");
                isMoving = false; // ���� ���·� ����
                isColliding = true; // �浹 ���·� ����
                timer = 0f; // Ÿ�̸� �ʱ�ȭ
            }
        }
        else
        {
            // �浹�� ���� ���
            if (isColliding)
            {
                timer += Time.deltaTime; // Ÿ�̸� ����
                if (timer >= waitTime)
                {
                    isMoving = true; // �̵� ���� ���·� ����
                    isColliding = false; // �浹 ���� ����
                }
            }
        }

    }
    }






