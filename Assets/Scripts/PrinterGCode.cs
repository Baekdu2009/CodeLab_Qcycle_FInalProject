using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrinterGcode : MonoBehaviour
{
    public Transform nozzle;    // 노즐
    public Transform rod;       // 로드
    public Transform plate;     // 플레이트

    public float Xmin;
    public float Ymin;
    public float Zmin;
    public float Xmax;
    public float Ymax;
    public float Zmax;

    private Queue<string> gcodeQueue = new Queue<string>();

    private bool isMovingNozzle = false;
    private bool isMovingRod = false;
    private bool isMovingPlate = false;
    public float moveSpeed = 1.0f;  // 이동속도

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
    // G코드에서 G0: 재료공급 없는 빠른 이동, G1: 재료공급 있는 선형 이동
    void GenerateGcode(string gcommand, float x, float y, float z)
    {
        gcodeQueue.Enqueue($"{gcommand} X{x} Y{y} Z{z}");
    }

    void GenerateGcode(string gcommand, Vector3 vector)
    {
        gcodeQueue.Enqueue($"{gcommand} X{vector.x} Y{vector.y} Z{vector.z}");
    }
    void GenerateGcode(string gcommand, Transform transform)
    {
        gcodeQueue.Enqueue($"{gcommand} X{transform.position.x} Y{transform.position.y} Z{transform.position.z}");
    }

    // x축 움직임 -> 플레이트
    // y축 움직임 -> 노즐
    private IEnumerator PlateMoving(string gcode)
    {
        isMovingPlate = true;

        Vector3 targetPosition = ParseGCode(gcode, plate.localPosition);

        while (Vector3.Distance(plate.localPosition, targetPosition) > 0.01f)
        {
            plate.localPosition = Vector3.MoveTowards(plate.localPosition, targetPosition, moveSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        isMovingPlate = false;
    }

    private IEnumerator NozzleMoving(string gcode)
    {
        isMovingNozzle = true;

        Vector3 targetPosition = ParseGCode(gcode, nozzle.localPosition);

        while (Vector3.Distance(nozzle.localPosition, targetPosition) > 0.01f)
        {
            nozzle.localPosition = Vector3.MoveTowards(nozzle.localPosition, targetPosition, moveSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        isMovingNozzle = false;
    }

    private Vector3 ParseGCode(string gcode, Vector3 position)
    {
        string[] parts = gcode.Split(' ');
        float x = position.x;
        float y = position.y;
        float z = position.z;

        foreach (string part in parts)
        {
            if (part.StartsWith("X"))
            {
                x = Mathf.Clamp(float.Parse(part.Substring(1)), Xmin, Xmax);
            }
            else if (part.StartsWith("Y"))
            {
                y = Mathf.Clamp(float.Parse(part.Substring(1)), Ymin, Ymax);
            }
            else if (part.StartsWith("Z"))
            {
                z = Mathf.Clamp(float.Parse(part.Substring(1)), Zmin, Zmax);
            }
        }
        return new Vector3(x, y, z);
    }
}
