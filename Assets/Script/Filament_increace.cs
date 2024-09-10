using UnityEngine;

public class Filament_increace : MonoBehaviour
{
    public float initialScale = 0.2f; // 초기 Scale
    public float scaleIncreaseSpeed; // 길이 증가 속도
    private bool isScaling = true;  // Scale 증가 여부
    private float maxScale; // 최대 Scale
    public float maxScaleMultiplier; // 최대 Scale배수
    public float xMoveSpeed; // X축 이동 속도



    void Start()
    {
        // 초기 Scale 설정
        transform.localScale = new Vector3(initialScale, initialScale, initialScale);
        maxScale = initialScale * maxScaleMultiplier; // 최대 Scale 설정
    }

    void Update()
    {
        if (isScaling)
        {
            // 실린더의 Y축 Scale을 증가시켜 길이 늘리기
            Vector3 currentScale = transform.localScale;
            currentScale.y += scaleIncreaseSpeed * Time.deltaTime; // Y축 Scale 증가 양쪽으로 증가
            transform.position += new Vector3(scaleIncreaseSpeed * Time.deltaTime, 0, 0); // 위치 조정 (X축으로 이동)

            // Y축 Scale이 9 초과 시 제한
            if (currentScale.y > 9)
            {
                currentScale.y = 9; // 최대 Scale로 설정
                isScaling = false;    // Scale 증가 중지
            }

            transform.localScale = currentScale; // 새로운 Scale 적용 

            // X축으로 이동 (오브젝트를 오른쪽으로 이동)
            transform.position += new Vector3(xMoveSpeed * Time.deltaTime, 0, 0);
        }
        else
        {
            // Scale이 최대에 도달했을 때 X축 이동 중지
            // 이 부분은 아무것도 하지 않음
        }

    }
}