using UnityEngine;

public class robot : MonoBehaviour
{
    public GameObject box; // 붙일 박스
    public Vector3 foldedPosition; // 접혀있는 상태의 위치
    public Vector3 unfoldedPosition; // 펴진 상태의 위치
    private bool isHolding = false; // 박스를 잡고 있는지 여부

    void Start()
    {
        // 접혀있는 위치와 펴진 위치 설정
        foldedPosition = box.transform.position; // 현재 위치를 접혀있는 위치로 설정
        unfoldedPosition = new Vector3(foldedPosition.x, foldedPosition.y + 1f, foldedPosition.z); // 예시: Y축으로 1만큼 올리기
        box.transform.position = foldedPosition; // 초기 상태를 접혀있는 상태로 설정
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == box && !isHolding)
        {
            // 박스를 그리퍼의 자식으로 설정
            box.transform.SetParent(transform);
            isHolding = true; // 박스를 잡고 있는 상태로 변경

            // 박스를 펴기
            UnfoldBox();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == box && isHolding)
        {
            // 박스의 부모를 해제
            box.transform.SetParent(null);
            isHolding = false; // 박스를 잡고 있지 않은 상태로 변경
        }
    }

    void UnfoldBox()
    {
        // 박스를 펴진 위치로 이동
        box.transform.position = unfoldedPosition;
    }
}
