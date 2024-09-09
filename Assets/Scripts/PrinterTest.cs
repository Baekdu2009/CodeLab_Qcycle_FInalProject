using System.Collections;
using UnityEngine;

public class PrinterTest : MonoBehaviour
{
    // 움직임 제한 범위 (x, y, z) = (-0.5 ~ 0.5, 0.6 ~ 1.15, -0.4 ~ 0.4)
    public GameObject nozzle;
    Transform nozzleOriginPos;
    public float nozzleSpeed = 2f;
    [Range(-0.5f, 0.5f)]
    float xPosRange;

    void Start()
    {
        nozzleOriginPos = nozzle.transform;
    }


    void Update()
    {

    }

    Vector3 Direction(float x, float y, float z)
    {
        Vector3 to = new Vector3(x, y, z);
        return nozzle.transform.position - to;
    }

    IEnumerator NozzleXposMoving()
    {
        float xPos = nozzle.transform.position.x;
        
        
        if (xPos < xPosRange)
        {

        }

        yield return null;
    }
}
