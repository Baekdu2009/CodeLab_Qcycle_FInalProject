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
    /// FilamentFactory List를 정방향으로 순회
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
    /// FilamentFactory List를 역방향으로 순회
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
