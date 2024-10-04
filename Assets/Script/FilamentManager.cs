using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class FilamentManager : MonoBehaviour
{
    [SerializeField] List<FilamentFactoryUI> filamentFactories = new List<FilamentFactoryUI>();
    [SerializeField] List<Transform> directPositions = new List<Transform>();
    [SerializeField] TMP_Text factoryNum;

    int currentCanvasNum;
    public GameObject directPointerPrefab;
    GameObject directPointer;
    float pointerRotSpeed = 100f;


    void Start()
    {
        filamentFactories[0].Canvas.SetActive(true);
        factoryNum.text = "0";
    }


    void Update()
    {
        PointerControl();
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
        foreach (var factory in filamentFactories)
        {
            factory.Canvas.SetActive(false);
        }
        currentCanvasNum = 0;
        factoryNum.text = "No.";
    }
}