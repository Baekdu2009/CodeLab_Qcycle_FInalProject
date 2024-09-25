using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCodeSimulator : MonoBehaviour
{
    public GameObject nozzle; // 노즐 오브젝트
    public GameObject nozzleRod;
    private Vector3 currentPosition;
    private Queue<string> gcodeQueue = new Queue<string>();
    private float moveSpeed = 1.0f; // 이동 속도
    private bool isMoving = false;

    public GameObject filament;
    float rotationSpeed = 200;

    void Start()
    {
        // G코드 생성
        GenerateGCode();
    }

    void Update()
    {
        if (!isMoving && gcodeQueue.Count > 0)
        {
            string gcode = gcodeQueue.Dequeue();
            StartCoroutine(MoveNozzle(gcode));
        }

        RotateFilament();
    }

    public void PositionCheck()
    {
        print(nozzle.transform.position);
    }

    private void GenerateGCode()
    {
        // Z축을 -0.4에서 0.4까지 왕복하며 G코드를 생성
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

        // 노즐을 목표 위치로 부드럽게 이동
        while (Vector3.Distance(nozzle.transform.position, targetPosition) > 0.01f)
        {
            nozzle.transform.position = Vector3.MoveTowards(nozzle.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame(); // 다음 프레임까지 대기
        }

        currentPosition = targetPosition;
        isMoving = false;
    }

    private void RotateFilament()
    {
        // 현재 회전 상태를 가져옴
        Quaternion currentRotation = filament.transform.localRotation;

        // Y축을 기준으로 회전할 각도 계산
        Quaternion deltaRotation = Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);

        // 새로운 회전 상태 계산
        filament.transform.localRotation = currentRotation * deltaRotation;
    }
}
