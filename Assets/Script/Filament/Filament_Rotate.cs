using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Filament_Rotate : MonoBehaviour
{
    public static Filament_Rotate instance;
    // ������ prefab �ν��Ͻ��� ���� -> GameObjectŸ��
    // prefab GameObject Ÿ��
    [SerializeField] GameObject framPrefab1;
    [SerializeField] GameObject mainprefab2;
    [SerializeField] GameObject Filamnetprefab;   // ������ Filament
    private GameObject instancePrefab1;
    private GameObject instancePrefab2;

    private float RotationSpeed = 50f; // prefab ȸ�� �ӵ�
    private const float ScaleIncreaseRate = 0.08f; // prefab ������ ���� �ӵ�
    private const float MaxScale = 0.6f; // prefab �ִ� ������
    // private bool FimanetMove = false;
    public Button FilamentSpawn;


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    private void Start()
    {
        FilamentSpawn.gameObject.SetActive(false);
        FilamentSpawn.onClick.AddListener(OnSpawnButtonClick);
    }

    public void Cpf()
    {

        // Scale ����(Vector3.one - (1, 1, 1) ������)
        instancePrefab1 = CreatePrefab(framPrefab1, new Vector3(0.6f, 0.6f, 0.6f));
        // Debug.Log("������1"); 
        instancePrefab2 = CreatePrefab(mainprefab2, new Vector3(0.1f, 0.1f, 0.6f));
        // Debug.Log("������2");
    }


    private GameObject CreatePrefab(GameObject prefab, Vector3 scale)
    {
        GameObject instance = Instantiate(prefab, transform.position, Quaternion.Euler(0, 90, 0));
        instance.transform.localScale = scale;
        // Debug.Log("1111");
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
                prefab.transform.localScale += new Vector3(scaleIncrease, scaleIncrease, 0);

            if (prefab.transform.localScale.x >= MaxScale)
            {
                StopRotation();
            }
        }
    }

    // ȸ�� �ӵ��� 0���� �ؼ� ���߰� ��
    private void StopRotation()
    {
        RotationSpeed = 0f;
        FilamentSpawn.gameObject.SetActive(true);
    }


    public void OnSpawnButtonClick()
    {
        Destroy(instancePrefab1);
        Destroy(instancePrefab2);

        Instantiate(Filamnetprefab, new Vector3(-17.678f, 0.726f, 0.302f), Quaternion.Euler(90, 0, 0));

        FilamentSpawn.gameObject.SetActive(false);
    }
}

// Filament position 7.5102 1.9204 2.5876
// ���� 90, 90, 0
// Filament Spool Scale 0.4 1 0.4
// Filament Spool 002 Scale 0.5 0.5 0.5
