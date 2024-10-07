using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSensor : MonoBehaviour
{
    [SerializeField] private int collisionCount = 0; // 충돌 수
    public bool isDetected = false; // 감지 상태

    private HashSet<Collider> collidedPlastics = new HashSet<Collider>(); // 중복 충돌 방지를 위한 리스트

    private void Update()
    {
        // 감지 횟수가 150회 이상이면 isDetected를 true로 설정
        if (collisionCount >= 150 && !isDetected)
        {
            isDetected = true;
            Debug.Log("isDetected 활성화!");
            StartCoroutine(RemovePlastics(0.4f, 4)); // 30%씩 4번 제거
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 Plastic 태그인지 확인
        if (other.CompareTag("Plastic"))
        {
            // 중복 충돌 방지
            if (!collidedPlastics.Contains(other))
            {
                collidedPlastics.Add(other); // 현재 충돌 중인 Plastic을 추가
                collisionCount++;
                Debug.Log("Plastic과 충돌 감지: " + collisionCount);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 충돌이 끝나면 해당 Collider 제거
        if (other.CompareTag("Plastic"))
        {
            collidedPlastics.Remove(other); // 중복 충돌 리스트에서 제거
            Debug.Log("Plastic과의 충돌 종료: " + other.gameObject.name);
        }
    }

    private IEnumerator RemovePlastics(float percentage, int times)
    {
        for (int i = 0; i < times; i++)
        {
            // 현재 모든 Plastic 오브젝트를 가져옴
            GameObject[] allPlastics = GameObject.FindGameObjectsWithTag("Plastic");
            int totalPlastics = allPlastics.Length;
            int countToRemove = Mathf.CeilToInt(totalPlastics * percentage);

            for (int j = 0; j < countToRemove && allPlastics.Length > 0; j++)
            {
                int randomIndex = Random.Range(0, allPlastics.Length);
                GameObject plasticToRemove = allPlastics[randomIndex];

                if (plasticToRemove != null)
                {
                    Destroy(plasticToRemove);
                    Debug.Log("씬에서 플라스틱 제거됨: " + plasticToRemove.name);
                    allPlastics = GameObject.FindGameObjectsWithTag("Plastic"); // 갱신된 Plastic 배열
                }
            }

            yield return new WaitForSeconds(3f); // 각 단계 후 1초 대기
        }

        ResetDetection(); // 모든 플라스틱 제거 후 초기화
    }

    private void ResetDetection()
    {
        isDetected = false; // 감지 상태 초기화
        collisionCount = 0; // 충돌 수 초기화
        collidedPlastics.Clear(); // 중복 충돌 리스트 초기화
        Debug.Log("감지 상태와 충돌 수 초기화 완료!");
    }
}
