using System.Collections;
using UnityEngine;

public class BoxSpawn : MonoBehaviour
{
    public GameObject SqawnBox;
    public ObjectDestroy ObjectDestroy;
    private float delayTime = 5f;
    private Coroutine spawnCoroutine;
    public bool spawnOn = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        spawnOn = ObjectDestroy.objectDestroied;

        if (spawnOn)
        {
            StartSpawning();
        }
        else
        {
            StopSpawning();
        }
    }

    public void StartSpawning()
    {
        if (spawnCoroutine == null) // 이미 코루틴이 실행 중인지 확인
        {
            spawnCoroutine = StartCoroutine(BoxSpawnCoroutine()); // 코루틴 시작
        }
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null) // 실행 중인 코루틴이 있을 경우
        {
            StopCoroutine(spawnCoroutine); // 코루틴 중지
            spawnCoroutine = null; // 참조 초기화
        }
    }

    private IEnumerator BoxSpawnCoroutine()
    {
        while (true) // 무한 루프
        {
            Quaternion BoxSqawnRotate = Quaternion.Euler(-90, 0, 180);
            // 박스를 생성
            Instantiate(SqawnBox, transform.position, BoxSqawnRotate);
            // 대기 시간
            yield return new WaitForSeconds(delayTime);
        }
    }
}
