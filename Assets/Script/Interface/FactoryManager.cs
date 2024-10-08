using UnityEngine;

public class FactoryManager : MonoBehaviour
{
    [SerializeField] GameObject[] panels; // 모든 패널을 배열로 관리
    private int currentPanelIndex;

    void Start()
    {
        currentPanelIndex = 0; // 처음 패널 인덱스
        UpdateUI(); // 초기 UI 업데이트
    }

    private void UpdateUI()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == currentPanelIndex); // 현재 패널만 활성화
        }
    }

    public void BtnStartingPanel()
    {
        currentPanelIndex = 0; // 선택 패널
        UpdateUI(); // UI 업데이트
    }

    public void BtnFilamentFactory()
    {
        currentPanelIndex = 1; // 필라멘트 공장
        UpdateUI(); // UI 업데이트
    }

    public void BtnPrinterFactory()
    {
        currentPanelIndex = 2; // 프린터 공장
        UpdateUI(); // UI 업데이트
    }

    public void BtnBoxingMachine()
    {
        currentPanelIndex = 3; // 박스 기계
        UpdateUI(); // UI 업데이트
    }

    public void BtnAGVControl()
    {
        currentPanelIndex = 4; // AGV 관리
        UpdateUI(); // UI 업데이트
    }
}