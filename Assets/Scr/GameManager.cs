

using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public GameObject cylinderPrefab; // 실린더 프리팹
    public float scaleIncreaseSpeed; // 길이 증가 속도


    void Start()
    {
        // (0, 0, 0) 위치에 Z축으로 90도 회전하여 실린더 생성
        Quaternion rotation = Quaternion.Euler(0, 0, 90); // Z축으로 90도 회전
        
        // GameManager의 위치에서 실린더 생성
        GameObject cylinder = Instantiate(cylinderPrefab, transform.position, rotation);

        // CylinderController 스크립트를 추가하여 초기화
        Filament_increace controller = cylinder.AddComponent<Filament_increace>();
        controller.initialScale = 0.01f; // 초기 스케일 설정
        controller.scaleIncreaseSpeed = scaleIncreaseSpeed; // 길이 증가 속도 설정
    }
}