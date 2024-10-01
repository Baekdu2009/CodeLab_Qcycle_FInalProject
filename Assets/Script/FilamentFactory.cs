using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class FilamentFactory : MonoBehaviour
{
    public Transform filamentRotPosition;
    public GameObject filamentCoverPrefab;
    public GameObject filamentLinePrefab;
    private GameObject filamentObject;
    private GameObject filamentLineObj;
    private GameObject filamentCoverObj;

    public GameObject Canvas;
    public Image conveyorStatus;
    public Image shredderStatus;
    public Image extruder1Status;
    public Image wirecuttingStatus;
    public Image screwconveyorStatus;
    public Image extruder2Status;
    public Image rollingStatus;
    public Image spoolerStatus;
    public TMP_Text tank1Text;
    public TMP_Text tank2Text;

    float rotSpeed = 200f;
    Vector3 initialScale;
    float currentRotation = 0f;

    // 각 장비 상태 변수
   public bool conveyorWorkWell = false;
    public bool shredderWorkWell = false;
    public bool extruder1WorkWell = false;
    public bool wirecuttingWorkWell = false;
    public bool screwconveyorWorkWell = false;
    public bool extruder2WorkWell = false;
    public bool rollingWorkWell = false;
    public bool spoolerWorkWell = false;

    // 각 장비 정지 상태 변수
    public bool conveyorStop = false;
    public bool shredderStop = false;
    public bool extruder1Stop = false;
    public bool wirecuttingStop = false;
    public bool screwconveyorStop = false;
    public bool extruder2Stop = false;
    public bool rollingStop = false;
    public bool spoolerStop = false;

    void Start() { }

    void Update()
    {
        UpdateStatus();
        UpdateTankLevels();
        HandleFilament();
    }

    private void UpdateStatus()
    {
        StatusCheck(conveyorStatus, conveyorWorkWell, conveyorStop);
        StatusCheck(shredderStatus, shredderWorkWell, shredderStop);
        StatusCheck(extruder1Status, extruder1WorkWell, extruder1Stop);
        StatusCheck(wirecuttingStatus, wirecuttingWorkWell, wirecuttingStop);
        StatusCheck(screwconveyorStatus, screwconveyorWorkWell, screwconveyorStop);
        StatusCheck(extruder2Status, extruder2WorkWell, extruder2Stop);
        StatusCheck(rollingStatus, rollingWorkWell, rollingStop);
        StatusCheck(spoolerStatus, spoolerWorkWell, spoolerStop);
    }

    private void UpdateTankLevels()
    {
        TankLevelCheck(tank1Text, 1, 10);
        TankLevelCheck(tank2Text, 2, 20);
    }

    public void StatusCheck(Image image, bool work, bool stop)
    {
        image.color = work ? (stop ? Color.yellow : Color.green) : Color.red;
    }
    public void StatusCheck(Image image, int work, int stop)
    {
        image.color = work == 1 ? (stop == 1 ? Color.yellow : Color.green) : Color.red;
    }

    private void TankLevelCheck(TMP_Text text, int tankNum, int tankLevel)
    {
        text.text = $"Tank{tankNum} Level {tankLevel:D2}%";
    }

    public void FilamentCreate()
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
            filamentLineObj.transform.localScale = new Vector3(1, 1, 0.1f);
            initialScale = filamentLineObj.transform.localScale;
        }
    }

    private void HandleFilament()
    {
        if (filamentObject != null)
        {
            FilamentScale();
        }
    }

    private void FilamentScale()
    {
        float rotationThisFrame = rotSpeed * Time.deltaTime;
        filamentObject.transform.Rotate(0, 0, rotationThisFrame);

        currentRotation += rotationThisFrame;

        if (currentRotation >= 360f)
        {
            currentRotation = 0;
            filamentLineObj.transform.localScale += new Vector3(0, 0, 0.1f);
        }

        if (filamentLineObj.transform.localScale.z >= 1)
        {
            rotSpeed = 0;
        }
    }
}
