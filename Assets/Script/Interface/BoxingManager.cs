using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class BoxingManager : ManagerClass
{
    [SerializeField] List<BoxingMachine> BoxingMachines = new List<BoxingMachine>();

    protected override void Start()
    {
        basicObject = new List<object>(BoxingMachines);
        base.Start();
    }

    protected override GameObject GetCanvasProperty(object obj)
    {
        return (obj as BoxingMachine)?.Canvas;
    }
}