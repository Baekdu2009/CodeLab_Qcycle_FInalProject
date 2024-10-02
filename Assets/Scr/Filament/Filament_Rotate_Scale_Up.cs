using System;
using System.Collections;
using UnityEngine;




public class Filament_Rotate_Scale_Up : MonoBehaviour
{
    public float fixedYPosition; // ������ Y ��ġ
    private float currentRotation = 0f; // ���� ȸ�� ����


    public float rotationSpeed = 50f; // ȸ�� �ӵ�
    public float scaleIncreaseAmount = 0.1f; // �� ���� �� �� ������ ������ ��
    private bool isRotating = true; // ȸ�� ����
    private Vector3 initialScale; // �ʱ� ������




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
    }


    // 2�� �ڿ� ����
    public IEnumerator delayTime(float waitTime) // 
    {
        if (!isRotating)   
        {
            yield return new WaitForSeconds(waitTime);  // 2�� ��ٸ�
            Destroy(gameObject);
        }

    }

   
}


// 7.5112 1.9223 2.5838

