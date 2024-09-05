using UnityEngine;

public class FilamentMachine : MonoBehaviour
{
    public GameObject filamentRoller;
    float rotSpeed = 200;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RollerRotate();
    }

    void RollerRotate()
    {
        filamentRoller.transform.Rotate(0, 0, -rotSpeed * Time.deltaTime);
    }
}
