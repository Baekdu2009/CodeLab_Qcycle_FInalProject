using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingPlastic : MonoBehaviour
{
    [SerializeField] GameObject cuttingPrefab;
    [SerializeField] List<Transform> spawnPositions;

    private void OnCollisionEnter(Collision collision)
    {
        // 자신과 같은 태그를 가진 경우만 작동
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
        // 모든 위치에 대해 prefab 생성
        foreach (Transform spawnPosition in spawnPositions)
        {
            Instantiate(cuttingPrefab, spawnPosition.position, transform.rotation);
        }

        Destroy(gameObj);
    }
}