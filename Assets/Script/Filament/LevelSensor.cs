using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSensor : MonoBehaviour
{
    [SerializeField] private int collisionCount = 0; // �浹 ��
    public bool isDetected = false; // ���� ����

    private HashSet<Collider> collidedPlastics = new HashSet<Collider>(); // �ߺ� �浹 ������ ���� ����Ʈ
    private LevelSensorExtruder extruder; // LevelSensorExtruder�� �ν��Ͻ�
    private Coroutine reduceCoroutine; // �ö�ƽ ���� �ڷ�ƾ
    private int sensingChangeCount = 0; // isSensing ���� ��ȭ ī��Ʈ
    private bool lastIsSensingState = false; // ������ isSensing ���� ����

    private void Start()
    {
        // ������ LevelSensorExtruder ã��
        extruder = FindObjectOfType<LevelSensorExtruder>();
        if (extruder == null)
        {
            Debug.LogError("LevelSensorExtruder�� ã�� �� �����ϴ�.");
        }
    }

    private void Update()
    {
        // ���� Ƚ���� 150ȸ �̻��̸� isDetected�� true�� ����
        if (collisionCount >= 150 && !isDetected)
        {
            isDetected = true;
            Debug.Log("isDetected Ȱ��ȭ!");
           
        }

        // ����: !isSensing && !isDetected
        if (!extruder.isSensing && !isDetected)
        {
            if (reduceCoroutine == null)
            {
                reduceCoroutine = StartCoroutine(ReducePlasticCount(0.2f)); // 20% ����
            }
        }

        // ���� ��ȭ ����
        if (extruder.isSensing)
        {
            if (!lastIsSensingState) // ������ ���°� false���� ��
            {
                lastIsSensingState = true; // ���� ���¸� true�� ����
            }
        }
        else
        {
            if (lastIsSensingState) // ������ ���°� true���� ��
            {
                sensingChangeCount++;
                lastIsSensingState = false; // ���� ���¸� false�� ����
                Debug.Log("isSensing ���� ��ȭ: " + sensingChangeCount);
            }
        }

        // ���°� false���� true�� �ٽ� �ٲ� �� ī��Ʈ ����
        if (sensingChangeCount >= 4)
        {
            ResetDetection(); // 4�� ��ȭ �� �ʱ�ȭ
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� Plastic �±����� Ȯ��
        if (other.CompareTag("Plastic"))
        {
            // isDetected�� Ȱ��ȭ�Ǿ� ���� ���� �浹 ���� ������Ű�� ����
            if (!isDetected)
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

    private IEnumerator ReducePlasticCount(float percentage)
    {
        while (!extruder.isSensing) // isSensing�� ��Ȱ��ȭ�� ���ȸ� �ݺ�
        {
            GameObject[] allPlastics = GameObject.FindGameObjectsWithTag("Plastic");
            int totalPlastics = allPlastics.Length;

            if (totalPlastics > 0)
            {
                // �����ִ� �ö�ƽ�� ���� ���� ����
                int countToRemove = Mathf.CeilToInt(totalPlastics * percentage);

                for (int i = 0; i < countToRemove && totalPlastics > 0; i++)
                {
                    int randomIndex = Random.Range(0, totalPlastics);
                    GameObject plasticToRemove = allPlastics[randomIndex];

                    if (plasticToRemove != null)
                    {
                        Destroy(plasticToRemove);
                        Debug.Log("������ �ö�ƽ ���ŵ�: " + plasticToRemove.name);
                        allPlastics = GameObject.FindGameObjectsWithTag("Plastic"); // ����Ʈ ������Ʈ
                        totalPlastics = allPlastics.Length; // ������Ʈ�� ���� �ٽ� ���
                    }
                    yield return new WaitForSeconds(0.1f); // ������ ��� �ð�
                }

                yield return new WaitForSeconds(3f); // ���� �ݺ����� 3�� ���
            }
            else
            {
                break; // �� �̻� ������ �ö�ƽ�� ������ ����
            }
        }

        reduceCoroutine = null; // �ڷ�ƾ ���� �� null�� ����
    }

    private void ResetDetection()
    {
        isDetected = false; // �ʱ�ȭ
        collisionCount = 0; // �浹 �� �ʱ�ȭ
        sensingChangeCount = 0; // ���� ��ȭ ī��Ʈ �ʱ�ȭ
        collidedPlastics.Clear(); // �ߺ� �浹 ����Ʈ �ʱ�ȭ

        Debug.Log("�浹 ���� �ʱ�ȭ��.");
    }
}