using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class FilamentManager : MonoBehaviour
{
    [SerializeField] List<FilamentFactory> filamentFactories = new List<FilamentFactory>();
    [SerializeField] TMP_Text factoryNum;

    int currentCanvasNum;

    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void BtnFactoryOnOff()
    {
        bool isOn = filamentFactories[currentCanvasNum].Canvas.activeSelf;

        filamentFactories[currentCanvasNum].Canvas.SetActive(!isOn);

        factoryNum.text = currentCanvasNum.ToString();

        if (!isOn) currentCanvasNum = 0;
    }
    /// <summary>
    /// FilamentFactory List�� ���������� ��ȸ
    /// </summary>
    public void BtnFactoryNext()
    {
        if (currentCanvasNum < filamentFactories.Count - 1)
        {
            foreach (var factory in filamentFactories)
            {
                factory.Canvas.SetActive(false);
            }
            filamentFactories[++currentCanvasNum].Canvas.SetActive(true);
        }
        else
        {
            currentCanvasNum = 0;
            foreach (var factory in filamentFactories)
            {
                factory.Canvas.SetActive(false);
            }
            filamentFactories[currentCanvasNum].Canvas.SetActive(true);
        }
        factoryNum.text = currentCanvasNum.ToString();
    }
    /// <summary>
    /// FilamentFactory List�� ���������� ��ȸ
    /// </summary>
    public void BtnFactoryBack()
    {
        if (currentCanvasNum > 0 && currentCanvasNum < filamentFactories.Count)
        {
            foreach (var factory in filamentFactories)
            {
                factory.Canvas.SetActive(false);
            }
            filamentFactories[--currentCanvasNum].Canvas.SetActive(true);
        }
        else
        {
            currentCanvasNum = (filamentFactories.Count - 1);

            foreach (var factory in filamentFactories)
            {
                factory.Canvas.SetActive(false);
            }
            filamentFactories[currentCanvasNum].Canvas.SetActive(true);
        }
        factoryNum.text = currentCanvasNum.ToString();
    }
}
