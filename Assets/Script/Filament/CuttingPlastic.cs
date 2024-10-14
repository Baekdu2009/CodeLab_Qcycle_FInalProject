using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingPlastic : MonoBehaviour
{
    [SerializeField] GameObject cuttingPrefab;
    [SerializeField] List<Transform> spawnPositions;

    private void OnCollisionEnter(Collision collision)
    {
        // �ڽŰ� ���� �±׸� ���� ��츸 �۵�
        if (collision.gameObject.CompareTag("Material"))
        {
            StartCoroutine(shatterAfterDelay(collision.gameObject));
        }
    }

    IEnumerator shatterAfterDelay(GameObject gameObj)
    {
        yield return new WaitForSeconds(0.5f);
        shatter(gameObj);
    }

    private void shatter(GameObject gameObj)
    {
        // ��� ��ġ�� ���� prefab ����
        foreach (Transform spawnPosition in spawnPositions)
        {
            Instantiate(cuttingPrefab, spawnPosition.position, transform.rotation);
        }

        Destroy(gameObj);
    }
}