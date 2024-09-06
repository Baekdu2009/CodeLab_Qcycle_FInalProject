using UnityEngine;

public class Printer : MonoBehaviour
{
    public Transform filamentLocation; // 필라멘트 위치
    GameObject filamentObject; // 현재 필라멘트 객체
    Road road; // Road 참조
    float rotSpeed = 200;
    public GameObject itemPrefab;
    GameObject examItem;
    public Transform itemSpawnLocation;
    public Transform dropLocation;
    public float dropSpeed = 2;


    void Start()
    {
        road = FindAnyObjectByType<Road>(); // Road 타입의 객체를 찾음
    }

    void Update()
    {
        
    }

    public void OnFilamentLocationBtn()
    {
        filamentObject = null;

        if (road != null && filamentObject == null)
        {
            filamentObject = road.GetCurrentFilament(); // Road에서 filament 객체 가져오기
            print(filamentObject);

            if (filamentObject != null) // filament가 존재하는지 확인
            {
                filamentObject.transform.position = filamentLocation.position; // 위치 설정
                filamentObject.transform.rotation = Quaternion.Euler(90, 0, 0); // 회전 설정
            }
            else
            {
                Debug.LogError("현재 filament가 없습니다.");
            }
        }
        else
        {
            Debug.LogError("Road 객체를 찾을 수 없습니다.");
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
