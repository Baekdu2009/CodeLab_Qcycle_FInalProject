using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ManagerClass : MonoBehaviour
{
    [SerializeField] protected List<object> basicObject = new List<object>();
    [SerializeField] protected List<Transform> directPositions = new List<Transform>();
    [SerializeField] protected TMP_Text objectNumText;

    protected int currentCanvasNum = 0;
    public GameObject directPointerPrefab;
    protected GameObject directPointer;
    public float pointerRotSpeed = 100f;

    protected virtual void Start()
    {
        if (basicObject.Count > 0)
        {
            SetActiveCanvas(0);
            UpdateObjectNumText();
        }
    }

    protected virtual void Update()
    {
        PointerControl();
    }

    protected void PointerControl()
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

    protected void SetActiveCanvas(int index)
    {
        foreach (var obj in basicObject)
        {
            // 각 객체의 Canvas 속성을 가져오기
            var canvasProperty = GetCanvasProperty(obj);
            if (canvasProperty != null)
            {
                canvasProperty.SetActive(false);
            }
        }

        var activeCanvas = GetCanvasProperty(basicObject[index]);
        if (activeCanvas != null)
        {
            activeCanvas.SetActive(true);
        }

        currentCanvasNum = index;
        UpdateObjectNumText();
    }

    protected void UpdateObjectNumText()
    {
        objectNumText.text = currentCanvasNum.ToString();
    }

    protected virtual GameObject GetCanvasProperty(object obj)
    {
        // 이 메서드는 각 객체에 대한 올바른 캔버스를 반환하도록 구현해야 합니다.
        // 이는 플레이스홀더이며 실제 로직으로 교체해야 합니다.
        return null;
    }

    public void BtnNext()
    {
        if (currentCanvasNum < basicObject.Count - 1)
        {
            SetActiveCanvas(currentCanvasNum + 1);
        }
        else
        {
            SetActiveCanvas(0);
        }
    }

    public void BtnBack()
    {
        if (currentCanvasNum > 0)
        {
            SetActiveCanvas(currentCanvasNum - 1);
        }
        else
        {
            SetActiveCanvas(basicObject.Count - 1);
        }
    }

    public void BtnSelectPanelEvent()
    {
        Destroy(directPointer);
        foreach (var obj in basicObject)
        {
            var canvasProperty = GetCanvasProperty(obj);
            if (canvasProperty != null)
            {
                canvasProperty.SetActive(false);
            }
        }
    }
}