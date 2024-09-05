using UnityEngine;
using System.Collections;

public class Road : MonoBehaviour
{
    public Transform roadStart; // 시작 위치
    public Transform roadEnd; // 끝 위치
    public float speed = 2.0f; // 이동 속도
    private GameObject filament; // 인스턴스화된 filament
    FilamentMachine filamentMachine;

    void Start()
    {
        filamentMachine = FindAnyObjectByType<FilamentMachine>();
    }

    public void OnRoadMoveBtn()
    {
        if (filamentMachine != null)
        {
            filament = filamentMachine.GetCurrentFilament(); // filament 가져오기

            if (filament == null)
            {
                Debug.LogError("FilamentMachine에서 filament를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("FilamentMachine을 찾을 수 없습니다.");
        }

        if (filament != null) // filament가 존재하는 경우에만 이동
        {
            StartCoroutine(RoadMove(roadStart, roadEnd));
        }
        else
        {
            Debug.LogError("이동할 filament가 없습니다.");
        }
    }

    IEnumerator RoadMove(Transform start, Transform end)
    {
        filament.transform.position = start.position;
        filament.transform.rotation = Quaternion.Euler(0, 0, 0);

        while (Vector3.Distance(filament.transform.position, end.position) > 0.1f)
        {
            // 현재 위치와 목표 위치 사이의 방향 벡터 계산
            Vector3 direction = (end.position - filament.transform.position).normalized;

            // 속도에 따라 이동
            filament.transform.position += direction * speed * Time.deltaTime;

            yield return null; // 다음 프레임까지 대기
        }

        // 최종 위치 보정
        filament.transform.position = end.position;
    }
    public GameObject GetCurrentFilament()
    {
        return filament; // 현재 filament 반환
    }
}
