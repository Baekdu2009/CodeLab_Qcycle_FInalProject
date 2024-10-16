using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class BoxingManager : ManagerClass
{
    [SerializeField] List<BoxingMachine> BoxingMachines = new List<BoxingMachine>();
    [SerializeField] GameObject Canvas;
    [SerializeField] GameObject robotArmCanvas;
    public TMP_Text robotArmOnOff;
    bool robotArmCanvasOn = false;

    protected override void Start()
    {
        basicObject = new List<object>(BoxingMachines);
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        RobotArmCanvas();
    }

    protected override GameObject GetCanvasProperty(object obj)
    {
        return (obj as BoxingManager)?.Canvas;
    }

    public void BtnRobotArmCanvasOnOff()
    {
        robotArmCanvasOn = !robotArmCanvasOn;
    }

    private void RobotArmCanvas()
    {
        if (robotArmCanvasOn)
        {
            robotArmCanvas.SetActive(true);
            robotArmOnOff.text = "OFF";
        }
        else
        {
            robotArmCanvas.SetActive(false);
            robotArmOnOff.text = "ON";
        }
    }
}