using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class FilamentManager : ManagerClass
{
    [SerializeField] List<EachFilamentFactory> filamentFactories = new List<EachFilamentFactory>();

    protected override void Start()
    {
        basicObject = new List<object>(filamentFactories);
        base.Start();
    }

    protected override GameObject GetCanvasProperty(object obj)
    {
        return (obj as EachFilamentFactory)?.Canvas; // EachFilamentFactory�� Canvas �Ӽ��� ������ �ִٰ� ����
    }
}