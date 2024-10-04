using System.Collections;
using UnityEngine;

public class Filament_Rotate : MonoBehaviour
{
    public static Filament_Rotate instance;
    // prefab GameObject 타입
    [SerializeField] GameObject framPrefab1;
    [SerializeField] GameObject mainprefab2;

    // 생성된 prefab 인스턴스에 저장 -> GameObject타입
    private GameObject instancePrefab1;
    private GameObject instancePrefab2;
    private float RotationSpeed = 100f; // prefab 회전 속도
    private const float ScaleIncreaseRate = 10f; // prefab 스케일 증가 속도
    private const float MaxScale = 100f; // prefab 최대 스케일
    private float DelayTime = 2f;


    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

   /* private void Start()
    {
        // Scale 설정(Vector3.one - (1, 1, 1) 스케일)
        instancePrefab1 = CreatePrefab(framPrefab1, new Vector3(100f, 100f, 100f));
        instancePrefab2 = CreatePrefab(mainprefab2, new Vector3(40f, 100f, 40f));
    }*/

    public void Cpf()
    {
        // Scale 설정(Vector3.one - (1, 1, 1) 스케일)
        instancePrefab1 = CreatePrefab(framPrefab1, new Vector3(100f, 100f, 100f));
        Debug.Log("프리탭1");
        instancePrefab2 = CreatePrefab(mainprefab2, new Vector3(40f, 100f, 40f));
        Debug.Log("프리탭2");
    }


    private GameObject CreatePrefab(GameObject prefab, Vector3 scale)
    {
        GameObject instance = Instantiate(prefab, transform.position, Quaternion.Euler(90, 90, 0));
        instance.transform.localScale = scale;
        Debug.Log("1111");
        return instance;
        // instancePrefab1에 저장하기 때문에 반환을 해줘야한다.
        // 만약 호출만 하게 되면 return이 필요가 없다. (CreatePrefab();)
    }

    private void Update()
    {
        RotatePrefab(instancePrefab1);
        RotateAndScalePrefab(instancePrefab2);
    }

    private void RotatePrefab(GameObject prefab)
    {
        if (prefab != null)
        {
            prefab.transform.Rotate(new Vector3(RotationSpeed, 0, 0) * Time.deltaTime, Space.World);
        }
    }

    private void RotateAndScalePrefab(GameObject prefab)
    {
        if (prefab != null)
        {
            prefab.transform.Rotate(new Vector3(RotationSpeed, 0, 0) * Time.deltaTime, Space.World);
            float scaleIncrease = Time.deltaTime * ScaleIncreaseRate;

            if (prefab.transform.localScale.x < MaxScale)
                prefab.transform.localScale += new Vector3(scaleIncrease, 0, scaleIncrease);

            if (prefab.transform.localScale.x >= MaxScale)
            {
                StopRotation();
                // Debug.Log("회전 멈춤");

                StartCoroutine(DelayPrefab(instancePrefab1, DelayTime));
                StartCoroutine(DelayPrefab(instancePrefab2, DelayTime));
            }
        }
    }

    // Coroutine을 사용해서 2초 뒤에 Destroy함
    public IEnumerator DelayPrefab(GameObject prefab, float waitTime)
    {

        yield return new WaitForSeconds(waitTime);
        // Debug.Log("기다림");
        Destroy(prefab);
    }

    // 회전 속도를 0으로 해서 멈추게 함
    private void StopRotation()
    {
        RotationSpeed = 0f;
    }
}

    // Filament position 7.5102 1.9204 2.5876
    // 방향 90, 90, 0
    // Filament Spool Scale 0.4 1 0.4
    // Filament Spool 002 Scale 0.5 0.5 0.5
