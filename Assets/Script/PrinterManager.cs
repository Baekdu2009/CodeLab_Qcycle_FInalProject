using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class PrinterManager : MonoBehaviour
{
    [SerializeField] List<PrinterGcode> printers = new List<PrinterGcode>();
    [SerializeField] List<Transform> directPositions = new List<Transform>();
    [SerializeField] TMP_Text printerNum;

    int currentCanvasNum;
    public GameObject directPointerPrefab;
    GameObject directPointer;
    float pointerRotSpeed = 100f;

    void Start()
    {
        printers[0].Canvas.SetActive(true);
        printerNum.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        PointerControl();
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
    private void PointerControl()
    {
        if (directPointer == null)
        {
            directPointer = Instantiate(directPointerPrefab);
            directPointer.transform.position = directPositions[currentCanvasNum].position;
        }
        else
        {
            directPointer.transform.position = directPositions[currentCanvasNum].position;
        }
        directPointer.transform.Rotate(0, 0, pointerRotSpeed * Time.deltaTime);
    }

    public void BtnSelectPanelEvent()
    {
        Destroy(directPointer);
        foreach (var printer in printers)
        {
            printer.Canvas.SetActive(false);
        }
    }
}
