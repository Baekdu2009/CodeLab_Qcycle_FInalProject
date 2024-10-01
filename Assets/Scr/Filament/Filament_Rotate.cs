using System.Collections;
using UnityEngine;

public class Filament_Rotate : MonoBehaviour
{
    public static Filament_Rotate instance;
    // prefab GameObject Ÿ��
    [SerializeField] GameObject framPrefab1;
    [SerializeField] GameObject mainprefab2;

    // ������ prefab �ν��Ͻ��� ���� -> GameObjectŸ��
    private GameObject instancePrefab1;
    private GameObject instancePrefab2;
    private float RotationSpeed = 100f; // prefab ȸ�� �ӵ�
    private const float ScaleIncreaseRate = 10f; // prefab ������ ���� �ӵ�
    private const float MaxScale = 100f; // prefab �ִ� ������
    private float DelayTime = 2f;


    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

   /* private void Start()
    {
        // Scale ����(Vector3.one - (1, 1, 1) ������)
        instancePrefab1 = CreatePrefab(framPrefab1, new Vector3(100f, 100f, 100f));
        instancePrefab2 = CreatePrefab(mainprefab2, new Vector3(40f, 100f, 40f));
    }*/

    public void Cpf()
    {
        // Scale ����(Vector3.one - (1, 1, 1) ������)
        instancePrefab1 = CreatePrefab(framPrefab1, new Vector3(100f, 100f, 100f));
        Debug.Log("������1");
        instancePrefab2 = CreatePrefab(mainprefab2, new Vector3(40f, 100f, 40f));
        Debug.Log("������2");
    }


    private GameObject CreatePrefab(GameObject prefab, Vector3 scale)
    {
        GameObject instance = Instantiate(prefab, transform.position, Quaternion.Euler(90, 90, 0));
        instance.transform.localScale = scale;
        Debug.Log("1111");
        return instance;
        // instancePrefab1�� �����ϱ� ������ ��ȯ�� ������Ѵ�.
        // ���� ȣ�⸸ �ϰ� �Ǹ� return�� �ʿ䰡 ����. (CreatePrefab();)
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
                // Debug.Log("ȸ�� ����");

                StartCoroutine(DelayPrefab(instancePrefab1, DelayTime));
                StartCoroutine(DelayPrefab(instancePrefab2, DelayTime));
            }
        }
    }

    // Coroutine�� ����ؼ� 2�� �ڿ� Destroy��
    public IEnumerator DelayPrefab(GameObject prefab, float waitTime)
    {

        yield return new WaitForSeconds(waitTime);
        // Debug.Log("��ٸ�");
        Destroy(prefab);
    }

    // ȸ�� �ӵ��� 0���� �ؼ� ���߰� ��
    private void StopRotation()
    {
        RotationSpeed = 0f;
    }
}

    // Filament position 7.5102 1.9204 2.5876
    // ���� 90, 90, 0
    // Filament Spool Scale 0.4 1 0.4
    // Filament Spool 002 Scale 0.5 0.5 0.5
