using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GcodeMoving : MonoBehaviour
{
    public Transform Nozzle;
    public Transform Rod;
    public Transform Plate;

    public float Xmin;
    public float Xmax;
    public float Ymin;
    public float Ymax;
    public float Zmin;
    public float Zmax;

    // private Queue<string> nozzleQueue = new Queue<string> ();
    private Queue<string> plateQueue = new Queue<string> ();
    private Queue<string> rodQueue = new Queue<string> ();

    public float moveSpeed = 1f;
    public float printingResolution = 0.02f;

    private void Start()
    {
        StartCoroutine(NozzleYMovement());
    }
    private void GenerateGcode(string gcommand, float x, float y, float z, Queue<string> queue)
    {
        queue.Enqueue($"{gcommand} X{x} Y{y} Z{z}");
    }

    private IEnumerator NozzleYMovement()
    {
        Queue<string> nozzleQueue = new Queue<string>();
        // Y축 왕복
        for (float y = Ymin; y <= Ymax; y += printingResolution)
        {
            GenerateGcode("G1", 0, y, 0, nozzleQueue);
            yield return MoveNozzle(nozzleQueue.Dequeue());
        }

        for (float y = Ymax; y >= Ymin; y -= printingResolution)
        {
            GenerateGcode("G1", 0, y, 0, nozzleQueue);
            yield return MoveNozzle(nozzleQueue.Dequeue());
        }
    }

    private IEnumerator PlateXMovement()
    {
        if (Plate.localPosition.x >= Xmax - printingResolution)
        {
            GenerateGcode("G1", Xmin, 0, 0, plateQueue);
        }
        else
        {
            GenerateGcode("G1", Plate.localPosition.x + printingResolution, 0, 0, plateQueue);
        }

        yield return MovePlate(plateQueue.Dequeue());
    }

    private IEnumerator MoveNozzle(string gcode)
    {
        Vector3 targetPosition = ParseGCode(gcode, Nozzle.localPosition);

        while (Vector3.Distance(Nozzle.localPosition, targetPosition) > 0.01f)
        {
            Nozzle.localPosition = Vector3.MoveTowards(Nozzle.localPosition, targetPosition, moveSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator MoveRod(string gcode)
    {
        Vector3 targetPosition = ParseGCode(gcode, Rod.localPosition);

        while (Vector3.Distance(Rod.localPosition, targetPosition) > 0.01f)
        {
            Rod.localPosition = Vector3.MoveTowards(Rod.localPosition, targetPosition, moveSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator MovePlate(string gcode)
    {
        Vector3 targetPosition = ParseGCode(gcode, Plate.localPosition);

        while (Vector3.Distance(Plate.localPosition, targetPosition) > 0.01f)
        {
            Plate.localPosition = Vector3.MoveTowards(Plate.localPosition, targetPosition, moveSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    // nozzle, plate, rod별로 G코드 생성을 모두 따로?
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
                x = Mathf.Clamp(float.Parse(part.Substring(1)), -100, 100);
            }
            else if (part.StartsWith("Y"))
            {
                y = Mathf.Clamp(float.Parse(part.Substring(1)), -100, 100);
            }
            else if (part.StartsWith("Z"))
            {
                z = Mathf.Clamp(float.Parse(part.Substring(1)), -100, 100);
            }
        }
        
        return new Vector3(x, y, z);
    }
}
