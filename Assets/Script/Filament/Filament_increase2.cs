using UnityEngine;
using System.Collections;
using UnityEngine.PlayerLoop;

public class Filament_increase2 : MonoBehaviour
{
    [SerializeField] GameObject Filamentdia; // 대각선 Filament 프리팹
    [SerializeField] GameObject Filament2; // Filament2 프리팹
    [SerializeField] GameObject Filament3; // Filament3 프리팹
    [SerializeField] float initialScale = 0.01f; // 초기 Scale
    [SerializeField] float maxScale = 1.4f; // 최대 Scale
    [SerializeField] float scaleIncreaseSpeed = 70f; // 길이 증가 속도
    [SerializeField] float zMoveSpeed = 70f; // X축 이동 속도

    bool isScaling = true;  // Scale 증가 여부
    float delayTime = 2.0f;
    void Start()
    {
       // Filament_Rotate.FilamentSpawn.gameObject.SetActive(false);
        transform.localScale = new Vector3(initialScale, initialScale, initialScale);
    }


    void Update()
    {
        if (isScaling)
        {
            UpdateScaleAndPosition();

            if (transform.localScale.y >= maxScale)
            {
                FinalizeScaling();
            }
        }
    }



    void UpdateScaleAndPosition()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.y += scaleIncreaseSpeed * Time.deltaTime;
        transform.localScale = currentScale;
        transform.Translate(new Vector3(0, 0, - zMoveSpeed * Time.deltaTime), Space.World);
    }

    void FinalizeScaling()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.y = maxScale;
        transform.localScale = currentScale;
        isScaling = false;

        SpawnPrefab1();
    }

    void SpawnPrefab1()
    {
        if (Filamentdia != null)
        {
            Vector3 SpawnPosition = new Vector3(-16.029f, 1.183f, 5.334f);
            Quaternion SpawnRotation = Quaternion.Euler(0, 90, -12.5f);

            GameObject FilamentSpDia = Instantiate(Filamentdia, SpawnPosition, SpawnRotation);

            StartCoroutine(SpawnWithDelay());
        }
    }

    IEnumerator SpawnWithDelay()
    {
        if (Filament2 != null)
        {
            yield return new WaitForSeconds(delayTime);

            Vector3 SpawnPosition2 = new Vector3(-16.027f, 1.631f, 4.758f);
            Quaternion SpawnRotation2 = Quaternion.Euler(0, 90, 90);

            GameObject FilamentSp2 = Instantiate(Filament2, SpawnPosition2, SpawnRotation2);

            SpawnWithDelay2();
        }
    }

    void SpawnWithDelay2()
    {
        if (Filament3 != null)
        {
            Vector3 SpawnPosition3 = new Vector3(-16.026f, 1.39f, 3.891f);
            Quaternion SpawnRotation3 = Quaternion.Euler(0, 90, 58);

            GameObject FilamentSp3 = Instantiate(Filament3, SpawnPosition3, SpawnRotation3);
        }
        Filament_Rotate.instance.Cpf();
    }
}