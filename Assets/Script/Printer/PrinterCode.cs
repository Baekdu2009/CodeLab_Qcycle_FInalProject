using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class PrinterCode : MonoBehaviour
{
    public enum PrinterSize
    {
        Large,
        Small
    }

    [Header("프린터 작업")]
    public PrinterSize size;
    public Transform nozzle;    // 노즐(Y)
    public Transform rod;       // 로드(Z)
    public Transform plate;     // 플레이트(X)
    public MeshRenderer machineLight;
    public GameObject[] filaments; // 필라멘트
    public bool[] filamentCCW;  //필라멘트 회전방향

    public float Xmin;
    public float Ymin;
    public float Zmin;
    public float Xmax;
    public float Ymax;
    public float Zmax;
    
    public float moveSpeed = 0.1f;              // 이동속도
    public float printingResolutionx = 0.02f;   // x축 해상도  -> range를 통해서 resolution을 변화시키게 바꾸기
    public float printingResolutiony = 0.02f;   // y축 해상도
    public float printingResolutionz = 0.02f;   // z축 해상도

    [Header("프린터UI")]
    public TMP_Text printerInformation; // 프린터 작업 크기
    public TMP_Text printerWorkingTime; // 프린터 진행 시간
    public TMP_Text printerExpectTime;  // 프린터 예상 시간
    public TMP_Text printingStatus;     // 프린터 진행률
    public TMP_Text filamentStatus;     // 필라멘트 상태
    public GameObject resetBtn;
    public GameObject Canvas;

    public Dictionary<string, GameObject> objectDictionary = new Dictionary<string, GameObject>();
    public TMP_Dropdown objectDropdown;     // Dropdown UI 요소
    public GameObject[] objectPrefabs;      // 프리팹 목록
    public Transform printingObjectLocate;  // 출력되는 오브젝트의 위치

    // private 목록
    private GameObject printingObj;     // 선택된 오브젝트
    private GameObject visibleObject;   // 실제 출력 오브젝트

    private Queue<string> nozzleQueue = new Queue<string>();
    private Queue<string> plateQueue = new Queue<string>();
    private Queue<string> rodQueue = new Queue<string>();

    private Vector3 plateOrigin;
    private Vector3 rodOrigin;
    private Vector3 nozzleOrigin;

    bool plateMoveOn = false;       // plate 움직임 여부
    bool isOriginLocate = false;    // 원점 이동 여부
    bool isPrinting;                // 인쇄 중 여부
    bool isObjSelect = false;       // 인쇄할 오브젝트 선택 여부
    bool isPaused = false;         // 일시정지 여부

    private float rotSpeed = 200;                // 필라멘트 회전 속도
    private float filamentUsingPercent = 100f;   // 필라멘트 남은량

    private Coroutine originCoroutine;      // 원점 코루틴
    private Coroutine finishCoroutine;      // 종료 코루틴
    private float workingTime;              // 작업 시간(Seconds)
    private float expectedTime;             // 잔여 예상 시간(Seconds)
    private float totalExpectedTime;        // 전체 예상 시간(Seconds)

    private void Start()
    {
        PrinterInformationNotice();
        SetExpectedTime(); // 예상 작업 시간 설정
        resetBtn.SetActive(false); // 초기화 버튼 비활성화
        filamentCCW = new bool[filaments.Length];

        plateOrigin = plate.transform.localPosition;
        rodOrigin = rod.transform.localPosition;
        nozzleOrigin = nozzle.transform.localPosition;
        machineLight.material.color = Color.yellow;

        objectDropdown.onValueChanged.AddListener(OnObjectSelected); // 이벤트 리스너 추가
        PopulateObjectDictionary();
    }

    private void Update()
    {
        FilamentStatusUpdate();
        if (plateMoveOn)
        {
            PrintingObjectControl();
        }

    }

    public void BtnOriginEvent()
    {
        if (originCoroutine == null && !isPrinting)
        {
            originCoroutine = StartCoroutine(OriginPosition());
        }
        else
        {
            StopCoroutine(originCoroutine);
            StopCoroutine(RotateFilament());
            originCoroutine = null;
        }
        isOriginLocate = true;
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

    public void BtnStartProcess()
    {
        if (isOriginLocate && isObjSelect && filamentUsingPercent != 0)
        {
            isPrinting = true; // 인쇄 시작
            workingTime = 0f; // 작업 시간 초기화
            objectDropdown.interactable = false;
            machineLight.material.color = Color.green;
            
            StartCoroutine(PrintProcess());
            StartCoroutine(RotateFilament());
            StartCoroutine(UpdateWorkingTime());
            StartCoroutine(UpdateExpectedTime());
        }
        else
        {
            if (!isOriginLocate)
            {
                print("원점 이동을 먼저 누르기 바랍니다.");
            }
            else if (!isObjSelect)
            {
                print("프린팅 오브젝트를 선택하시기 바랍니다.");
            }
            else if (filamentUsingPercent == 0)
            {
                print("필라멘트를 교체하시기 바랍니다.");
            }
        }
    }
    public void BtnTogglePauseProcess()
    {
        isPaused = !isPaused; // 현재 일시정지 상태를 반전

        if (isPaused)
        {
            PauseProcess(); // 현재 상태가 일시정지면 일시정지 호출
        }
        else
        {
            ResumeProcess(); // 현재 상태가 재개면 재개 호출
        }
    }

    private void PauseProcess()
    {
        isPrinting = false; // 인쇄 중지
        machineLight.material.color = Color.yellow; // 색상 변경

        StopCoroutine(PrintProcess());
        StopCoroutine(RotateFilament());
        StopCoroutine(UpdateExpectedTime());
        StopCoroutine(UpdateWorkingTime());
        print("일시정지 되었습니다.");
    }

    private void ResumeProcess()
    {
        isPrinting = true; // 인쇄 재개
        machineLight.material.color = Color.green; // 색상 변경

        StartCoroutine(PrintProcess()); // 인쇄 프로세스 재개
        StartCoroutine(RotateFilament()); // 필라멘트 회전 재개
        StartCoroutine(UpdateWorkingTime()); // 작업 시간 업데이트 재개
        StartCoroutine(UpdateExpectedTime()); // 예상 시간 업데이트 재개

        print("작업을 재개합니다");
    }
    
    public void BtnStopProcess()
    {
        
        StopAllCoroutines();
        UpdateExpectedTime();
        resetBtn.SetActive(true);

        isPrinting = false;
        machineLight.material.color = Color.red;
        print("강제 종료되었습니다.");
    }

    private IEnumerator PrintProcess()
    {
        while (isPrinting || !isPaused) // 무한 루프
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
        for (float x = Xmin; x <= Xmax; x += printingResolutionx)
        {
            GenerateGcode("G1", x, nozzleOrigin.y, nozzleOrigin.z, nozzleQueue);
            yield return MoveNozzle(nozzleQueue.Dequeue());
        }

        for (float x = Xmax; x >= Xmin; x -= printingResolutionx)
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
                GenerateGcode("G1", plateOrigin.x, plateOrigin.y, plate.localPosition.z - printingResolutionz, plateQueue);
            }
            yield return MovePlate(plateQueue.Dequeue());
            
        }
        plateMoveOn = false;
    }

    private IEnumerator RodMovement()
    {
        if(rod.localPosition.y <= Ymax)
        {
            GenerateGcode("G1", rodOrigin.x, rod.localPosition.y + printingResolutiony, rodOrigin.z, rodQueue);
        }
        else
        {
            GenerateGcode("G1", rodOrigin.x, Ymin, rodOrigin.z, rodQueue);
            plateMoveOn = true;
        }
        yield return MoveRod(rodQueue.Dequeue());
    }
    
    private void UpdatePrintingObjectLocate(string axis, Transform referenceTransform, float resolution)
    {
        Vector3 newPosition = printingObjectLocate.localPosition;

        // 축에 따라 위치 업데이트
        switch (axis.ToLower())
        {
            case "x":
                newPosition.x = referenceTransform.localPosition.x + resolution;
                break;
            case "y":
                newPosition.y = referenceTransform.localPosition.y + resolution;
                break;
            case "z":
                newPosition.z = referenceTransform.localPosition.z + resolution;
                break;
            default:
                Debug.LogError("잘못된 축입니다. 'x', 'y', 또는 'z'를 입력하세요.");
                return; // 잘못된 축인 경우 메서드 종료
        }

        printingObjectLocate.localPosition = newPosition; // 새로운 위치로 업데이트
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
        printerInformation.text = "Working Space\n" + information + "(mm)";
    }

    private IEnumerator UpdateWorkingTime()
    {
        while (isPrinting && !isPaused)
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
        while (isPrinting && expectedTime > 0 && !isPaused)
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
        if (isPrinting && expectedTime < 0 && !isPaused)
        {
            PrinterFinish(); // 프린터 완료 처리
        }
    }

    private void SetExpectedTime()
    {
        if (size == PrinterSize.Large)
        {
            expectedTime = 600;
        }
        else if (size == PrinterSize.Small)
        {
            expectedTime = 13;
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

    private void UpdatePrintStatus()
    {
        // 진행률을 정수형으로 계산
        int status = Mathf.FloorToInt((workingTime / totalExpectedTime) * 100);
        printingStatus.text = $"Printing Status \n{status:D2}%"; // 정수형으로 표시
        printingStatus.color = Color.cyan;
    }
    
    private void PrinterFinish()
    {
        StopAllCoroutines();

        resetBtn.SetActive(true);
        
        printerExpectTime.text = "Expected Time \n 00:00:00";
        printingStatus.text = "Printing Complete"; // 완료 메시지 표시
        printingStatus.color = Color.red;
        machineLight.material.color = Color.yellow;

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

    public void BtnResetPrinter()
    {
        // 초기화 작업 수행
        workingTime = 0f; // 작업 시간 초기화
        expectedTime = totalExpectedTime; // 예상 시간 초기화
        UpdateExpectTimeText(); // 예상 작업 시간 텍스트 초기화
        printingStatus.text = "Printing Status\n00%"; // 프린팅 상태 초기화
        printerExpectTime.text = $"Expect Time\n00:00:00";
        printerWorkingTime.text = "Working Time\n00:00:00";
        
        objectDropdown.interactable = true;
        isPrinting = false; // 인쇄 중지 상태로 설정
        resetBtn.SetActive(false);
        printingStatus.color = Color.black;
        printerExpectTime.color = Color.black;
        printerWorkingTime.color = Color.black;
    }

    private void FilamentStatusUpdate()
    {
        if (filaments != null)
        {
            filamentStatus.text = $"Filament Status\n{filamentUsingPercent}%";
            // Start the filament usage tracking coroutine if not already running
            if (isPrinting && filamentUsingPercent > 0)
            {
                StartCoroutine(FilamentUsingPercent());
            }
        }
    }

    private IEnumerator FilamentUsingPercent()
    {
        while (isPrinting && filamentUsingPercent > 0)
        {
            filamentUsingPercent = Mathf.Max(filamentUsingPercent, 0); // 0 이하로 떨어지지 않게 조정
            filamentStatus.text = $"Filament Status\n{filamentUsingPercent}%";

            if (filamentUsingPercent <= 0)
            {
                Debug.Log("필라멘트가 소진되었습니다.");
            }
            yield return null;
        }
    }

    private void PopulateObjectDictionary()
    {
        // 기존 내용 초기화
        objectDictionary.Clear();

        // "None" 항목 추가
        objectDictionary.Add("None", null);

        // objectPrefabs의 모든 항목을 objectDictionary에 추가
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            if (objectPrefabs[i] != null) // null 체크
            {
                objectDictionary.Add($"Example{i + 1}", objectPrefabs[i]);
            }
            else
            {
                Debug.LogWarning($"objectPrefabs[{i}]는 null입니다. 확인하세요.");
            }
        }
        // DropDown 업데이트
        PopulateDropdown();
    }

    private void PopulateDropdown()
    {
        objectDropdown.options.Clear(); // 기존 옵션 제거

        foreach (var key in objectDictionary.Keys)
        {
            objectDropdown.options.Add(new TMP_Dropdown.OptionData(key));
        }

        objectDropdown.value = 0; // 기본값 설정
    }

    private void OnObjectSelected(int index)
    {
        if (index < 0 || index >= objectDropdown.options.Count) return; // 인덱스 범위 체크

        string selectedObjectName = objectDropdown.options[index].text;
        PrintObjectSelect(selectedObjectName);
    }

    public void PrintObjectSelect(string objectName)
    {
        if (objectName != "None" && objectDictionary.ContainsKey(objectName))
        {
            printingObj = objectDictionary[objectName];
            // 객체 출력 로직 추가
            Debug.Log($"{objectName}를 출력합니다.");
            isObjSelect = true; // 오브젝트 선택 상태 설정
        }
        else
        {
            isObjSelect = false;
            printingObj = null;
            Debug.Log("기존 작업을 취소했습니다.");
        }
    }

    private void PrintingObjectControl()
    {
        if (visibleObject == null)
            PrintingObjectCreate();
        else
            PrintingObjectScaleChange();
    }

    private void PrintingObjectCreate()
    {
        printingObj.GetComponent<GameObject>();
        visibleObject = Instantiate(printingObj, printingObjectLocate);
        visibleObject.transform.localPosition = new Vector3(0, -0.02f, 0);
        visibleObject.transform.localScale = new Vector3(1, 1, 0);
    }

    private void PrintingObjectScaleChange()
    {
        visibleObject.transform.localScale = new Vector3(visibleObject.transform.localScale.x, visibleObject.transform.localScale.y, visibleObject.transform.localScale.z + printingResolutionz / 10);
    }
}
