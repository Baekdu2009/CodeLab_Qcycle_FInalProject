using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinterGCodeTraining : MonoBehaviour
{
    public Transform nozzle; // 노즐
    public Transform rod;    // 로드
    public Transform plate;  // 플레이트

    public float Xmin;
    public float Xmax;
    public float Ymin;
    public float Ymax;
    public float Zmin;
    public float Zmax;

    private Queue<string> gcodeQueueNozzle = new Queue<string>();
    private Queue<string> gcodeQueueRod = new Queue<string>();
    private Queue<string> gcodeQueuePlate = new Queue<string>();
    private bool isMovingNozzle = false;
    private bool isMovingRod = false;
    private bool isMovingPlate = false;
    public float moveSpeed = 1.0f; // 이동 속도

    private void Start()
    {
        GenerateGCode();
    }

    private void Update()
    {
        if (!isMovingNozzle && gcodeQueueNozzle.Count > 0)
        {
            string gcode = gcodeQueueNozzle.Dequeue();
            StartCoroutine(MoveNozzle(gcode));
        }

        if (!isMovingRod && gcodeQueueRod.Count > 0)
        {
            string gcode = gcodeQueueRod.Dequeue();
            StartCoroutine(MoveRod(gcode));
        }

        if (!isMovingPlate && gcodeQueuePlate.Count > 0)
        {
            string gcode = gcodeQueuePlate.Dequeue();
            StartCoroutine(MovePlate(gcode));
        }
    }

    private void GenerateGCode()
    {
        // Y축을 Ymin에서 Ymax까지 왕복하며 G코드를 생성 -> nozzle

        for (float y = Ymin; y <= Ymax; y += 0.01f)
        {
            gcodeQueueNozzle.Enqueue($"G1 X{0} Y{y} Z{0}");
        }
        for (float y = Ymax; y >= Ymin; y -= 0.01f)
        {
            gcodeQueueNozzle.Enqueue($"G1 X{0} Y{y} Z{0}");
        }

        // Z축을 Zmin에서 Zmax까지 왕복하며 G코드를 생성 -> rod
        for (float z = Zmin; z <= Zmax; z += 0.01f)
        {
            gcodeQueueRod.Enqueue($"G1 X{0} Y{0} Z{z}");
        }
        for (float z = Zmax; z >= Zmin; z -= 0.01f)
        {
            gcodeQueueRod.Enqueue($"G1 X{0} Y{0} Z{z}");
        }

        // X축을 Xmin에서 Xmax까지 왕복하며 G코드를 생성 -> plate
        for (float x = Xmin; x <= Xmax; x += 0.01f)
        {
            gcodeQueuePlate.Enqueue($"G1 X{x} Y{0} Z{0}");
        }
        for (float x = Xmax; x >= Xmin; x -= 0.01f)
        {
            gcodeQueuePlate.Enqueue($"G1 X{x} Y{0} Z{0}");
        }
    }

    //private IEnumerator NozzlePlateMoving()
    //{
    //    isMovingPlate = true;

    //    for (float x = Xmin; x <= Xmax; x += 0.01f)
    //    {

    //    }
    //}

    private IEnumerator MoveNozzle(string gcode)
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

    private IEnumerator MoveRod(string gcode)
    {
        isMovingRod = true;

        Vector3 targetPosition = ParseGCode(gcode, rod.localPosition);

        while (Vector3.Distance(rod.localPosition, targetPosition) > 0.01f)
        {
            rod.localPosition = Vector3.MoveTowards(rod.localPosition, targetPosition, moveSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        // 로드가 이동한 후 노즐의 Z축 위치를 로드의 위치와 동일하게 설정
        nozzle.localPosition = new Vector3(nozzle.localPosition.x, nozzle.localPosition.y, targetPosition.z);

        isMovingRod = false;
    }

    private IEnumerator MovePlate(string gcode)
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

    private Vector3 ParseGCode(string gcode, Vector3 currentPosition)
    {
        string[] parts = gcode.Split(' ');
        float x = currentPosition.x;
        float y = currentPosition.y;
        float z = currentPosition.z;

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
