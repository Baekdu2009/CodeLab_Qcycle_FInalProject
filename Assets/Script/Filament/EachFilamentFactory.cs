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
    [Header("설비 스크립트 오브젝트")]
    public FilamentLine[] linemanagers;
    public LevelSensor[] levelSensors;
    public Conveyor conveyor;
    public Shredder shredder;
    public WireCutting wireCutting;
    public ScrewBelt screwBelt;
    public PlasticSpawn[] plasticSpawn;
    public LevelSensorExtruder[] extruder;
    public PressureSensor[] pressureSensor;

    [Header("기타 오브젝트")]
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
    // public GameObject filamentTakeOut;
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
    public bool limiting;

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
    }

    void Update()
    {
        UpdateStatus();
        UpdateStatusUI();
        // TankLevelCheck();
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

        for (int i = 0; i < plasticSpawn.Length; i++)
        {
            TankLevelNoticeUI(tankStatus[i], plasticSpawn[i].isOn);
        }
    }

    private void UpdateStatus()
    {
        // 컨베이어벨트
        conveyorWorkWell = !conveyor.conveyorIsProblem;
        conveyorStop = !conveyor.conveyorRunning;

        // 파쇄기
        shredderWorkWell = !shredder.isProblem;
        shredderStop = !shredder.isRunning;

        // 압출기1
        extruder1WorkWell = !linemanagers[0].isProblem;
        extruder1Stop = !linemanagers[0].isWorking;

        // 커팅기
        wirecuttingWorkWell = !wireCutting.isProblem;
        wirecuttingStop = !wireCutting.isWorking;

        // 스크류벨트
        screwconveyorWorkWell = !screwBelt.isProblem;
        screwconveyorStop = !screwBelt.isWorking;

        // 압출기2
        extruder2WorkWell = !linemanagers[1].isProblem;
        extruder2Stop = !linemanagers[1].isWorking;

        // 롤링
        rollingWorkWell = !linemanagers[1].isProblem;
        rollingStop = !linemanagers[1].isWorking;

        // 스풀러
        spoolerWorkWell = !linemanagers[1].isProblem;
        spoolerStop = !isfilamentOnRotate;
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
            // 초기화 코드
            filamentLineObj = Instantiate(filamentLinePrefab);
            filamentCoverObj = Instantiate(filamentCoverPrefab);

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
            // 회전 코드
            rotSpeed = 200f;
            float rotationThisFrame = rotSpeed * Time.deltaTime;
            filamentObject.transform.Rotate(0, 0, rotationThisFrame);
            currentRotation += rotationThisFrame;

            if (currentRotation >= 360f)
            {
                currentRotation = 0;
                scaleFactor = 0.1f;
                filamentLineObj.transform.localScale += new Vector3(scaleFactor, scaleFactor, 0);

                // 스케일이 최대치에 도달한 경우
                if (filamentLineObj.transform.localScale.x >= 0.96f || filamentLineObj.transform.localScale.y >= 0.96f)
                {
                    rotSpeed = 0;
                    limiting = true; // 스케일 증가가 완료되면 true로 설정
                    FilamentShift(); // 여기서 FilamentShift 호출
                }
            }
        }
    }

    private void NextAction()
    {
        if (linemanagers[1].LastPointArrive())
        {
            // limiting이 false일 때만 true로 변경
            if (!limiting)
            {
                isfilamentOnRotate = true; // 여기서 회전 시작
                HandleFilament(); // HandleFilament 호출
            }
        }
    }

    private IEnumerator WaitAndSetLimitingFalse(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        limiting = false;
    }

    // FilamentShift() 내에서
    public void FilamentShift()
    {
        if (limiting)
        {
            if (Vector3.Distance(filamentObject.transform.position, filamentRotPosition.position) < 0.1f)
            {
                Destroy(filamentObject);
                Instantiate(filamentFullObject);
                filamentFullObject.transform.position = shiftToConveyor.position;

                // Coroutine을 통해 limiting을 false로 변경
                StartCoroutine(WaitAndSetLimitingFalse(2f)); // 2초 후 false로 설정
                isfilamentOnRotate = false;
            }
        }
    }

}
