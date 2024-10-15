using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class AGVManager : ManagerClass
{
    [SerializeField] List<AGVControl> mobility = new List<AGVControl>();
    [SerializeField] List<AGVCart> cart = new List<AGVCart>();
    [SerializeField] GameObject cartCanvas;
    public TMP_Text cartOnOff;
    bool cartCanvasOn = false;

    protected override void Start()
    {
        basicObject = new List<object>(mobility);
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        CartCanvasOnOff();
    }

    protected override GameObject GetCanvasProperty(object obj)
    {
        return (obj as AGVControl).Canvas;
        
    }

    private void CartCanvasOnOff()
    {
        if (cartCanvasOn)
        {
            cartCanvas.SetActive(true);
            cartOnOff.text = "OFF";
        }
        else
        {
            cartCanvas.SetActive(false);
            cartOnOff.text = "ON";
        }
    }

    public void BtnCartCanvas(int i)
    {
        cart[i].isAGVCallOn = !cart[i].isAGVCallOn;
    }

    public void BtnCartCanvasOnOff()
    {
        cartCanvasOn = !cartCanvasOn;
    }
}
