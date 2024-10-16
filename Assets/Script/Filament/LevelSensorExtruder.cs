using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSensorExtruder : MonoBehaviour
{
    private int collisionCount = 0; // �浹 ��
    public bool isSensing = false; // ���� ����
    [SerializeField] private GameObject prefab; // ������ ������
    [SerializeField] private List<Transform> spawnPositions; // ���� ���� ��ġ�� �����ϴ� ����Ʈ
    [SerializeField] public string plasticTag = "Plastic1"; // ������ �±�
    [SerializeField] public string plasticTagMe = "Plastic2";
    private Coroutine spawningCoroutine; // ������ ���� �ڷ�ƾ

    private void Update()
    {
        // Plastic �±׸� ���� ������Ʈ�� ���� ���� ���� ������ ����
        if (!isSensing && spawningCoroutine == null && GameObject.FindGameObjectWithTag(plasticTag) != null)
        {
            spawningCoroutine = StartCoroutine(SpawnInitialPrefabs());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� ������ �±����� Ȯ��
        if (other.CompareTag(plasticTagMe))
        {
            collisionCount++;

            if (collisionCount >= 100 && !isSensing)
            {
                StartSensing();
            }

            if (!isSensing)
            {
                StartCoroutine(RemoveAfterDelay(other.gameObject, 2f));
            }
        }
    }

    private void StartSensing()
    {
        isSensing = true; // ���� ���� Ȱ��ȭ
        StartCoroutine(RemoveAllPlastic());
    }

    private IEnumerator RemoveAllPlastic()
    {
        // ��� ������ �ö�ƽ ����
        GameObject[] allPlastics = GameObject.FindGameObjectsWithTag(plasticTagMe);

        foreach (GameObject plastic in allPlastics)
        {
            if (plastic != null)
            {
                Destroy(plastic);
                yield return new WaitForSeconds(0.1f);
            }
        }

        yield return new WaitForSeconds(2f);
        ResetSensing();
    }

    private IEnumerator RemoveAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (obj != null)
        {
            Destroy(obj);
        }
    }

    public void ResetSensing()
    {
        isSensing = false;
        collisionCount = 0;
        spawningCoroutine = null;
    }

    private IEnumerator SpawnInitialPrefabs()
    {
        while (!isSensing)
        {
            foreach (var spawnPosition in spawnPositions)
            {
                Instantiate(prefab, spawnPosition.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(1f);
        }

        spawningCoroutine = null;
    }
}