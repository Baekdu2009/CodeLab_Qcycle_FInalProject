using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSensorExtruder : MonoBehaviour
{
    private int collisionCount = 0; // 충돌 수
    public bool isSensing = false; // 감지 상태
    [SerializeField] private GameObject prefab; // 생성할 프리팹
    [SerializeField] private List<Transform> spawnPositions; // 여러 개의 위치를 저장하는 리스트
    private Coroutine spawningCoroutine; // 프리팹 생성 코루틴

    private void Update()
    {
        // Plastic 태그를 가진 오브젝트가 씬에 있을 때만 프리팹 생성
        if (!isSensing && spawningCoroutine == null && GameObject.FindGameObjectWithTag("Plastic") != null)
        {
            spawningCoroutine = StartCoroutine(SpawnInitialPrefabs());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 Plastic1 태그인지 확인
        if (other.CompareTag("Plastic1"))
        {
            // 충돌 횟수 증가
            collisionCount++;

            // 충돌 횟수가 30 이상이면 isSensing을 true로 설정
            if (collisionCount >= 30 && !isSensing)
            {
                StartSensing();
            }

            // isSensing이 false일 때만 오브젝트 제거
            if (!isSensing)
            {
                StartCoroutine(RemoveAfterDelay(other.gameObject, 2f));
            }
        }
    }

    private void StartSensing()
    {
        isSensing = true; // 감지 상태 활성화

        // 모든 Plastic1 제거
        StartCoroutine(RemoveAllPlastic1());
    }

    private IEnumerator RemoveAllPlastic1()
    {
        // 모든 Plastic1 오브젝트 제거
        GameObject[] allPlastics = GameObject.FindGameObjectsWithTag("Plastic1");

        foreach (GameObject plastic in allPlastics)
        {
            if (plastic != null) // plastic이 존재하는지 확인
            {
                Destroy(plastic); // 오브젝트 제거
                yield return new WaitForSeconds(0.1f); // 조금의 대기 시간
            }
        }

        // 모든 Plastic1이 제거된 후 추가적인 대기 시간
        yield return new WaitForSeconds(2f); // 이 부분에서 지속 시간을 늘림

        // 초기화
        ResetSensing();
    }

    private IEnumerator RemoveAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay); // 지연 시간 대기

        // 오브젝트가 아직 존재하는지 확인 후 제거
        if (obj != null)
        {
            Destroy(obj); // 오브젝트 제거
        }
    }

    // isSensing을 초기화하는 메서드 추가
    public void ResetSensing()
    {
        isSensing = false; // 감지 상태 초기화
        collisionCount = 0; // 충돌 수 초기화

        // 초기화 후 프리팹 생성
        spawningCoroutine = null; // spawningCoroutine을 null로 설정하여 프리팹 생성 가능하게 함
    }

    private IEnumerator SpawnInitialPrefabs()
    {
        while (!isSensing) // isSensing이 false인 동안만 반복
        {
            foreach (var spawnPosition in spawnPositions)
            {
                Instantiate(prefab, spawnPosition.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(1f); // 1초 대기 후 다시 생성
        }

        spawningCoroutine = null; // 코루틴 종료 후 null로 설정
    }
}
