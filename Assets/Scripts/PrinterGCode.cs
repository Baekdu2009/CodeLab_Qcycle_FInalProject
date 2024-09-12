using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCodeSimulator : MonoBehaviour
{
    public GameObject nozzle; // ���� ������Ʈ
    public GameObject nozzleRod;
    private Vector3 currentPosition;
    private Queue<string> gcodeQueue = new Queue<string>();
    private float moveSpeed = 1.0f; // �̵� �ӵ�
    private bool isMoving = false;

    void Start()
    {
        // G�ڵ� ����
        GenerateGCode();
    }

    void Update()
    {
        if (!isMoving && gcodeQueue.Count > 0)
        {
            string gcode = gcodeQueue.Dequeue();
            StartCoroutine(MoveNozzle(gcode));
        }
    }

    public void PositionCheck()
    {
        print(nozzle.transform.position);
    }

    private void GenerateGCode()
    {
        // Z���� -0.4���� 0.4���� �պ��ϸ� G�ڵ带 ����
        for (float z = -2.8f; z <= -0.35f; z += 0.01f)
        {
            gcodeQueue.Enqueue($"G1 X0 Y1.75 Z{z}");
        }
        for (float z = -0.35f; z >= -2.8f; z -= 0.01f)
        {
            gcodeQueue.Enqueue($"G1 X0 Y1.75 Z{z}");
        }
    }

    private IEnumerator MoveNozzle(string gcode)
    {
        isMoving = true;

        string[] parts = gcode.Split(' ');
        Vector3 targetPosition = currentPosition;

        foreach (string part in parts)
        {
            if (part.StartsWith("X"))
            {
                targetPosition.x = float.Parse(part.Substring(1));
            }
            else if (part.StartsWith("Y"))
            {
                targetPosition.y = float.Parse(part.Substring(1));
            }
            else if (part.StartsWith("Z"))
            {
                targetPosition.z = float.Parse(part.Substring(1));
            }
        }

        // ������ ��ǥ ��ġ�� �ε巴�� �̵�
        while (Vector3.Distance(nozzle.transform.position, targetPosition) > 0.01f)
        {
            nozzle.transform.position = Vector3.MoveTowards(nozzle.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame(); // ���� �����ӱ��� ���
        }

        currentPosition = targetPosition;
        isMoving = false;
    }
}
