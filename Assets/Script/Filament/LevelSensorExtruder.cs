using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSensorExtruder : MonoBehaviour
{
    private int collisionCount = 0; // �浹 ��
    public bool isSensing = false; // ���� ����
    [SerializeField] private GameObject prefab; // ������ ������
    [SerializeField] private List<Transform> spawnPositions; // ���� ���� ��ġ�� �����ϴ� ����Ʈ
    private Coroutine spawningCoroutine; // ������ ���� �ڷ�ƾ

    private void Update()
    {
        // Plastic �±׸� ���� ������Ʈ�� ���� ���� ���� ������ ����
        if (!isSensing && spawningCoroutine == null && GameObject.FindGameObjectWithTag("Plastic") != null)
        {
            spawningCoroutine = StartCoroutine(SpawnInitialPrefabs());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� Plastic1 �±����� Ȯ��
        if (other.CompareTag("Plastic1"))
        {
            // �浹 Ƚ�� ����
            collisionCount++;

            // �浹 Ƚ���� 30 �̻��̸� isSensing�� true�� ����
            if (collisionCount >= 30 && !isSensing)
            {
                StartSensing();
            }

            // isSensing�� false�� ���� ������Ʈ ����
            if (!isSensing)
            {
                StartCoroutine(RemoveAfterDelay(other.gameObject, 2f));
            }
        }
    }

    private void StartSensing()
    {
        isSensing = true; // ���� ���� Ȱ��ȭ

        // ��� Plastic1 ����
        StartCoroutine(RemoveAllPlastic1());
    }

    private IEnumerator RemoveAllPlastic1()
    {
        // ��� Plastic1 ������Ʈ ����
        GameObject[] allPlastics = GameObject.FindGameObjectsWithTag("Plastic1");

        foreach (GameObject plastic in allPlastics)
        {
            if (plastic != null) // plastic�� �����ϴ��� Ȯ��
            {
                Destroy(plastic); // ������Ʈ ����
                yield return new WaitForSeconds(0.1f); // ������ ��� �ð�
            }
        }

        // ��� Plastic1�� ���ŵ� �� �߰����� ��� �ð�
        yield return new WaitForSeconds(2f); // �� �κп��� ���� �ð��� �ø�

        // �ʱ�ȭ
        ResetSensing();
    }

    private IEnumerator RemoveAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay); // ���� �ð� ���

        // ������Ʈ�� ���� �����ϴ��� Ȯ�� �� ����
        if (obj != null)
        {
            Destroy(obj); // ������Ʈ ����
        }
    }

    // isSensing�� �ʱ�ȭ�ϴ� �޼��� �߰�
    public void ResetSensing()
    {
        isSensing = false; // ���� ���� �ʱ�ȭ
        collisionCount = 0; // �浹 �� �ʱ�ȭ

        // �ʱ�ȭ �� ������ ����
        spawningCoroutine = null; // spawningCoroutine�� null�� �����Ͽ� ������ ���� �����ϰ� ��
    }

    private IEnumerator SpawnInitialPrefabs()
    {
        while (!isSensing) // isSensing�� false�� ���ȸ� �ݺ�
        {
            foreach (var spawnPosition in spawnPositions)
            {
                Instantiate(prefab, spawnPosition.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(1f); // 1�� ��� �� �ٽ� ����
        }

        spawningCoroutine = null; // �ڷ�ƾ ���� �� null�� ����
    }
}
