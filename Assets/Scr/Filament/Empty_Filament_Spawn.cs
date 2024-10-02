using System.Collections;
using UnityEngine;

public class Empty_Filament_Spawn : MonoBehaviour
{
    [SerializeField] GameObject Filamentdia; // 대각선 Filament 프리팹
    [SerializeField] GameObject Filament2; // Filament2 프리팹
    [SerializeField] float initialScale = 0.01f; // 초기 Scale
    [SerializeField] float maxScale = 1.65f; // 최대 Scale
    [SerializeField] float scaleIncreaseSpeed = 1f; // 길이 증가 속도
    [SerializeField] float xMoveSpeed = 1f; // X축 이동 속도
    [SerializeField] float delayTime = 2.0f;

    private Transform spawnTransform; // CreateEmpty에서 전달받을 Transform
    bool isScaling = true; // Scale 증가 여부

    public void Start()
    {
        // 초기 Scale 설정
        transform.localScale = new Vector3(initialScale, initialScale, initialScale);
    }

    // CreateEmpty에서 Transform을 설정하는 메서드
    public void SetSpawnTransform(Transform transform)
    {
        spawnTransform = transform;
    }

    void Update()
    {
        if (isScaling)
        {
            UpdateScaleAndPosition();

            // Y축 Scale이 최대치를 초과 시 제한
            if (transform.localScale.x >= maxScale)
            {
                FinalizeScaling();
            }
        }
    }

    void UpdateScaleAndPosition()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x += scaleIncreaseSpeed * Time.deltaTime;
        transform.localScale = currentScale; // 새로운 Scale 적용
        transform.Translate(new Vector3(xMoveSpeed * Time.deltaTime, 0, 0), Space.World); // X축으로 이동
    }

    void FinalizeScaling()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x = maxScale; // 최대 Scale로 설정
        transform.localScale = currentScale;
        isScaling = false;

        SpawnPrefab1();
    }

    void SpawnPrefab1()
    {
        if (Filamentdia != null)
        {
            // CreateEmpty의 위치를 사용하여 생성
            Vector3 spawnPosition = spawnTransform.position;
            Quaternion spawnRotation = Quaternion.Euler(0, 0, -50);

            Instantiate(Filamentdia, spawnPosition, spawnRotation);
            StartCoroutine(SpawnWithDelay());
        }
    }

    IEnumerator SpawnWithDelay()
    {
        yield return new WaitForSeconds(delayTime);

        if (Filament2 != null)
        {
            // CreateEmpty의 위치를 사용하여 생성
            Vector3 spawnPosition2 = spawnTransform.position;
            Quaternion spawnRotation2 = Quaternion.Euler(0, 0, 90);

            Instantiate(Filament2, spawnPosition2, spawnRotation2);
        }
    }
}
