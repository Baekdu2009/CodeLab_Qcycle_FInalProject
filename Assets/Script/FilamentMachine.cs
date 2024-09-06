using UnityEngine;

public class FilamentMachine : MonoBehaviour
{
    public GameObject filamentRoller;
    public GameObject filamentPrefab;
    public Transform spawnFilament; // Unity �����Ϳ��� ���� ����
    float rotSpeed = 200;
    private GameObject currentFilament; // ���� ������ filament ����
    MeshRenderer filamentMesh;


    void Start()
    {
        if (spawnFilament == null)
        {
            Debug.LogError("spawnFilament�� �������� �ʾҽ��ϴ�. Unity �����Ϳ��� ������ �ּ���.");
        }
        else
        {
            spawnFilament.position = filamentRoller.transform.position; // �ʱ� ��ġ ����
        }
    }

    void Update()
    {
        RollerRotate();
    }

    void RollerRotate()
    {
        filamentRoller.transform.Rotate(0, 0, -rotSpeed * Time.deltaTime);
    }

    public void OnFilamentSpawnBtn()
    {
        if (spawnFilament != null)
        {
            if (currentFilament == null) // ���� filament�� ���� ��쿡�� ����
            {
                currentFilament = Instantiate(filamentPrefab, spawnFilament.position, Quaternion.Euler(90, 0, 0));
                filamentMesh = currentFilament.GetComponent<MeshRenderer>();
                filamentMesh.material.color = Color.red;
            }
            else
            {
                Debug.LogWarning("�̹� filament�� �����Ǿ� �ֽ��ϴ�.");
            }
        }
        else
        {
            Debug.LogError("spawnFilament�� null�Դϴ�. �ʶ��Ʈ�� ������ �� �����ϴ�.");
        }
    }

    public GameObject GetCurrentFilament()
    {
        return currentFilament; // ���� filament ��ȯ
    }
}
