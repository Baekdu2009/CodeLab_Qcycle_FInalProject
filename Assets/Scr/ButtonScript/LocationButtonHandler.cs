using UnityEngine;
using UnityEngine.UI;

public class LocationButtonHandler : MonoBehaviour
{
    public Button[] button; // ���� ��ư�� �迭�� ����

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �� ��ư�� Ŭ�� �̺�Ʈ �ڵ鷯 �߰�
        foreach(Button button in button)
        {
            button.onClick.AddListener(delegate { HandleButtonClick(button); });
        }
    }

    // ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    private void HandleButtonClick(Button button)
    {
        Debug.Log(button.name +"��ư�� Ŭ���Ǿ����ϴ�.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
