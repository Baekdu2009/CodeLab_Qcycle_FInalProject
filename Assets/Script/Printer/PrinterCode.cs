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

    [Header("������ �۾�")]
    public PrinterSize size;
    public Transform nozzle;    // ����(Y)
    public Transform rod;       // �ε�(Z)
    public Transform plate;     // �÷���Ʈ(X)
    public MeshRenderer machineLight;
    public GameObject[] filaments; // �ʶ��Ʈ
    public bool[] filamentCCW;  //�ʶ��Ʈ ȸ������

    public float Xmin;
    public float Ymin;
    public float Zmin;
    public float Xmax;
    public float Ymax;
    public float Zmax;
    
    public float moveSpeed = 0.1f;              // �̵��ӵ�
    public float printingResolutionx = 0.02f;   // x�� �ػ�  -> range�� ���ؼ� resolution�� ��ȭ��Ű�� �ٲٱ�
    public float printingResolutiony = 0.02f;   // y�� �ػ�
    public float printingResolutionz = 0.02f;   // z�� �ػ�

    [Header("������UI")]
    public TMP_Text printerInformation; // ������ �۾� ũ��
    public TMP_Text printerWorkingTime; // ������ ���� �ð�
    public TMP_Text printerExpectTime;  // ������ ���� �ð�
    public TMP_Text printingStatus;     // ������ �����
    public TMP_Text filamentStatus;     // �ʶ��Ʈ ����
    public GameObject resetBtn;
    public GameObject Canvas;

    public Dictionary<string, GameObject> objectDictionary = new Dictionary<string, GameObject>();
    public TMP_Dropdown objectDropdown;     // Dropdown UI ���
    public GameObject[] objectPrefabs;      // ������ ���
    public Transform printingObjectLocate;  // ��µǴ� ������Ʈ�� ��ġ

    // private ���
    private GameObject printingObj;     // ���õ� ������Ʈ
    private GameObject visibleObject;   // ���� ��� ������Ʈ

    private Queue<string> nozzleQueue = new Queue<string>();
    private Queue<string> plateQueue = new Queue<string>();
    private Queue<string> rodQueue = new Queue<string>();

    private Vector3 plateOrigin;
    private Vector3 rodOrigin;
    private Vector3 nozzleOrigin;

    bool plateMoveOn = false;       // plate ������ ����
    bool isOriginLocate = false;    // ���� �̵� ����
    bool isPrinting;                // �μ� �� ����
    bool isObjSelect = false;       // �μ��� ������Ʈ ���� ����
    bool isPaused = false;         // �Ͻ����� ����

    private float rotSpeed = 200;                // �ʶ��Ʈ ȸ�� �ӵ�
    private float filamentUsingPercent = 100f;   // �ʶ��Ʈ ������

    private Coroutine originCoroutine;      // ���� �ڷ�ƾ
    private Coroutine finishCoroutine;      // ���� �ڷ�ƾ
    private float workingTime;              // �۾� �ð�(Seconds)
    private float expectedTime;             // �ܿ� ���� �ð�(Seconds)
    private float totalExpectedTime;        // ��ü ���� �ð�(Seconds)

    private void Start()
    {
        PrinterInformationNotice();
        SetExpectedTime(); // ���� �۾� �ð� ����
        resetBtn.SetActive(false); // �ʱ�ȭ ��ư ��Ȱ��ȭ
        filamentCCW = new bool[filaments.Length];

        plateOrigin = plate.transform.localPosition;
        rodOrigin = rod.transform.localPosition;
        nozzleOrigin = nozzle.transform.localPosition;
        machineLight.material.color = Color.yellow;

        objectDropdown.onValueChanged.AddListener(OnObjectSelected); // �̺�Ʈ ������ �߰�
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
            isPrinting = true; // �μ� ����
            workingTime = 0f; // �۾� �ð� �ʱ�ȭ
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
                print("���� �̵��� ���� ������ �ٶ��ϴ�.");
            }
            else if (!isObjSelect)
            {
                print("������ ������Ʈ�� �����Ͻñ� �ٶ��ϴ�.");
            }
            else if (filamentUsingPercent == 0)
            {
                print("�ʶ��Ʈ�� ��ü�Ͻñ� �ٶ��ϴ�.");
            }
        }
    }
    public void BtnTogglePauseProcess()
    {
        isPaused = !isPaused; // ���� �Ͻ����� ���¸� ����

        if (isPaused)
        {
            PauseProcess(); // ���� ���°� �Ͻ������� �Ͻ����� ȣ��
        }
        else
        {
            ResumeProcess(); // ���� ���°� �簳�� �簳 ȣ��
        }
    }

    private void PauseProcess()
    {
        isPrinting = false; // �μ� ����
        machineLight.material.color = Color.yellow; // ���� ����

        StopCoroutine(PrintProcess());
        StopCoroutine(RotateFilament());
        StopCoroutine(UpdateExpectedTime());
        StopCoroutine(UpdateWorkingTime());
        print("�Ͻ����� �Ǿ����ϴ�.");
    }

    private void ResumeProcess()
    {
        isPrinting = true; // �μ� �簳
        machineLight.material.color = Color.green; // ���� ����

        StartCoroutine(PrintProcess()); // �μ� ���μ��� �簳
        StartCoroutine(RotateFilament()); // �ʶ��Ʈ ȸ�� �簳
        StartCoroutine(UpdateWorkingTime()); // �۾� �ð� ������Ʈ �簳
        StartCoroutine(UpdateExpectedTime()); // ���� �ð� ������Ʈ �簳

        print("�۾��� �簳�մϴ�");
    }
    
    public void BtnStopProcess()
    {
        
        StopAllCoroutines();
        UpdateExpectedTime();
        resetBtn.SetActive(true);

        isPrinting = false;
        machineLight.material.color = Color.red;
        print("���� ����Ǿ����ϴ�.");
    }

    private IEnumerator PrintProcess()
    {
        while (isPrinting || !isPaused) // ���� ����
        {
            // ���� �
            yield return StartCoroutine(NozzleMovement());

            // �ε� �
            yield return StartCoroutine(RodMovement());

            // �÷���Ʈ �
            yield return StartCoroutine(PlateMovement());
        }
    }

    private IEnumerator NozzleMovement()
    {
        // Y�� �պ�
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

        // �࿡ ���� ��ġ ������Ʈ
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
                Debug.LogError("�߸��� ���Դϴ�. 'x', 'y', �Ǵ� 'z'�� �Է��ϼ���.");
                return; // �߸��� ���� ��� �޼��� ����
        }

        printingObjectLocate.localPosition = newPosition; // ���ο� ��ġ�� ������Ʈ
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

            workingTime += Time.deltaTime; // �帥 �ð� ������Ʈ

            // �ð��� hh:mm:ss �������� ��ȯ
            int hours = Mathf.FloorToInt(workingTime / 3600);
            int minutes = Mathf.FloorToInt((workingTime % 3600) / 60);
            int seconds = Mathf.FloorToInt(workingTime % 60);

            // �ؽ�Ʈ ������Ʈ
            printerWorkingTime.text = $"Working Time \n{hours:D2}:{minutes:D2}:{seconds:D2}"; // ���� ����
            printerWorkingTime.color = Color.yellow;

            yield return null; // ���� �����ӱ��� ���
        }
    }

    private IEnumerator UpdateExpectedTime()
    {
        while (isPrinting && expectedTime > 0 && !isPaused)
        {
            expectedTime -= Time.deltaTime;

            // �ð��� hh:mm:ss �������� ��ȯ
            int hours = Mathf.FloorToInt(expectedTime / 3600);
            int minutes = Mathf.FloorToInt((expectedTime % 3600) / 60);
            int seconds = Mathf.FloorToInt(expectedTime % 60);

            // �ؽ�Ʈ ������Ʈ
            printerExpectTime.text = $"Expected Time \n{hours:D2}:{minutes:D2}:{seconds:D2}"; // ���� ����
            printerExpectTime.color = Color.blue;

            UpdatePrintStatus();

            yield return null; // ���� �����ӱ��� ���
        }
        if (isPrinting && expectedTime < 0 && !isPaused)
        {
            PrinterFinish(); // ������ �Ϸ� ó��
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
        UpdateExpectTimeText(); // ���� �۾� �ð� ǥ��
    }

    private void UpdateExpectTimeText()
    {
        int hours = Mathf.FloorToInt(expectedTime / 3600);
        int minutes = Mathf.FloorToInt((expectedTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(expectedTime % 60);

        printerExpectTime.text = $"Expected Time \n{hours:D2}:{minutes:D2}:{seconds:D2}"; // ���� ����
    }

    private void UpdatePrintStatus()
    {
        // ������� ���������� ���
        int status = Mathf.FloorToInt((workingTime / totalExpectedTime) * 100);
        printingStatus.text = $"Printing Status \n{status:D2}%"; // ���������� ǥ��
        printingStatus.color = Color.cyan;
    }
    
    private void PrinterFinish()
    {
        StopAllCoroutines();

        resetBtn.SetActive(true);
        
        printerExpectTime.text = "Expected Time \n 00:00:00";
        printingStatus.text = "Printing Complete"; // �Ϸ� �޽��� ǥ��
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
        // �ʱ�ȭ �۾� ����
        workingTime = 0f; // �۾� �ð� �ʱ�ȭ
        expectedTime = totalExpectedTime; // ���� �ð� �ʱ�ȭ
        UpdateExpectTimeText(); // ���� �۾� �ð� �ؽ�Ʈ �ʱ�ȭ
        printingStatus.text = "Printing Status\n00%"; // ������ ���� �ʱ�ȭ
        printerExpectTime.text = $"Expect Time\n00:00:00";
        printerWorkingTime.text = "Working Time\n00:00:00";
        
        objectDropdown.interactable = true;
        isPrinting = false; // �μ� ���� ���·� ����
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
            filamentUsingPercent = Mathf.Max(filamentUsingPercent, 0); // 0 ���Ϸ� �������� �ʰ� ����
            filamentStatus.text = $"Filament Status\n{filamentUsingPercent}%";

            if (filamentUsingPercent <= 0)
            {
                Debug.Log("�ʶ��Ʈ�� �����Ǿ����ϴ�.");
            }
            yield return null;
        }
    }

    private void PopulateObjectDictionary()
    {
        // ���� ���� �ʱ�ȭ
        objectDictionary.Clear();

        // "None" �׸� �߰�
        objectDictionary.Add("None", null);

        // objectPrefabs�� ��� �׸��� objectDictionary�� �߰�
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            if (objectPrefabs[i] != null) // null üũ
            {
                objectDictionary.Add($"Example{i + 1}", objectPrefabs[i]);
            }
            else
            {
                Debug.LogWarning($"objectPrefabs[{i}]�� null�Դϴ�. Ȯ���ϼ���.");
            }
        }
        // DropDown ������Ʈ
        PopulateDropdown();
    }

    private void PopulateDropdown()
    {
        objectDropdown.options.Clear(); // ���� �ɼ� ����

        foreach (var key in objectDictionary.Keys)
        {
            objectDropdown.options.Add(new TMP_Dropdown.OptionData(key));
        }

        objectDropdown.value = 0; // �⺻�� ����
    }

    private void OnObjectSelected(int index)
    {
        if (index < 0 || index >= objectDropdown.options.Count) return; // �ε��� ���� üũ

        string selectedObjectName = objectDropdown.options[index].text;
        PrintObjectSelect(selectedObjectName);
    }

    public void PrintObjectSelect(string objectName)
    {
        if (objectName != "None" && objectDictionary.ContainsKey(objectName))
        {
            printingObj = objectDictionary[objectName];
            // ��ü ��� ���� �߰�
            Debug.Log($"{objectName}�� ����մϴ�.");
            isObjSelect = true; // ������Ʈ ���� ���� ����
        }
        else
        {
            isObjSelect = false;
            printingObj = null;
            Debug.Log("���� �۾��� ����߽��ϴ�.");
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
