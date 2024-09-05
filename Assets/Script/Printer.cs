using UnityEngine;

public class Printer : MonoBehaviour
{
    public Transform filamentLocation;
    GameObject filamentObject;
    Road road;

    void Start()
    {
        road = FindAnyObjectByType<Road>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnFilamentLocationBtn()
    {
        if (road != null)
        {
            filamentObject = road.GetCurrentFilament(); // Road���� ���� filament ��������

            if (filamentObject != null) // filament�� �����ϴ��� Ȯ��
            {
                filamentObject.transform.position = filamentLocation.position; // ��ġ ����
                filamentObject.transform.rotation = Quaternion.Euler(90, 0, 0);
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
}
