using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class PrinterManager : MonoBehaviour
{
    [SerializeField] List<PrinterGcode> printers = new List<PrinterGcode>();
    [SerializeField] TMP_Text printerNum;

    int currentCanvasNum;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BtnPrinterOnOff()
    {
        bool isOn = printers[currentCanvasNum].Canvas.activeSelf;

        printers[currentCanvasNum].Canvas.SetActive(!isOn);

        printerNum.text = currentCanvasNum.ToString();
    }
    /// <summary>
    /// Printer List를 정방향으로 순회
    /// </summary>
    public void BtnPrinterNext()
    {
        if (currentCanvasNum < printers.Count - 1)
        {
            foreach (var printer in printers)
            {
                printer.Canvas.SetActive(false);
            }
            printers[++currentCanvasNum].Canvas.SetActive(true);
        }
        else
        {
            currentCanvasNum = 0;
            foreach (var printer in printers)
            {
                printer.Canvas.SetActive(false);
            }
            printers[currentCanvasNum].Canvas.SetActive(true);
        }
        printerNum.text = currentCanvasNum.ToString();
    }
    /// <summary>
    /// Printer List를 역방향으로 순회
    /// </summary>
    public void BtnPrinterBack()
    {
        if (currentCanvasNum > 0 && currentCanvasNum < printers.Count)
        {
            foreach (var printer in printers)
            {
                printer.Canvas.SetActive(false);
            }
            printers[--currentCanvasNum].Canvas.SetActive(true);
        }
        else
        {
            currentCanvasNum = (printers.Count - 1);

            foreach (var printer in printers)
            {
                printer.Canvas.SetActive(false);
            }
            printers[currentCanvasNum].Canvas.SetActive(true);
        }
        printerNum.text = currentCanvasNum.ToString();
    }
}
