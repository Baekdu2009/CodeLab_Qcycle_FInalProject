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
    public GameObject[] tanks;
    public Transform filamentRotPosition;
    public GameObject filamentCoverPrefab;
    public GameObject filamentLinePrefab;

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
    public Image[] tankStatus;

    // private
    private GameObject filamentObject;
    private GameObject filamentLineObj;
    private GameObject filamentCoverObj;

    float rotSpeed = 200f;
    Vector3 initialScale;
    float currentRotation = 0f;

    // 저장탱크 변수
    bool[] tankLevelbool;

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
        UpdateStatusUI();
        NextAction();

        ArrayLengthSet(ref tankStatus, tanks);
        ArrayLengthSet(ref tankLevelbool, tanks);
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

        for (int i = 0; i < tanks.Length; i++)
        {
            TankLevelNoticeUI(tankStatus[i], tankLevelbool[i]);
        }
    }

    private void StatusNoticeUI(Image image, bool work, bool stop)
    {
        image.color = work ? (stop ? Color.yellow : Color.green) : Color.red;
    }

    private void TankLevelNoticeUI(Image image, bool full)
    {
        image.color = full ? Color.yellow : Color.green;
    }
    private void ArrayLengthSet<T>(ref T[] variableArray, Array baseArray)
    {
        variableArray = new T[baseArray.Length];
    }

    private void TankLevelAction()
    {

    }

    private void HandleFilament()
    {
        if (filamentObject == null)
        {
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
        else
        {
            float rotationThisFrame = rotSpeed * Time.deltaTime;
            filamentObject.transform.Rotate(0, 0, rotationThisFrame);

            currentRotation += rotationThisFrame;

            if (currentRotation >= 360f)
            {
                currentRotation = 0;
                filamentLineObj.transform.localScale += new Vector3(0.02f, 0.02f, 0);
            }

            if (filamentLineObj.transform.localScale.x >= 1 || filamentLineObj.transform.localScale.y >= 1)
            {
                rotSpeed = 0;
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
}
