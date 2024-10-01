using UnityEngine;

public class FactoryManager : MonoBehaviour
{
    [SerializeField] GameObject SelectionPanel;
    [SerializeField] GameObject FilamentFactory;
    [SerializeField] GameObject PrinterFactory;

    private bool selectionIsOn;
    private bool filamentFactIsOn;
    private bool printerFactIsOn;

    void Start()
    {
        selectionIsOn = true;
        filamentFactIsOn = false;
        printerFactIsOn = false;

        // 초기 UI 업데이트
        UpdateUI();
    }

    private void UpdateUI()
    {
        SelectionPanel.SetActive(selectionIsOn);
        FilamentFactory.SetActive(filamentFactIsOn);
        PrinterFactory.SetActive(printerFactIsOn);
    }

    public void BtnStartingPanel()
    {
        selectionIsOn = true;
        filamentFactIsOn = false;
        printerFactIsOn = false;
        UpdateUI(); // UI 업데이트
    }

    public void BtnFilamentFactory()
    {
        selectionIsOn = false;
        filamentFactIsOn = true;
        printerFactIsOn = false;
        UpdateUI(); // UI 업데이트
    }

    public void BtnPrinterFactory()
    {
        selectionIsOn = false;
        filamentFactIsOn = false;
        printerFactIsOn = true;
        UpdateUI(); // UI 업데이트
    }

    public void BtnAGVControl()
    {
        selectionIsOn = false;
        filamentFactIsOn = false;
        printerFactIsOn = false;
        UpdateUI(); // UI 업데이트
    }
}
