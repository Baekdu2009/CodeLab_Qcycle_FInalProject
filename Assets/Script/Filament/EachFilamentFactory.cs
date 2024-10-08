using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EachFilamentFactory : MonoBehaviour
{
    [Header("제어 오브젝트")]
    public List<FilamentLine> linemanagers = new List<FilamentLine>();
    public LevelSensor[] levelSensors;
    // public GameObject[] tanks;
    public Transform filamentRotPosition;
    public GameObject filamentCoverPrefab;
    public GameObject filamentLinePrefab;
    public GameObject filamentFullObject;
    public Transform shiftToConveyor;

    [Header("상태표시UI")]
    public GameObject Canvas;
    public Image conveyorStatus;
    public Image shredderStatus;
    public Image extruder1Status;
    public Image wirecuttingStatus;
    public Image screwconveyorStatus;
    public Image extruder2Status;
    public Image rollingStatus;
    public Image spoolerStatus;
    public GameObject filamentTakeOut;
    public Image[] tankStatus;

    // private
    private GameObject filamentObject;
    private GameObject fullFilament;
    private GameObject filamentLineObj;
    private GameObject filamentCoverObj;

    float rotSpeed = 200f;
    Vector3 initialScale;
    float currentRotation = 0f;
    float scaleFactor;

    bool isfilamentOnRotate;

    Conveyor conveyor;
    WireCutting WireCutting;

    // 저장탱크 변수
    // public bool[] tankLevelbool;

    // 각 장비 상태 변수
    bool conveyorWorkWell = false;
    bool shredderWorkWell = false;
    bool extruder1WorkWell = false;
    bool wirecuttingWorkWell = false;
    bool screwconveyorWorkWell = false;
    bool extruder2WorkWell = false;
    bool rollingWorkWell = false;
    bool spoolerWorkWell = false;

    // 각 장비 정지 상태 변수
    bool conveyorStop = false;
    bool shredderStop = false;
    bool extruder1Stop = false;
    bool wirecuttingStop = false;
    bool screwconveyorStop = false;
    bool extruder2Stop = false;
    bool rollingStop = false;
    bool spoolerStop = false;

    void Start()
    {
        filamentObject = null;
        conveyor = FindAnyObjectByType<Conveyor>();
        WireCutting = FindAnyObjectByType<WireCutting>();
    }

    void Update()
    {
        UpdateStatus();
        UpdateStatusUI();
        TankLevelCheck();
        NextAction();
    }

    private void UpdateStatusUI()
    {
        StatusNoticeUI(conveyorStatus, conveyorWorkWell, conveyorStop);
        StatusNoticeUI(shredderStatus, shredderWorkWell, shredderStop);
        StatusNoticeUI(extruder1Status, extruder1WorkWell, extruder1Stop);
        StatusNoticeUI(wirecuttingStatus, wirecuttingWorkWell, wirecuttingStop);
        StatusNoticeUI(screwconveyorStatus, screwconveyorWorkWell, screwconveyorStop);
        StatusNoticeUI(extruder2Status, extruder2WorkWell, extruder2Stop);
        StatusNoticeUI(rollingStatus, rollingWorkWell, rollingStop);
        StatusNoticeUI(spoolerStatus, spoolerWorkWell, spoolerStop);

        for (int i = 0; i < levelSensors.Length; i++)
        {
            TankLevelNoticeUI(tankStatus[i], levelSensors[i]);
        }
    }

    private void UpdateStatus()
    {
        // 컨베이어벨트
        conveyorWorkWell = !conveyor.conveyorIsProblem;
        conveyorStop = !conveyor.conveyorRunning;

        // 파쇄기
        shredderWorkWell = !conveyor.shredderIsProblem;
        shredderStop = !conveyor.shredderRunning;

        // 압출기1
        extruder1WorkWell = !linemanagers[0].isOn;
        extruder1Stop = !linemanagers[0].isProblem;

        // 커팅기
        wirecuttingWorkWell = !WireCutting.isProblem;
        wirecuttingStop = !WireCutting.isWorking;
    }

    private void StatusNoticeUI(Image image, bool work, bool stop)
    {
        image.color = work ? (stop ? Color.yellow : Color.green) : Color.red;
    }

    private void TankLevelNoticeUI(Image image, bool check)
    {
        image.color = check ? Color.yellow : Color.green;
    }
    private void ArrayLengthSet<T>(ref T[] variableArray, Array baseArray)
    {
        variableArray = new T[baseArray.Length];
    }

    private void TankLevelCheck()
    {
        for (int i = 0; i < levelSensors.Length; i++)
        {
            if (!linemanagers[i].isOn && levelSensors[i].isDetected)
            {
                linemanagers[i].isOn = levelSensors[i].isDetected;
            }
        }
    }

    private void HandleFilament()
    {
        if (filamentObject == null)
        {
            filamentTakeOut.SetActive(false);
            filamentLineObj = Instantiate(filamentLinePrefab);
            filamentCoverObj = Instantiate(filamentCoverPrefab);
            isfilamentOnRotate = true;
            
            filamentObject = new GameObject("FilamentObject");
            filamentLineObj.transform.parent = filamentObject.transform;
            filamentCoverObj.transform.parent = filamentObject.transform;

            filamentObject.transform.position = filamentRotPosition.position;
            filamentObject.transform.rotation = Quaternion.Euler(0, 90, 0);
            filamentCoverObj.transform.localScale = new Vector3(1, 1, 1);
            filamentLineObj.transform.localScale = new Vector3(0.6f, 0.6f, 1);
            initialScale = filamentLineObj.transform.localScale;
        }
        else if (filamentObject != null && isfilamentOnRotate)
        {
            rotSpeed = 200f;
            float rotationThisFrame = rotSpeed * Time.deltaTime;
            filamentObject.transform.Rotate(0, 0, rotationThisFrame);

            currentRotation += rotationThisFrame;

            if (currentRotation >= 360f)
            {
                currentRotation = 0;
                scaleFactor = 0.1f;
                filamentLineObj.transform.localScale += new Vector3(scaleFactor, scaleFactor, 0);
            }

            if (filamentLineObj.transform.localScale.x >= 0.96f || filamentLineObj.transform.localScale.y >= 0.96f)
            {
                rotSpeed = 0;
                filamentTakeOut.SetActive(true);

                isfilamentOnRotate = false;
            }
        }
    }

    private void NextAction()
    {
        if (linemanagers[1].LastPointArrive())
        {
            HandleFilament();
        }
    }

    public void BtnFilamentShift()
    {
        if (Vector3.Distance(filamentObject.transform.position, filamentRotPosition.position) < 0.1f)
        {
            Destroy(filamentObject);
            Instantiate(filamentFullObject);
            filamentFullObject.transform.position = shiftToConveyor.position;
            // filamentFullObject.transform.rotation = Quaternion.Euler(0, 0, 0);

            isfilamentOnRotate = true;
        }
        else
            return;
    }
}
