using UnityEngine;
using System.Collections;

public class Conveyor4 : MonoBehaviour
{
    public float speed = 1.0f; // 컨베이어 벨트 속도
    public GameObject[] clints; // clint 오브젝트 배열
    private Vector3[] positions; // clint의 위치 배열

    void Start()
    {
        // clint 오브젝트를 찾기
        clints = new GameObject[36];
        positions = new Vector3[36];

        for (int i = 0; i < 36; i++)
        {
            string clintName = $"CLINT_{i:D2}"; // CLINT_00 형식으로 이름 생성
            clints[i] = GameObject.Find(clintName);
            if (clints[i] != null)
            {
                // clint의 초기 위치 저장
                positions[i] = clints[i].transform.position;
            }
            else
            {
                Debug.LogWarning($"클린트 {clintName}를 찾을 수 없습니다.");
            }
        }

        // 모든 클린트를 동시에 이동 시작
        for (int i = 0; i < clints.Length; i++)
        {
            if (clints[i] != null)
            {
                StartCoroutine(MoveClint(i)); // 각 클린트를 위한 코루틴 시작
            }
        }
    }

    void Update()
    {
        // 현재 위치를 positions 배열에 저장
        for (int i = 0; i < clints.Length; i++)
        {
            if (clints[i] != null)
            {
                positions[i] = clints[i].transform.position;
            }
        }
    }

    private IEnumerator MoveClint(int index)
    {
        while (true) // 무한 루프
        {
            int nextIndex = (index + 1) % clints.Length; // 다음 클린트 인덱스
            GameObject clint = clints[index];
            Vector3 targetPosition = positions[nextIndex];

            // 목표 위치로 이동
            while (Vector3.Distance(clint.transform.position, targetPosition) > 0.01f)
            {
                clint.transform.position = Vector3.MoveTowards(clint.transform.position, targetPosition, speed * Time.deltaTime);
                yield return null; // 다음 프레임까지 대기
            }

            // 이동 완료 후 위치 설정
            clint.transform.position = targetPosition;

            // 다음 목표 위치 설정
            index = nextIndex; // 인덱스 업데이트

            // 잠시 대기 (이동이 완료된 후)
            yield return new WaitForSeconds(0.1f); // 원하는 대기 시간 조정 가능
        }
    }
}
