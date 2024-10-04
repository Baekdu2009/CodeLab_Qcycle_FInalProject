using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingPlastic : MonoBehaviour
{
    [SerializeField] GameObject cuttingPrefab;
    [SerializeField] List<Transform> spawnPositions; // 여러 개의 위치를 저장하는 리스트

    private void OnCollisionEnter(Collision collision)
    {
        print("작동");
        if (collision.gameObject.CompareTag("Metal"))
        {
            StartCoroutine(shatterAfterDelay(collision.gameObject));
        }
    }

    IEnumerator shatterAfterDelay(GameObject gameObj)
    {
        yield return new WaitForSeconds(1);
        shatter(gameObj);
    }

    private void shatter(GameObject gameObj)
    {
        // 모든 위치에 대해 prefab 생성
        foreach (Transform spawnPosition in spawnPositions)
        {
            Instantiate(cuttingPrefab, spawnPosition.position, transform.rotation);
        }

        Destroy(gameObj);
    }
}