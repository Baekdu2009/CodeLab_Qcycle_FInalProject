using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrinterGcode : MonoBehaviour
{
    public Transform nozzle;    // 노즐
    public Transform rod;       // 로드
    public Transform plate;     // 플레이트
    public GameObject filament; // 필라멘트
    public GameObject cubePrefab;
    public Transform ObjectPos;
    GameObject cube;

    public float Xmin;
    public float Ymin;
    public float Zmin;
    public float Xmax;
    public float Ymax;
    public float Zmax;

    private Queue<Vector3> nozzleQueue = new Queue<Vector3>();
    private Queue<Vector3> plateQueue = new Queue<Vector3>();
    private Queue<Vector3> rodQueue = new Queue<Vector3>();

    private bool isMovingNozzle = false;
    private bool isMovingPlate = false;
    private bool isMovingRod = false;
    private bool isObject = false;

    public float moveSpeed = 0.1f;  // 이동속도
    public float filamentRotSpeed = 200; // 필라멘트 회전 속도
    public float printingResolution = 0.02f;

    private void Start()
    {
        plate.localPosition = new Vector3(Xmin, 0, 0);

        StartCoroutine(PrintProcess());
    }

    private void Update()
    {
        RotateFilament();
        CubeControl();
    }

    private IEnumerator PrintProcess()
    {
        while (true) // 무한 루프
        {
            // 노즐 Y축으로 왕복 운동
            yield return StartCoroutine(NozzleYMovement());

            // 플레이트 X축으로 이동
            yield return StartCoroutine(PlateXMovement());

            // 로드 Z축으로 이동
            // yield return StartCoroutine(RodZMovement());

            // 플레이트가 Xmax에 도달하면 노즐과 플레이트를 초기 위치로 이동
            yield return StartCoroutine(ResetPositions());
        }
    }

    private IEnumerator NozzleYMovement()
    {
        isMovingNozzle = true;

        // Y축 왕복
        for (float y = Ymin; y <= Ymax; y += printingResolution)
        {
            Vector3 targetPosition = new Vector3(0, y, 0);
            nozzleQueue.Enqueue(targetPosition);
            yield return MoveNozzle(nozzleQueue.Dequeue());
        }
        for (float y = Ymax; y >= Ymin; y -= printingResolution)
        {
            Vector3 targetPosition = new Vector3(0, y, 0);
            nozzleQueue.Enqueue(targetPosition);
            yield return MoveNozzle(nozzleQueue.Dequeue());
        }

        isMovingNozzle = false;
    }

    private IEnumerator PlateXMovement()
    {
        isMovingPlate = true;

        if (plate.localPosition.x >= Xmax - printingResolution)
        {
            Vector3 targetPosition = new Vector3(Xmin, 0, 0);
            plateQueue.Enqueue(targetPosition);
        }
        else
        {
            Vector3 targetPosition = new Vector3(plate.localPosition.x + printingResolution, 0, 0);
            plateQueue.Enqueue(targetPosition);
        }

        yield return MovePlate(plateQueue.Dequeue());

        isMovingPlate = false;
    }

    //private IEnumerator RodZMovement()
    //{
    //    isMovingRod = true;

    //    // Z축으로 이동
    //    for (float z = Zmin; z <= Zmax; z += 0.01f)
    //    {
    //        Vector3 targetPosition = new Vector3(0, 0, z);
    //        rodQueue.Enqueue(targetPosition);
    //        yield return MoveRod(rodQueue.Dequeue());
    //    }

    //    isMovingRod = false;
    //}

    private IEnumerator ResetPositions()
    {
        // 플레이트를 초기 위치로 이동
        Vector3 plateResetPosition = new Vector3(plate.localPosition.x, 0, 0);
        plateQueue.Enqueue(plateResetPosition);
        yield return MovePlate(plateQueue.Dequeue());

        // 노즐을 Ymin으로 이동
        Vector3 nozzleResetPosition = new Vector3(0, Ymin, 0);
        nozzleQueue.Enqueue(nozzleResetPosition);
        yield return MoveNozzle(nozzleQueue.Dequeue());

        // 로드를 Z축으로 printingResolution만큼 이동
        if (plate.localPosition.x >= Xmax - printingResolution)
        {
            Vector3 rodResetPosition = new Vector3(0, 0, rod.localPosition.z + printingResolution);
            rodQueue.Enqueue(rodResetPosition);
        }
        else
        {
            Vector3 rodResetPosition = new Vector3(0, 0, rod.localPosition.z);
            rodQueue.Enqueue(rodResetPosition);

        }
        yield return MoveRod(rodQueue.Dequeue());

        yield return null; // 다음 프레임으로 넘어가기
    }

    private IEnumerator MoveNozzle(Vector3 targetPosition)
    {
        while (Vector3.Distance(nozzle.localPosition, targetPosition) > 0.01f)
        {
            nozzle.localPosition = Vector3.MoveTowards(nozzle.localPosition, targetPosition, moveSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator MovePlate(Vector3 targetPosition)
    {
        while (Vector3.Distance(plate.localPosition, targetPosition) > 0.01f)
        {
            plate.localPosition = Vector3.MoveTowards(plate.localPosition, targetPosition, moveSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator MoveRod(Vector3 targetPosition)
    {
        while (Vector3.Distance(rod.localPosition, targetPosition) > 0.01f)
        {
            rod.localPosition = Vector3.MoveTowards(rod.localPosition, targetPosition, moveSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private void RotateFilament()
    {
        if (filament != null)
        {
            // 현재 회전 상태를 가져옴
            Quaternion currentRotation = filament.transform.localRotation;

            // Y축을 기준으로 회전할 각도 계산
            Quaternion deltaRotation = Quaternion.Euler(0, filamentRotSpeed * Time.deltaTime, 0);

            // 새로운 회전 상태 계산
            filament.transform.localRotation = currentRotation * deltaRotation;
        }
    }

    public void CubeControl()
    {
        if (cube == null)
        {
            cube = Instantiate(cubePrefab, ObjectPos);
            
        }
    }
}
