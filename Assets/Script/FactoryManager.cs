using UnityEngine;

public class FactoryManager : MonoBehaviour
{
    public GameObject SelectionPanel;
    public GameObject FilamentFactory;
    public GameObject PrinterFactory;
    public GameObject AGVcontrol;

    void Start()
    {
        SelectionPanel.SetActive(true);
        FilamentFactory.SetActive(false);
        PrinterFactory.SetActive(false);
        AGVcontrol.SetActive(false);
    }

    void Update()
    {

    }

    public void BtnStartingPanel()
    {
        SelectionPanel.SetActive(true);
        FilamentFactory.SetActive(false);
        PrinterFactory.SetActive(false);
        AGVcontrol.SetActive(false);
    }
    public void BtnFilamentFactory()
    {
        SelectionPanel.SetActive(false);
        FilamentFactory.SetActive(true);
        PrinterFactory.SetActive(false);
        AGVcontrol.SetActive(false);
    }
    public void BtnPrinterFactory()
    {
        SelectionPanel.SetActive(false);
        FilamentFactory.SetActive(false);
        PrinterFactory.SetActive(true);
        AGVcontrol.SetActive(false);
    }
    public void BtnAGVControl()
    {
        SelectionPanel.SetActive(false);
        FilamentFactory.SetActive(false);
        PrinterFactory.SetActive(false);
        AGVcontrol.SetActive(true);
    }
}
