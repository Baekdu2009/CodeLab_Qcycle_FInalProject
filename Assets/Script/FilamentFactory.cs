using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FilamentFactory : MonoBehaviour
{
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

    public bool conveyorWorkWell;
    public bool shredderWorkWell;
    public bool extruder1WorkWell;
    public bool wirecuttingWorkWell;
    public bool screwconveyorWorkWell;
    public bool extruder2WorkWell;
    public bool rollingWorkWell;
    public bool spoolerWorkWell;

    public bool conveyorStop;
    public bool shredderStop;
    public bool extruder1Stop;
    public bool wirecuttingStop;
    public bool screwconveyorStop;
    public bool extruder2Stop;
    public bool rollingStop;
    public bool spoolerStop;
    
    void Start()
    {
        
    }

    void Update()
    {
        StatusCheck(conveyorStatus, conveyorWorkWell, conveyorStop);
        StatusCheck(shredderStatus, shredderWorkWell, shredderStop);
        StatusCheck(extruder1Status, extruder1WorkWell, extruder1Stop);
        StatusCheck(wirecuttingStatus, wirecuttingWorkWell, wirecuttingStop);
        StatusCheck(screwconveyorStatus, screwconveyorWorkWell, screwconveyorStop);
        StatusCheck(extruder2Status, extruder2WorkWell, extruder2Stop);
        StatusCheck(rollingStatus, rollingWorkWell, rollingStop);
        StatusCheck(spoolerStatus, spoolerWorkWell, spoolerStop);
        TankLevelCheck(tank1Text, 1, 10);
        TankLevelCheck(tank2Text, 2, 20);
    }
    private void StatusCheck(Image image, bool work, bool stop)
    {
        if (work)
        {
            if (!stop)
            {
                image.color = Color.green;
            }
            else
            {
                
                image.color = Color.yellow;
            }
        }
        else
        {
            image.color = Color.red;
        }
    }

    private void TankLevelCheck(TMP_Text text, int tankNum, int tankLevel)
    {
        text.text = $"Tank{tankNum} Level {tankLevel:D2}%";
    }
}
