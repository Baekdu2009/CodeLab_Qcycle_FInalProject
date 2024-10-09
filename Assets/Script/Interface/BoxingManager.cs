using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class BoxingManager : MonoBehaviour
{
    [SerializeField] List<GameObject> BoxingMachine = new List<GameObject>();
    [SerializeField] List<BoxingRobot> BoxingRobotArm = new List<BoxingRobot>();
    [SerializeField] List<Transform> directPositions = new List<Transform>();
    [SerializeField] TMP_Text erectorNum;

    int currentCanvasNum;
    public GameObject directPointerPrefab;
    GameObject directPointer;
    float pointerRotSpeed = 100f;

    void Start()
    {
        
        erectorNum.text = "0";
    }
}
