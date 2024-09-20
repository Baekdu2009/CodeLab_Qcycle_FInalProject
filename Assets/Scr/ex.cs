/*using UnityEngine;
using System.Collections;

public class MoveAlongLine : MonoBehaviour
{
    Vector3 point1 = new Vector3(0, 0, 0);     // ����
    Vector3 point2 = Vector3.right * 100;      // X�� �������� 100������ŭ �̵��� ���� ��Ÿ��
    RaycastHit hitinfo;                        // �ʱ�ȭ�� �Ǿ����� ����, �浹 ������ ����

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        // RaycastHit hitinfo;
        Debug.Log(point2);
        Debug.DrawRay(transform.position, point2, Color.red);           // transform.position�� ���� ��ũ��Ʈ�� ������ ������Ʈ�� ��ġ�� ��Ÿ��

        if (Physics.Raycast(transform.position, point2, out hitinfo))
        {
            Debug.Log("�浹��ü ����");
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
        // �⺻ ��ġ
        Vector3 position = transform.position;

        // ��, ��, ����, ������ ����
        Debug.DrawRay(position, transform.forward, Color.red); // ��
        Debug.DrawRay(position, -transform.forward, Color.blue); // ��
        Debug.DrawRay(position, -transform.right, Color.green); // ����
        Debug.DrawRay(position, transform.right, Color.yellow); // ������

        // �밢�� ����
        Debug.DrawRay(position, (transform.forward + transform.right).normalized, Color.magenta); // �� ������
        Debug.DrawRay(position, (transform.forward - transform.right).normalized, Color.cyan); // �� ����
        Debug.DrawRay(position, (-transform.forward + transform.right).normalized, Color.white); // �� ������
        Debug.DrawRay(position, (-transform.forward - transform.right).normalized, Color.gray); // �� ����
    }
}*//*
using UnityEngine;

public class MoveAndStop : MonoBehaviour
{
    public Vector3 Senser1;
    public Vector3 Senser2;
    public Vector3 Senser3;
    public Vector3 Senser4;
    public float moveSpeed = 5f; // �̵� �ӵ�
    private bool isMoving = true; // �̵� ����
    private bool isColliding = false; // �浹 ����
    private float waitTime = 1f; // ��� �ð�
    private float timer = 0f; // Ÿ�̸�

    void Update()
    {
        Vector3 position = transform.position;

        // DrawRay�� �̿��ؼ� ������ ���� �ۿ�
        Debug.DrawRay(position, transform.forward, Color.blue);     // ��
        Debug.DrawRay(position, -transform.forward, Color.blue);    // ��
        Debug.DrawRay(position, transform.right, Color.blue);       // ������
        Debug.DrawRay(position, -transform.right, Color.blue);      // ����

        // ������Ʈ �̿� �밢��
        Debug.DrawRay(position, Senser1, Color.blue);
        Debug.DrawRay(position, Senser2, Color.blue);
        Debug.DrawRay(position, Senser3, Color.blue);
        Debug.DrawRay(position, Senser4, Color.blue);

        // �� ���⿡ ���� ����ĳ��Ʈ �浹 üũ
        CheckCollision(transform.right);
        CheckCollision(transform.forward);
        CheckCollision(-transform.forward);
        CheckCollision(-transform.right);
    }

    private void CheckCollision(Vector3 direction)
    {
        RaycastHit hitinfo; // ���� ������ ����

        // ����ĳ��Ʈ�� ���� �浹 üũ
        if (Physics.Raycast(transform.position, direction, out hitinfo))
        {
            // �浹�� ���� ���
            if (hitinfo.collider.CompareTag("target"))
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
    }
}*/
/*using UnityEngine;
using System.Collections;

public class asd : MonoBehaviour
{
    RaycastHit hitinfo;
    float detectionDistance = 5f; // ���� �Ÿ� ����
    private float moveSpeed = 5f; // �̵� �ӵ�
    public bool isMoving = true; // �̵� ����
    public bool isColliding = false; // �浹 ����
    public float waitTime = 1f; // ��� �ð�
    public float timer = 0f; // Ÿ�̸�

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
        // Debug.DrawRay(position, (transform.forward + transform.right).normalized, Color.blue);     
        // Debug.DrawRay(position, (transform.forward - transform.right).normalized, Color.blue);    
        // Debug.DrawRay(position, (-transform.forward + transform.right).normalized, Color.blue);    
        // Debug.DrawRay(position, (-transform.forward - transform.right).normalized, Color.blue);      

        // �� ���⿡ ���� ����ĳ��Ʈ �浹 üũ
        CheckCollision(transform.right);
        CheckCollision(transform.forward);
        CheckCollision(-transform.forward);
        CheckCollision(-transform.right);


        *//*// RaycastHit hitinfo;
        Debug.Log(point2);
        Debug.DrawRay(transform.position, point2, Color.blue);
        *//*

        // ��ü �̵�
        if (isMoving)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }

        // �� �������� �浹 üũ
        CheckDiagonalSensors();

    }

    private void CheckCollision(Vector3 direction)
    {
        RaycastHit hitinfo; // ���� ������ ����

        // ����ĳ��Ʈ�� ���� �浹 üũ
        if (Physics.Raycast(transform.position, direction, out hitinfo, detectionDistance))
        {
            // �浹�� ���� ���
            if (hitinfo.collider.CompareTag("target"))
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
    }
    private void CheckDiagonalSensors()
    {
        // ���� ������Ʈ ã��
        GameObject senser1 = GameObject.Find("Senser1");
        GameObject senser2 = GameObject.Find("Senser2");
        GameObject senser3 = GameObject.Find("Senser3");
        GameObject senser4 = GameObject.Find("Senser4");

        // �������� �浹 üũ
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
        RaycastHit hitinfo; // ���� ������ ����

        // ����ĳ��Ʈ�� ���� �浹 üũ
        if (Physics.Raycast(sensorPosition, transform.forward, out hitinfo))
        {
            // �浹�� ���� ���
            if (hitinfo.collider.CompareTag("target"))
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

*/
using System;
using System.Collections;
using UnityEngine;

public class CloneObject : MonoBehaviour
{
    public float fixedYPosition; // ������ Y ��ġ
    public float rotationSpeed = 50f; // ȸ�� �ӵ�
    public float scaleIncreaseAmount = 0.1f; // �� ���� �� �� ������ ������ ��
    private float currentRotation = 0f; // ���� ȸ�� ����
    private bool isRotating = true; // ȸ�� ����
    private Vector3 initialScale; // �ʱ� ������
    public GameObject objectToClone; // ������ ������Ʈ
    public float cloneInterval = 1.0f; // ���� ����
    private float timer;

    void Start()
    {
        initialScale = transform.localScale; // �ʱ� ������ ����

    }

    void Update()
    {
        // Y�� ��ġ�� ����
        transform.position = new Vector3(transform.position.x, fixedYPosition, transform.position.z);

        // ȸ�� ���� ���� ȸ��
        if (isRotating)
        {
            // �ð�������� ȸ�� (Y�� ����)
            float rotationThisFrame = rotationSpeed * Time.deltaTime;
            transform.Rotate(0, rotationThisFrame, 0);

            // ���� ȸ�� ���� ������Ʈ
            currentRotation += rotationThisFrame;

            // �� ���� (360��) ������ �� ������ ����
            if (currentRotation >= 360f)
            {
                currentRotation -= 360f; // ������ 0���� ����
                transform.localScale += new Vector3(scaleIncreaseAmount, 0, scaleIncreaseAmount); // ������ ����
            }

            // ũ�Ⱑ Ŀ���� ������ ������
            if (transform.localScale.x >= initialScale.x * 2f)
            {
                isRotating = false; // ȸ�� ����
                StartCoroutine(delayTime(2.0f)); // ���� �ð� 2��
            }
        }
        timer += Time.deltaTime;

        if (timer >= cloneInterval)
        {
            Clone();
            timer = 0; // Ÿ�̸� �ʱ�ȭ
        }
    }

    void Clone()
    {
        // ���ο� ������Ʈ ����
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        Quaternion rotation = Quaternion.Euler(90, 0, 0); // x������ 90�� ȸ��
        Instantiate(objectToClone, newPosition, rotation);
    }
    public IEnumerator delayTime(float waitTime) // 
    {
        if (!isRotating)
        {
            yield return new WaitForSeconds(waitTime);  // 2�� ��ٸ�
            Destroy(gameObject);
        }

    }
}