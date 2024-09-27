using UnityEngine;

public class Filament_increace1 : MonoBehaviour
{
    public float initialScale = 0.001f; // 초기 Scale
    public float scaleIncreaseSpeed; // 길이 증가 속도
    private bool isScaling = true;  // Scale 증가 여부
    public float maxScale = 1.65f; // 최대 Scale
    public float xMoveSpeed; // X축 이동 속도

    void Start()
    {
        // 초기 Scale 설정
        transform.localScale = new Vector3(initialScale, initialScale, initialScale);
    }

    void Update()
    {
        if (isScaling)
        {
            // 실린더의 Y축 Scale을 증가시켜 길이 늘리기
            Vector3 currentScale = transform.localScale;
            currentScale.y += scaleIncreaseSpeed * Time.deltaTime; // Y축 Scale 증가 양쪽으로 증가
            transform.position += new Vector3(scaleIncreaseSpeed * Time.deltaTime, 0, 0); // 위치 조정 (X축으로 이동)

            // Y축 Scale이 1.65 초과 시 제한
            if (currentScale.y > maxScale)
            {
                currentScale.y = maxScale; // 최대 Scale로 설정
                isScaling = false;    // Scale 증가 중지
                GameManager1.instance.SpawnPrefab1();
            }

            // 새로운 Scale 적용(매 프레임마다 변화하는 스케일 적용)
            transform.localScale = currentScale;
        }
    }
}