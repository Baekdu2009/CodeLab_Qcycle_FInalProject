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

    [Header("������ �۾�")]
    public PrinterSize size;
    public Transform nozzle;    // ����(Y)
    public Transform rod;       // �ε�(Z)
    public Transform plate;     // �÷���Ʈ(X)
    public GameObject[] filaments; // �ʶ��Ʈ
    public bool[] filamentCCW;  //�ʶ��Ʈ ȸ������

    public float Xmin;
    public float Ymin;
    public float Zmin;
    public float Xmax;
    public float Ymax;
    public float Zmax;
    
    public float moveSpeed = 0.1f;              // �̵��ӵ�
    public float printingResolutionx = 0.02f;   // x�� �ػ�
    public float printingResolutiony = 0.02f;   // y�� �ػ�
    public float printingResolutionz = 0.02f;   // z�� �ػ�
    public float rotSpeed = 200;                // �ʶ��Ʈ ȸ�� �ӵ�

    [Header("������UI")]
    public TMP_Text printerInformation; // ������ �۾� ũ��
    public TMP_Text printerWorkingTime; // ������ ���� �ð�
    public TMP_Text printerExpectTime;  // ������ ���� �ð�
    public TMP_Text printingStatus;     // ������ �����
    public GameObject resetBtn;
    public GameObject Canvas;

    [Header("������ ������Ʈ")]
    public Dictionary<string, GameObject> objectDictionary = new Dictionary<string, GameObject>();
    public TMP_Dropdown objectDropdown;     // Dropdown UI ���
    public GameObject[] objectPrefabs;      // ������ ���
    public Transform printingObjectLocate;  // ��µǴ� ������Ʈ�� ��ġ


    // private ���
    private GameObject printingObj;     // ��µ� ������Ʈ

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

        objectDropdown.onValueChanged.AddListener(OnObjectSelected); // �̺�Ʈ ������ �߰�
        PopulateObjectDictionary();
    }
    
    public void OriginBtnEvent()
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
    public void StartProcess()
    {
        if (isOriginLocate && isObjSelect)
        {
            isPrinting = true; // �μ� ����
            workingTime = 0f; // �۾� �ð� �ʱ�ȭ
            objectDropdown.interactable = false;
            
            StartCoroutine(PrintProcess());
            StartCoroutine(RotateFilament());
            StartCoroutine(UpdateWorkingTime());
            StartCoroutine(UpdateExpectedTime());
        }
        else
        {
            if (!isOriginLocate)
            {
                Debug.Log("���� �̵��� �����ּ���.");
            }
            else if (!isObjSelect)
            {
                Debug.Log("������ ������Ʈ�� �������ּ���.");
            }
        }
    }

    public void StopProcess()
    {
        StopAllCoroutines();
        isPrinting = false; // �μ� ����
        UpdateExpectedTime();
        resetBtn.SetActive(true);

        print("���� ����Ǿ����ϴ�.");
    }

    private IEnumerator PrintProcess()
    {
        while (true) // ���� ����
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
            
            UpdatePrintingObjectLocate("z", plate, printingResolutionz);
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
        Vector3 newPosition = printingObjectLocate.position;

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

        printingObjectLocate.position = newPosition; // ���ο� ��ġ�� ������Ʈ
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
        while (isPrinting && expectedTime > 0)
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

        // ���� �ð��� 0�� �������� ��
        PrinterFinish(); // ������ �Ϸ� ó��
    }

    private void SetExpectedTime()
    {
        if (size == PrinterSize.Large)
        {
            expectedTime = 20;
        }
        else if (size == PrinterSize.Small)
        {
            expectedTime = 10;
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
        // �ʱ�ȭ �۾� ����
        workingTime = 0f; // �۾� �ð� �ʱ�ȭ
        expectedTime = totalExpectedTime; // ���� �ð� �ʱ�ȭ
        UpdateExpectTimeText(); // ���� �۾� �ð� �ؽ�Ʈ �ʱ�ȭ
        printingStatus.text = "Printing Status \n00%"; // ������ ���� �ʱ�ȭ
        printerExpectTime.text = $"Expect Time \n00:00:00";
        printerWorkingTime.text = "Working Time \n00:00:00";
        
        objectDropdown.interactable = true;
        isPrinting = false; // �μ� ���� ���·� ����
        resetBtn.SetActive(false);
        printingStatus.color = Color.black;
        printerExpectTime.color = Color.black;
        printerWorkingTime.color = Color.black;
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

}
