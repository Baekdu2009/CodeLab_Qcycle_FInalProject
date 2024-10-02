using UnityEngine;

public class CreateEmpty : MonoBehaviour
{
    [SerializeField] GameObject filamentPrefab; // Filament_increace1이 붙어있는 프리팹

    void Start()
    {
        // Filament 프리팹을 CreateEmpty의 위치에 생성
        GameObject filamentInstance = Instantiate(filamentPrefab, transform.position, Quaternion.identity);

        // 필요한 초기화 작업
        Empty_Filament_Spawn filamentScript = filamentInstance.GetComponent<Empty_Filament_Spawn>();
        if (filamentScript != null)
        {
            filamentScript.SetSpawnTransform(transform); // CreateEmpty의 Transform을 전달
        }
    }
}

