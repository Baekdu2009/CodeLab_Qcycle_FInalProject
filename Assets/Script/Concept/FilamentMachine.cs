using UnityEngine;

public class FilamentMachine : MonoBehaviour
{
    public GameObject filamentRoller;
    public GameObject filamentPrefab;
    public Transform spawnFilament; // Unity 에디터에서 설정 가능
    float rotSpeed = 200;
    private GameObject currentFilament; // 현재 생성된 filament 저장
    MeshRenderer filamentMesh;


    void Start()
    {
        if (spawnFilament == null)
        {
            Debug.LogError("spawnFilament가 설정되지 않았습니다. Unity 에디터에서 설정해 주세요.");
        }
        else
        {
            spawnFilament.position = filamentRoller.transform.position; // 초기 위치 설정
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
            if (currentFilament == null) // 기존 filament가 없을 경우에만 생성
            {
                currentFilament = Instantiate(filamentPrefab, spawnFilament.position, Quaternion.Euler(90, 0, 0));
                filamentMesh = currentFilament.GetComponent<MeshRenderer>();
                filamentMesh.material.color = Color.red;
            }
            else
            {
                Debug.LogWarning("이미 filament가 생성되어 있습니다.");
            }
        }
        else
        {
            Debug.LogError("spawnFilament가 null입니다. 필라멘트를 생성할 수 없습니다.");
        }
    }

    public GameObject GetCurrentFilament()
    {
        return currentFilament; // 현재 filament 반환
    }
}
