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
            filamentObject = road.GetCurrentFilament(); // Road에서 현재 filament 가져오기

            if (filamentObject != null) // filament가 존재하는지 확인
            {
                filamentObject.transform.position = filamentLocation.position; // 위치 설정
                filamentObject.transform.rotation = Quaternion.Euler(90, 0, 0);
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
}
