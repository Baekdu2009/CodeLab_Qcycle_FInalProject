using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class PrinterGcode : MonoBehaviour
{
    public enum PrinterSize
    {
        Large,
        Small
    }

    public PrinterSize size;
    public Transform nozzle;    // 노즐(Y)
    public Transform rod;       // 로드(Z)
    public Transform plate;     // 플레이트(X)
    public GameObject[] filaments; // 필라멘트
    public bool[] filamentCCW;

    public TMP_Text printerInformation;
    public TMP_Text printerWorkingTime;
    public TMP_Text printerExpectTime;
    public TMP_Text printingStatus;
    public GameObject resetBtn;
    public GameObject Canvas;

    public float Xmin;
    public float Ymin;
    public float Zmin;
    public float Xmax;
    public float Ymax;
    public float Zmax;

    private Queue<string> nozzleQueue = new Queue<string>();
    private Queue<string> plateQueue = new Queue<string>();
    private Queue<string> rodQueue = new Queue<string>();

    private Vector3 plateOrigin;
    private Vector3 rodOrigin;
    private Vector3 nozzleOrigin;

    bool plateMoveOn = false;

    public float moveSpeed = 0.1f;  // 이동속도
    public float printingResolution = 0.02f;
    public float rotSpeed = 200;

    private Coroutine originCoroutine; // 원점 코루틴
    private Coroutine finishCoroutine; // 종료 코루틴
    private float workingTime; // 작업 시간
    private float expectedTime; // 잔여 예상 시간
    private float totalExpectedTime; // 전체 예상 시간
    private bool isPrinting; // 인쇄 중 여부

    private void Start()
    {
        PrinterInformationNotice();
        SetExpectedTime(); // 예상 작업 시간 설정
        resetBtn.SetActive(false); // 초기화 버튼 비활성화
        filamentCCW = new bool[filaments.Length];

        plateOrigin = plate.transform.localPosition;
        rodOrigin = rod.transform.localPosition;
        nozzleOrigin = nozzle.transform.localPosition;
    }
    
    private void SetExpectedTime()
    {
        if (size == PrinterSize.Large)
        {
            expectedTime = 14400; // 4시간
        }
        else if (size == PrinterSize.Small)
        {
            expectedTime = 7200; // 2시간
        }

        totalExpectedTime = expectedTime;
        UpdateExpectTimeText(); // 예상 작업 시간 표시
    }

    private void UpdateExpectTimeText()
    {
        int hours = Mathf.FloorToInt(expectedTime / 3600);
        int minutes = Mathf.FloorToInt((expectedTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(expectedTime % 60);

        printerExpectTime.text = $"Expected Time \n{hours:D2}:{minutes:D2}:{seconds:D2}"; // 형식 지정
    }
    public void OriginBtnEvent()
    {
        if (originCoroutine == null)
        {
            originCoroutine = StartCoroutine(OriginPosition());
        }
        else
        {
            StopCoroutine(originCoroutine);
            StopCoroutine(RotateFilament());
            originCoroutine = null;
        }
    }

    private IEnumerator OriginPosition()
    {
        GenerateGcode("G0", Xmin, nozzleOrigin.y, nozzleOrigin.z, nozzleQueue);
        GenerateGcode("G0", rodOrigin.x, Ymin, rodOrigin.z, rodQueue);
        GenerateGcode("G0", plateOrigin.x, plateOrigin.y, Zmax, plateQueue);

        yield return MoveNozzle(nozzleQueue.Dequeue());
        yield return MoveRod(rodQueue.Dequeue());
        yield return MovePlate(plateQueue.Dequeue());
    }
    private IEnumerator FinishPosition()
    {
        GenerateGcode("G0", Xmax, nozzleOrigin.y, nozzleOrigin.z, nozzleQueue);
        GenerateGcode("G0", rodOrigin.x, Ymax, rodOrigin.z, rodQueue);
        GenerateGcode("G0", plateOrigin.x, plateOrigin.y, Zmin, plateQueue);

        yield return MoveNozzle(nozzleQueue.Dequeue());
        yield return MoveRod(rodQueue.Dequeue());
        yield return MovePlate(plateQueue.Dequeue());
    }
    public void StartProcess()
    {
        isPrinting = true; // 인쇄 시작
        workingTime = 0f; // 작업 시간 초기화
        StartCoroutine(PrintProcess());
        StartCoroutine(RotateFilament());
        StartCoroutine(UpdateWorkingTime());
        StartCoroutine(UpdateExpectedTime());
    }

    public void StopProcess()
    {
        StopAllCoroutines();
        isPrinting = false; // 인쇄 중지
        UpdateExpectedTime();
    }

    private IEnumerator PrintProcess()
    {
        while (true) // 무한 루프
        {
            // 노즐 운동
            yield return StartCoroutine(NozzleMovement());

            // 로드 운동
            yield return StartCoroutine(RodMovement());

            // 플레이트 운동
            yield return StartCoroutine(PlateMovement());
        }
    }

    private IEnumerator NozzleMovement()
    {
        // Y축 왕복
        for (float x = Xmin; x <= Xmax; x += printingResolution)
        {
            GenerateGcode("G1", x, nozzleOrigin.y, nozzleOrigin.z, nozzleQueue);
            yield return MoveNozzle(nozzleQueue.Dequeue());
        }

        for (float x = Xmax; x >= Xmin; x -= printingResolution)
        {
            GenerateGcode("G1", x, nozzleOrigin.y, nozzleOrigin.z, nozzleQueue);
            yield return MoveNozzle(nozzleQueue.Dequeue());
        }
    }
    
    private IEnumerator PlateMovement()
    {
        if (plateMoveOn)
        {
            if (plate.localPosition.z <= Zmin)
            {
                GenerateGcode("G1", plateOrigin.x, plateOrigin.y, Zmax, plateQueue);
            }
            else
            {
                GenerateGcode("G1", plateOrigin.x, plateOrigin.y, plate.localPosition.z - printingResolution, plateQueue);
            }
            yield return MovePlate(plateQueue.Dequeue());
        }
        plateMoveOn = false;
    }

    private IEnumerator RodMovement()
    {
        if(rod.localPosition.y <= Ymax)
        {
            GenerateGcode("G1", rodOrigin.x, rod.localPosition.y + printingResolution, rodOrigin.z, rodQueue);
        }
        else
        {
            GenerateGcode("G1", rodOrigin.x, Ymin, rodOrigin.z, rodQueue);
            plateMoveOn = true;
        }
        yield return MoveRod(rodQueue.Dequeue());
    }


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
                y = float.Parse(part.Substring(1));
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
            if (filaments != null)
            {
                foreach(var filament in filaments)
                {
                    foreach (bool direction in filamentCCW)
                    {
                        if (direction) rotSpeed *= -1;
                        else rotSpeed *= 1;
                    }
                    Quaternion currentRotation = filament.transform.localRotation;
                    Quaternion deltaRotation = Quaternion.Euler(0, 0, rotSpeed * Time.deltaTime);
                    filament.transform.localRotation = currentRotation * deltaRotation;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void PrinterInformationNotice()
    {
        float x = (Xmax - Xmin) * 1000;
        float y = (Ymax - Ymin) * 1000;
        float z = (Zmax - Zmin) * 1000;
        string information = $"W{y} * B{x} * H{z}";
        printerInformation.text = "Working Space \n" + information + " (mm)";
    }

    private IEnumerator UpdateWorkingTime()
    {
        while (isPrinting)
        {

            workingTime += Time.deltaTime; // 흐른 시간 업데이트

            // 시간을 hh:mm:ss 형식으로 변환
            int hours = Mathf.FloorToInt(workingTime / 3600);
            int minutes = Mathf.FloorToInt((workingTime % 3600) / 60);
            int seconds = Mathf.FloorToInt(workingTime % 60);

            // 텍스트 업데이트
            printerWorkingTime.text = $"Working Time \n{hours:D2}:{minutes:D2}:{seconds:D2}"; // 형식 지정
            printerWorkingTime.color = Color.yellow;

            yield return null; // 다음 프레임까지 대기
        }
    }
    private IEnumerator UpdateExpectedTime()
    {
        while (isPrinting && expectedTime > 0)
        {
            expectedTime -= Time.deltaTime;

            // 시간을 hh:mm:ss 형식으로 변환
            int hours = Mathf.FloorToInt(expectedTime / 3600);
            int minutes = Mathf.FloorToInt((expectedTime % 3600) / 60);
            int seconds = Mathf.FloorToInt(expectedTime % 60);

            // 텍스트 업데이트
            printerExpectTime.text = $"Expected Time \n{hours:D2}:{minutes:D2}:{seconds:D2}"; // 형식 지정
            printerExpectTime.color = Color.blue;

            UpdatePrintStatus();

            yield return null; // 다음 프레임까지 대기
        }

        // 예상 시간이 0에 도달했을 때
        PrinterFinish(); // 프린터 완료 처리
    }
    private void UpdatePrintStatus()
    {
        // 진행률을 정수형으로 계산
        int status = Mathf.FloorToInt((workingTime / totalExpectedTime) * 100);
        printingStatus.text = $"Printing Status \n{status:D2}%"; // 정수형으로 표시
        printingStatus.color = Color.cyan;
    }


    private void PrinterFinish()
    {
        printerExpectTime.text = "Expected Time \n 00:00:00";
        StopAllCoroutines();

        // 초기화 버튼 활성화
        resetBtn.SetActive(true);
        printingStatus.text = "Printing Complete"; // 완료 메시지 표시
        printingStatus.color = Color.red;

        if (finishCoroutine == null)
        {
            finishCoroutine = StartCoroutine(FinishPosition());
        }
        else
        {
            StopCoroutine(finishCoroutine);
            finishCoroutine = null;
        }
    }
    public void ResetPrinter()
    {
        // 초기화 작업 수행
        workingTime = 0f; // 작업 시간 초기화
        expectedTime = totalExpectedTime; // 예상 시간 초기화
        UpdateExpectTimeText(); // 예상 작업 시간 텍스트 초기화
        printingStatus.text = "Printing Status \n00%"; // 프린팅 상태 초기화
        printerExpectTime.text = $"Expect Time \n00:00:00";
        printerWorkingTime.text = "Working Time \n00:00:00";
        isPrinting = false; // 인쇄 중지 상태로 설정
        resetBtn.SetActive(false);
        printingStatus.color = Color.black;
        printerExpectTime.color = Color.black;
        printerWorkingTime.color = Color.black;
    }
}
