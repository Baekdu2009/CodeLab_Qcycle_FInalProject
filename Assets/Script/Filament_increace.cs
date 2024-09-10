using UnityEngine;

public class Filament_increace : MonoBehaviour
{
    public float initialScale = 0.2f; // 초기 스케일
    public float scaleIncreaseSpeed; // 길이 증가 속도
    private bool isScaling = true;  // 스케일 증가 여부
    private float maxScale; // 최대 스케일
    public float maxScaleMultiplier; // 최대 스케일배수

    void Start()
    {
        // 초기 스케일 설정
        transform.localScale = new Vector3(initialScale, initialScale, initialScale);
        maxScale = initialScale * maxScaleMultiplier; // 최대 스케일 설정
    }

    void Update()
    {
        // 실린더의 Y축 스케일을 증가시켜 길이 늘리기
        Vector3 currentScale = transform.localScale;
        currentScale.y += scaleIncreaseSpeed * Time.deltaTime; // Y축 스케일 증가

        if(currentScale.y > 9)
        {
            currentScale.y = 9; // 최대 스케일로 설정
            isScaling = false;    // 스케일 증가 중지
        }

        transform.localScale = currentScale; // 새로운 스케일 적용
    }
}