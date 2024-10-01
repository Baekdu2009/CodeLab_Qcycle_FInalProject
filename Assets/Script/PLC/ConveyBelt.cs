using UnityEngine;

public class ConveyBelt : MonoBehaviour
{
    [SerializeField] float speed;
    float currentTime = 0;
    bool isConveyorMoving = false;

    [SerializeField] Slider[] sliders;


    public void OnStartConveyorBtnClkEvent()
    {
        foreach (var slider in sliders)
        {

            slider.GoRight();
        }


    }
}
