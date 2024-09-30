using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FilamentFactory : MonoBehaviour
{
    public Transform filamentRotPosition;
    public GameObject filamentCover;
    public GameObject filamentLine;
    private GameObject filamentObject;

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
    bool conveyorWorkWell = true;
    bool shredderWorkWell = true;
    bool extruder1WorkWell = true;
    bool wirecuttingWorkWell = true;
    bool screwconveyorWorkWell = true;
    bool extruder2WorkWell = true;
    bool rollingWorkWell = true;
    bool spoolerWorkWell = true;

    // 각 장비 정지 상태 변수
    bool conveyorStop = false;
    bool shredderStop = false;
    bool extruder1Stop = false;
    bool wirecuttingStop = false;
    bool screwconveyorStop = false;
    bool extruder2Stop = false;
    bool rollingStop = false;
    bool spoolerStop = false;

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

    private void StatusCheck(Image image, bool work, bool stop)
    {
        image.color = work ? (stop ? Color.yellow : Color.green) : Color.red;
    }

    private void TankLevelCheck(TMP_Text text, int tankNum, int tankLevel)
    {
        text.text = $"Tank{tankNum} Level {tankLevel:D2}%";
    }

    public void FilamentCreate()
    {
        if (filamentObject == null)
        {
            GameObject line = Instantiate(filamentLine);
            GameObject cover = Instantiate(filamentCover);
            
            filamentObject = new GameObject("FilamentObject");
            line.transform.parent = filamentObject.transform;
            cover.transform.parent = filamentObject.transform;

            filamentObject.transform.position = filamentRotPosition.position;
            filamentObject.transform.rotation = Quaternion.Euler(0, 90, 0);
            initialScale = filamentObject.transform.localScale;
            filamentObject.transform.localScale = new Vector3(0.1f, 1, 0.1f);
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
            filamentObject.transform.localScale += new Vector3(0.3f, 0.1f, 0.3f);
        }

        if (filamentObject.transform.localScale.z >= initialScale.z * 10f)
        {
            print("필라멘트 크기가 10배가 되었음");
            rotSpeed = 0;
        }
    }
}
