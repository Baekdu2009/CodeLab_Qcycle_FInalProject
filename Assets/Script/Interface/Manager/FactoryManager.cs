using UnityEngine;

public class FactoryManager : MonoBehaviour
{
    [SerializeField] GameObject[] panels; // ��� �г��� �迭�� ����
    private int currentPanelIndex;

    void Start()
    {
        currentPanelIndex = 0; // ó�� �г� �ε���
        UpdateUI(); // �ʱ� UI ������Ʈ
    }

    private void UpdateUI()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == currentPanelIndex); // ���� �гθ� Ȱ��ȭ
        }
    }

    public void BtnStartingPanel()
    {
        currentPanelIndex = 0; // ���� �г�
        UpdateUI(); // UI ������Ʈ
    }

    public void BtnFilamentFactory()
    {
        currentPanelIndex = 1; // �ʶ��Ʈ ����
        UpdateUI(); // UI ������Ʈ
    }

    public void BtnPrinterFactory()
    {
        currentPanelIndex = 2; // ������ ����
        UpdateUI(); // UI ������Ʈ
    }

    public void BtnBoxingMachine()
    {
        currentPanelIndex = 3; // �ڽ� ���
        UpdateUI(); // UI ������Ʈ
    }

    public void BtnAGVControl()
    {
        currentPanelIndex = 4; // AGV ����
        UpdateUI(); // UI ������Ʈ
    }
}