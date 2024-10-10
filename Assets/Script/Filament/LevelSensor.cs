using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSensor : MonoBehaviour
{
    [SerializeField] private int collisionCount = 0; // 충돌 수
    public bool isDetected = false; // 감지 상태

    private HashSet<Collider> collidedPlastics = new HashSet<Collider>(); // 중복 충돌 방지를 위한 리스트
    private LevelSensorExtruder extruder; // LevelSensorExtruder의 인스턴스
    private Coroutine reduceCoroutine; // 플라스틱 제거 코루틴
    private int sensingChangeCount = 0; // isSensing 상태 변화 카운트
    private bool lastIsSensingState = false; // 마지막 isSensing 상태 저장

    private void Start()
    {
        // 씬에서 LevelSensorExtruder 찾기
        extruder = FindObjectOfType<LevelSensorExtruder>();
        if (extruder == null)
        {
            Debug.LogError("LevelSensorExtruder를 찾을 수 없습니다.");
        }
    }

    private void Update()
    {
        // 감지 횟수가 150회 이상이면 isDetected를 true로 설정
        if (collisionCount >= 150 && !isDetected)
        {
            isDetected = true;
            Debug.Log("isDetected 활성화!");
           
        }

        // 조건: !isSensing && !isDetected
        if (!extruder.isSensing && !isDetected)
        {
            if (reduceCoroutine == null)
            {
                reduceCoroutine = StartCoroutine(ReducePlasticCount(0.2f)); // 20% 제거
            }
        }

        // 상태 변화 감지
        if (extruder.isSensing)
        {
            if (!lastIsSensingState) // 마지막 상태가 false였을 때
            {
                lastIsSensingState = true; // 현재 상태를 true로 변경
            }
        }
        else
        {
            if (lastIsSensingState) // 마지막 상태가 true였을 때
            {
                sensingChangeCount++;
                lastIsSensingState = false; // 현재 상태를 false로 변경
                Debug.Log("isSensing 상태 변화: " + sensingChangeCount);
            }
        }

        // 상태가 false에서 true로 다시 바뀔 때 카운트 증가
        if (sensingChangeCount >= 4)
        {
            ResetDetection(); // 4번 변화 후 초기화
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 Plastic 태그인지 확인
        if (other.CompareTag("Plastic"))
        {
            // isDetected가 활성화되어 있을 때는 충돌 수를 증가시키지 않음
            if (!isDetected)
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

    private IEnumerator ReducePlasticCount(float percentage)
    {
        while (!extruder.isSensing) // isSensing이 비활성화된 동안만 반복
        {
            GameObject[] allPlastics = GameObject.FindGameObjectsWithTag("Plastic");
            int totalPlastics = allPlastics.Length;

            if (totalPlastics > 0)
            {
                // 남아있는 플라스틱의 일정 비율 제거
                int countToRemove = Mathf.CeilToInt(totalPlastics * percentage);

                for (int i = 0; i < countToRemove && totalPlastics > 0; i++)
                {
                    int randomIndex = Random.Range(0, totalPlastics);
                    GameObject plasticToRemove = allPlastics[randomIndex];

                    if (plasticToRemove != null)
                    {
                        Destroy(plasticToRemove);
                        Debug.Log("씬에서 플라스틱 제거됨: " + plasticToRemove.name);
                        allPlastics = GameObject.FindGameObjectsWithTag("Plastic"); // 리스트 업데이트
                        totalPlastics = allPlastics.Length; // 업데이트된 수로 다시 계산
                    }
                    yield return new WaitForSeconds(0.1f); // 조금의 대기 시간
                }

                yield return new WaitForSeconds(3f); // 다음 반복까지 3초 대기
            }
            else
            {
                break; // 더 이상 제거할 플라스틱이 없으면 종료
            }
        }

        reduceCoroutine = null; // 코루틴 종료 후 null로 설정
    }

    private void ResetDetection()
    {
        isDetected = false; // 초기화
        collisionCount = 0; // 충돌 수 초기화
        sensingChangeCount = 0; // 상태 변화 카운트 초기화
        collidedPlastics.Clear(); // 중복 충돌 리스트 초기화

        Debug.Log("충돌 상태 초기화됨.");
    }
}