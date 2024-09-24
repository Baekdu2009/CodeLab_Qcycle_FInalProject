using UnityEngine;

public class Printer : MonoBehaviour
{
    public Transform filamentLocation; // �ʶ��Ʈ ��ġ
    GameObject filamentObject; // ���� �ʶ��Ʈ ��ü
    Road road; // Road ����
    float rotSpeed = 200;
    public GameObject itemPrefab;
    GameObject examItem;
    public Transform itemSpawnLocation;
    public Transform dropLocation;
    public float dropSpeed = 2;


    void Start()
    {
        road = FindAnyObjectByType<Road>(); // Road Ÿ���� ��ü�� ã��
    }

    void Update()
    {
        
    }

    public void OnFilamentLocationBtn()
    {
        filamentObject = null;

        if (road != null && filamentObject == null)
        {
            filamentObject = road.GetCurrentFilament(); // Road���� filament ��ü ��������
            print(filamentObject);

            if (filamentObject != null) // filament�� �����ϴ��� Ȯ��
            {
                filamentObject.transform.position = filamentLocation.position; // ��ġ ����
                filamentObject.transform.rotation = Quaternion.Euler(90, 0, 0); // ȸ�� ����
            }
            else
            {
                Debug.LogError("���� filament�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("Road ��ü�� ã�� �� �����ϴ�.");
        }
    }

    public void OnOperationBtn()
    {
        filamentObject.transform.Rotate(0, 0, rotSpeed * Time.deltaTime);
        examItem = Instantiate(itemPrefab, itemSpawnLocation);

        Vector3 direction = (examItem.transform.position - dropLocation.position).normalized;
        float distance = direction.magnitude;

        if (distance > 0.1f)
        {
            examItem.transform.position += direction * (-dropSpeed) * Time.deltaTime;
        }
    }
}
