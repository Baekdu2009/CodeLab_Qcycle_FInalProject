using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CuttingPlastic : MonoBehaviour
{
    [SerializeField] GameObject cuttingPrefab;
    [SerializeField] List<Transform> spawnPositions; // ���� ���� ��ġ�� �����ϴ� ����Ʈ


    private void OnCollisionEnter(Collision collision)
    {
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
