using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrinterGcode : MonoBehaviour
{
    public Transform nozzle;    // 노즐
    public Transform rod;       // 로드
    public Transform plate;     // 플레이트
    public GameObject filament; // 필라멘트
    public GameObject cubePrefab; // 큐브 프리팹
    public Transform objPos;

    public float Xmin;
    public float Ymin;
    public float Zmin;
    public float Xmax;
    public float Ymax;
    public float Zmax;

    private Queue<string> nozzleQueue = new Queue<string>();
    private Queue<string> plateQueue = new Queue<string>();
    private Queue<string> rodQueue = new Queue<string>();

    public float moveSpeed = 0.1f;  // 이동속도
    public float printingResolution = 0.02f;
    public float rotSpeed = 200;

    private GameObject currentCube; // 현재 큐브 오브젝트
    private Coroutine originCoroutine; // 클래스 필드로 선언


    public void OriginBtnEvent()
    {
        if (originCoroutine == null)
        {
            originCoroutine = StartCoroutine(OriginPosition());
        }
        else
        {
            StopCoroutine(originCoroutine);
            originCoroutine = null;
        }
    }


    private IEnumerator OriginPosition()
    {
        GenerateGcode("G0", 0, Ymin, 0, nozzleQueue);
        GenerateGcode("G0", 0, 0, Zmin, rodQueue);
        GenerateGcode("G0", Xmin, 0, 0, plateQueue);

        yield return MoveNozzle(nozzleQueue.Dequeue());
        yield return MoveRod(rodQueue.Dequeue());
        yield return MovePlate(plateQueue.Dequeue());
    }

    public void StartProcess()
    {
        StartCoroutine(PrintProcess());
        StartCoroutine(RotateFilament());
    }

    public void StopProcess()
    {
        StopAllCoroutines();
    }

    private IEnumerator PrintProcess()
    {
        while (true) // 무한 루프
        {
            // 노즐 Y축으로 왕복 운동
            yield return StartCoroutine(NozzleMovement());

            // 플레이트 X축으로 이동
            yield return StartCoroutine(PlateRodMovement());

            // 로드 Z축으로 이동
            // yield return StartCoroutine(RodZMovement());
        }
    }

    private IEnumerator NozzleMovement()
    {
        // 큐브가 없으면 추가
        if (currentCube == null)
        {
            // 노즐의 X축과 동일한 위치에 큐브 생성
            Vector3 cubePosition = new Vector3(nozzle.position.x, objPos.position.y, objPos.position.z);
            currentCube = Instantiate(cubePrefab, cubePosition, Quaternion.identity);
            currentCube.transform.localScale = new Vector3(0, currentCube.transform.localScale.y, currentCube.transform.localScale.z); // 초기 스케일을 0으로 설정
        }

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
        // 큐브의 스케일을 조정하여 출력하는 모습 만들기
        Vector3 newScale = currentCube.transform.localScale;
        newScale.x += printingResolution; // X축 방향으로 추가
        currentCube.transform.localScale = newScale;

        // 큐브 위치를 플레이트의 X축을 따라 이동
        Vector3 newPosition = currentCube.transform.localPosition;
        newPosition.x -= printingResolution / 2; // half of the printing resolution
        currentCube.transform.localPosition = newPosition;
    }

    private IEnumerator PlateRodMovement()
    {
        if (plate.localPosition.x >= Xmax - printingResolution)
        {
            GenerateGcode("G1", Xmin + printingResolution, 0, 0, plateQueue);
            GenerateGcode("G1", 0, 0, rod.localPosition.z + printingResolution, rodQueue);
        }
        else
        {
            GenerateGcode("G1", plate.localPosition.x + printingResolution, 0, 0, plateQueue);
            GenerateGcode("G1", 0, 0, rod.localPosition.z, rodQueue);
        }

        yield return MovePlate(plateQueue.Dequeue());
        yield return MoveRod(rodQueue.Dequeue());
    }

    //private IEnumerator RodZMovement()
    //{
    //    if (plate.localPosition.x >= Xmax - printingResolution)
    //    {
    //        GenerateGcode("G1", 0, 0, rod.localPosition.z + printingResolution, rodQueue);
    //    }
    //    else
    //    {
    //        GenerateGcode("G1", 0, 0, rod.localPosition.z, rodQueue);
    //    }

    //    yield return MoveRod(rodQueue.Dequeue());
    //}

    private void GenerateGcode(string gcommand, float x, float y, float z, Queue<string> queue)
    {
        queue.Enqueue($"{gcommand} X{x} Y{y} Z{z}");
    }

    private IEnumerator MoveNozzle(string gcode)
    {
        Vector3 targetPosition = ParseGCode(gcode, nozzle.localPosition);

        while (Vector3.Distance(nozzle.localPosition, targetPosition) > 0.01f)
        {
            nozzle.localPosition = Vector3.MoveTowards(nozzle.localPosition, targetPosition, moveSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator MovePlate(string gcode)
    {
        Vector3 targetPosition = ParseGCode(gcode, plate.localPosition);

        while (Vector3.Distance(plate.localPosition, targetPosition) > 0.01f)
        {
            plate.localPosition = Vector3.MoveTowards(plate.localPosition, targetPosition, moveSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator MoveRod(string gcode)
    {
        Vector3 targetPosition = ParseGCode(gcode, rod.localPosition);

        while (Vector3.Distance(rod.localPosition, targetPosition) > 0.01f)
        {
            rod.localPosition = Vector3.MoveTowards(rod.localPosition, targetPosition, moveSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
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
                x = float.Parse(part.Substring(1));
            }
            else if (part.StartsWith("Y"))
            {
                y =float.Parse(part.Substring(1));
            }
            else if (part.StartsWith("Z"))
            {
                z = float.Parse(part.Substring(1));
            }
        }

        return new Vector3(x, y, z);
    }

    private IEnumerator RotateFilament()
    {
        while (true)
        {
            if (filament != null)
            {
                // 현재 회전 상태를 가져옴
                Quaternion currentRotation = filament.transform.localRotation;

                // Y축을 기준으로 회전할 각도 계산
                Quaternion deltaRotation = Quaternion.Euler(0, rotSpeed * Time.deltaTime, 0);

                // 새로운 회전 상태 계산
                filament.transform.localRotation = currentRotation * deltaRotation;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
