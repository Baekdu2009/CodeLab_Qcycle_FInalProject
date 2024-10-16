using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class PrinterManager : ManagerClass
{
    [SerializeField] List<PrinterCode> printers = new List<PrinterCode>();

    protected override void Start()
    {
        basicObject = new List<object>(printers);
        base.Start();
    }

    protected override GameObject GetCanvasProperty(object obj)
    {
        return (obj as PrinterCode)?.Canvas; // PrinterCode�� Canvas �Ӽ��� ������ �ִٰ� ����
    }
}