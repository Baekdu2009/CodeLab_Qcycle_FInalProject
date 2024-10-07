using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSensor : MonoBehaviour
{
    [SerializeField] private int collisionCount = 0; // �浹 ��
    public bool isDetected = false; // ���� ����

    private HashSet<Collider> collidedPlastics = new HashSet<Collider>(); // �ߺ� �浹 ������ ���� ����Ʈ

    private void Update()
    {
        // ���� Ƚ���� 150ȸ �̻��̸� isDetected�� true�� ����
        if (collisionCount >= 150 && !isDetected)
        {
            isDetected = true;
            Debug.Log("isDetected Ȱ��ȭ!");
            StartCoroutine(RemovePlastics(0.4f, 4)); // 30%�� 4�� ����
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� Plastic �±����� Ȯ��
        if (other.CompareTag("Plastic"))
        {
            // �ߺ� �浹 ����
            if (!collidedPlastics.Contains(other))
            {
                collidedPlastics.Add(other); // ���� �浹 ���� Plastic�� �߰�
                collisionCount++;
                Debug.Log("Plastic�� �浹 ����: " + collisionCount);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �浹�� ������ �ش� Collider ����
        if (other.CompareTag("Plastic"))
        {
            collidedPlastics.Remove(other); // �ߺ� �浹 ����Ʈ���� ����
            Debug.Log("Plastic���� �浹 ����: " + other.gameObject.name);
        }
    }

    private IEnumerator RemovePlastics(float percentage, int times)
    {
        for (int i = 0; i < times; i++)
        {
            // ���� ��� Plastic ������Ʈ�� ������
            GameObject[] allPlastics = GameObject.FindGameObjectsWithTag("Plastic");
            int totalPlastics = allPlastics.Length;
            int countToRemove = Mathf.CeilToInt(totalPlastics * percentage);

            for (int j = 0; j < countToRemove && allPlastics.Length > 0; j++)
            {
                int randomIndex = Random.Range(0, allPlastics.Length);
                GameObject plasticToRemove = allPlastics[randomIndex];

                if (plasticToRemove != null)
                {
                    Destroy(plasticToRemove);
                    Debug.Log("������ �ö�ƽ ���ŵ�: " + plasticToRemove.name);
                    allPlastics = GameObject.FindGameObjectsWithTag("Plastic"); // ���ŵ� Plastic �迭
                }
            }

            yield return new WaitForSeconds(3f); // �� �ܰ� �� 1�� ���
        }

        ResetDetection(); // ��� �ö�ƽ ���� �� �ʱ�ȭ
    }

    private void ResetDetection()
    {
        isDetected = false; // ���� ���� �ʱ�ȭ
        collisionCount = 0; // �浹 �� �ʱ�ȭ
        collidedPlastics.Clear(); // �ߺ� �浹 ����Ʈ �ʱ�ȭ
        Debug.Log("���� ���¿� �浹 �� �ʱ�ȭ �Ϸ�!");
    }
}
