using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrinterGcode : MonoBehaviour
{
    public Transform nozzle;    // ����
    public Transform rod;       // �ε�
    public Transform plate;     // �÷���Ʈ

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
    public float moveSpeed = 1.0f;  // �̵��ӵ�

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
    // G�ڵ忡�� G0: ������ ���� ���� �̵�, G1: ������ �ִ� ���� �̵�
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

    // x�� ������ -> �÷���Ʈ
    // y�� ������ -> ����
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
