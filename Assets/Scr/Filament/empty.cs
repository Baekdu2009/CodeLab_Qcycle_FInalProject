using UnityEngine;

public class CreateEmpty : MonoBehaviour
{
    [SerializeField] GameObject filamentPrefab; // Filament_increace1�� �پ��ִ� ������

    void Start()
    {
        // Filament �������� CreateEmpty�� ��ġ�� ����
        GameObject filamentInstance = Instantiate(filamentPrefab, transform.position, Quaternion.identity);

        // �ʿ��� �ʱ�ȭ �۾�
        Empty_Filament_Spawn filamentScript = filamentInstance.GetComponent<Empty_Filament_Spawn>();
        if (filamentScript != null)
        {
            filamentScript.SetSpawnTransform(transform); // CreateEmpty�� Transform�� ����
        }
    }
}

