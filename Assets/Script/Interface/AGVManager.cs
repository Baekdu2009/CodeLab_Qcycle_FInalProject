using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AGVManager : ManagerClass
{
    [SerializeField] List<AGVControl> mobility = new List<AGVControl>();

    protected override void Start()
    {
        basicObject = new List<object>(mobility);
        base.Start();
    }

    
}
