using System.Collections;
using UnityEngine;

public class Filament_manager1 : MonoBehaviour
{
    public static Filament_manager1 instance;
    public GameObject Filament; // Filament 프리팹
    public GameObject Filamentdia; // 대각선 Filament 프리팹
    public GameObject Filament2;  // Filament2 프리팹

    public float scaleIncreaseSpeed; // 길이 증가 속도
    float delayTime = 2.0f;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    void Start()
    {
        // (0, 0, 0) 위치에 Z축으로 90도 회전하여 Filament 생성
        Quaternion rotation = Quaternion.Euler(0, 0, 90); // Z축으로 90도 회전

        // GameManager의 위치에서 Filament 생성
        GameObject FilamentIn = Instantiate(Filament, transform.position, rotation);

        // 필요시 주석 제거
        // CylinderController 스크립트를 추가하여 초기화
         /*Filament_increace controller = FilamentIn.AddComponent<Filament_increace>();
         controller.scaleIncreaseSpeed = scaleIncreaseSpeed;     // 길이 증가 속도 설정*/
       
        
        // 26.5 1.7 -6.39 rotate z -50
        // 27.0601 2.0876 -6.39 rotate z 90 Scale 0.2067529
    }

    public void SpawnPrefab1()
    {
        if (Filamentdia != null)
        {
            // 생성할 위치 및 회전
            Vector3 SpawnPosition = new Vector3(26.7042f, 1.90857f, -6.39f);
            Quaternion SpawnRotation = Quaternion.Euler(0, 0, -50);

            // Filamentdia 생성
            GameObject FilamentdiaIn = Instantiate(Filamentdia, SpawnPosition, SpawnRotation);
            Debug.Log("Filamentdia 생성");

            StartCoroutine(SpawnWithDelay());
        }
    }

    public IEnumerator SpawnWithDelay()
    {
        if (Filament2 != null)
        {
            // 생성할 위치 및 회전
            Vector3 SpawnPosition2 = new Vector3(27.135f, 2.0876f, -6.39f);
            Quaternion SpawnRotation2 = Quaternion.Euler(0, 0, 90);

            // Filament2 생성
            GameObject Filament2In = Instantiate(Filament2, SpawnPosition2, SpawnRotation2);
            Debug.Log("Filament2 생성");

            yield return new WaitForSeconds(delayTime);

        }
    }
}