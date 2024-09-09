using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class PrinterTest : MonoBehaviour
{
    // 움직임 제한 범위 (x, y, z) = (-0.5 ~ 0.5, 0.6 ~ 1.15, -0.4 ~ 0.4)
    public GameObject nozzle;
    Vector3 nozzleOriginPos;
    Vector3 pos;
    public float nozzleSpeed;
    float xRange = 0.5f;
    float yMin = 0.6f;
    float yMax = 1.15f;
    float zRange = 0.4f;

    void Start()
    {
        nozzleOriginPos = nozzle.transform.position;
        pos = nozzle.transform.position;
    }

    void Update()
    {
        Moving(nozzle);
    }

    Vector3 Direction(float x, float y, float z)
    {
        Vector3 to = new Vector3(x, y, z);
        return nozzle.transform.position - to;
    }

    void Moving(GameObject obj)
    {
        Vector3 v = obj.transform.position;

        v.z = nozzleOriginPos.z + zRange * Mathf.Sin(Time.time * nozzleSpeed);

        obj.transform.position = v;


    }
}