using UnityEngine;
using UnityEngine.UI;

public class LocationButtonHandler : MonoBehaviour
{
    public Button[] button; // 여러 버튼을 배열로 관리

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 각 버튼에 클릭 이벤트 핸들러 추가
        foreach(Button button in button)
        {
            button.onClick.AddListener(delegate { HandleButtonClick(button); });
        }
    }

    // 버튼 클릭 시 호출되는 메서드
    private void HandleButtonClick(Button button)
    {
        Debug.Log(button.name +"버튼이 클릭되었습니다.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
