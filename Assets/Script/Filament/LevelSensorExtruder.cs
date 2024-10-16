using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSensorExtruder : MonoBehaviour
{
    private int collisionCount = 0; // 충돌 수
    public bool isSensing = false; // 감지 상태
    [SerializeField] private GameObject prefab; // 생성할 프리팹
    [SerializeField] private List<Transform> spawnPositions; // 여러 개의 위치를 저장하는 리스트
    [SerializeField] public string plasticTag = "Plastic1"; // 설정할 태그
    [SerializeField] public string plasticTagMe = "Plastic2";
    private Coroutine spawningCoroutine; // 프리팹 생성 코루틴

    private void Update()
    {
        // Plastic 태그를 가진 오브젝트가 씬에 있을 때만 프리팹 생성
        if (!isSensing && spawningCoroutine == null && GameObject.FindGameObjectWithTag(plasticTag) != null)
        {
            spawningCoroutine = StartCoroutine(SpawnInitialPrefabs());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 설정한 태그인지 확인
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
        isSensing = true; // 감지 상태 활성화
        StartCoroutine(RemoveAllPlastic());
    }

    private IEnumerator RemoveAllPlastic()
    {
        // 모든 지정된 플라스틱 제거
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