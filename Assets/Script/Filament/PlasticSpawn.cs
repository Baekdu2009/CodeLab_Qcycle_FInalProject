using System.Collections;
using UnityEngine;

public class PlasticSpawn : MonoBehaviour
{
    [SerializeField] GameObject[] prefabs;

    [HideInInspector]
    private bool isOn;
    private Coroutine spawnCoroutine; // 코루틴 인스턴스를 저장할 변수

    public void OnBtnSpawn()
    {
        isOn = !isOn;

        if (isOn)
        {
            // 코루틴이 실행 중이지 않을 때만 새로 시작
            if (spawnCoroutine == null)
            {
                spawnCoroutine = StartCoroutine(SpawnObject());
            }
        }
        else
        {
            // 코루틴이 실행 중이면 중지하고 변수 초기화
            if (spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine);
                spawnCoroutine = null; // 코루틴 인스턴스 초기화
            }
        }
    }

    private IEnumerator SpawnObject()
    {
        while (isOn) // isOn이 true인 동안 계속해서 생성
        {
            int rand = Random.Range(0, prefabs.Length);
            GameObject newObj = Instantiate(prefabs[rand]);

            newObj.transform.SetParent(transform);
            newObj.transform.position = transform.position;

            yield return new WaitForSeconds(2f); // 2초 대기
        }

        spawnCoroutine = null; // 코루틴 종료 시 인스턴스 초기화
    }
}
