using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

<<<<<<< Updated upstream
public class PrinterGcode : MonoBehaviour
{
    public enum PrinterSize
    {
        Large,
        Small
    }

    public PrinterSize size;
    public Transform nozzle;    // ����
    public Transform rod;       // �ε�
    public Transform plate;     // �÷���Ʈ
    public GameObject filament; // �ʶ��Ʈ
    public GameObject printingPrefab; // ��¹� ������

    public TMP_Text printerInformation;
    public TMP_Text printerWorkingTime;
    public TMP_Text printerExpectTime;
    public TMP_Text printingStatus;
    public GameObject resetBtn;

    public float Xmin;
    public float Ymin;
    public float Zmin;
    public float Xmax;
    public float Ymax;
    public float Zmax;

    private Queue<string> nozzleQueue = new Queue<string>();
    private Queue<string> plateQueue = new Queue<string>();
    private Queue<string> rodQueue = new Queue<string>();

    public float moveSpeed = 0.1f;  // �̵��ӵ�
    public float printingResolution = 0.02f;
    public float rotSpeed = 200;

    private Coroutine originCoroutine; // ���� �ڷ�ƾ
    private Coroutine finishCoroutine; // ���� �ڷ�ƾ
    private float workingTime; // �۾� �ð�
    private float expectedTime; // �ܿ� ���� �ð�
    private float totalExpectedTime; // ��ü ���� �ð�
    private bool isPrinting; // �μ� �� ����
    private GameObject printingObj; // ��¹�

    private void Start()
    {
        PrinterInformationNotice();
        SetExpectedTime(); // ���� �۾� �ð� ����
        resetBtn.SetActive(false); // �ʱ�ȭ ��ư ��Ȱ��ȭ
    }

    private void SetExpectedTime()
    {
        if (size == PrinterSize.Large)
        {
            expectedTime = 10; // 4�ð�
        }
        else if (size == PrinterSize.Small)
        {
            expectedTime = 7200; // 2�ð�
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
=======
public class PrinterGCode : MonoBehaviour
{
    public Transform nozzle;
    public Transform rod;
    public Transform plate;

    public float Xmin;
    public float Xmax;
    public float Ymin;
    public float Ymax;
    public float Zmin;
    public float Zmax;

    private float nozzleY;
    private float rodZ;
    private float plateX;

    private Queue<string> gcodeQueue = new Queue<string>();
    private bool isMoving = false;
    private Vector3 currentPosition;
    public float moveSpeed = 1.0f; // �̵� �ӵ�

    private void Start()
    {
        // �ʱ� ��ġ ����
        nozzleY = nozzle.localPosition.y;
        rodZ = rod.localPosition.z;
        plateX = plate.localPosition.x;
        currentPosition = nozzle.localPosition; // �ʱ� ��ġ ����

        GenerateGCode();
    }

    private void Update()
    {
        if (!isMoving && gcodeQueue.Count > 0)
        {
            string gcode = gcodeQueue.Dequeue();
            StartCoroutine(MoveNozzle(gcode));
        }
>>>>>>> Stashed changes
    }

    public void OriginBtnEvent()
    {
<<<<<<< Updated upstream
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
        GenerateGcode("G0", Xmin, 0, 0, plateQueue);
        GenerateGcode("G0", 0, Ymin, 0, nozzleQueue);
        GenerateGcode("G0", 0, 0, Zmin, rodQueue);

        yield return MoveNozzle(nozzleQueue.Dequeue());
        yield return MoveRod(rodQueue.Dequeue());
        yield return MovePlate(plateQueue.Dequeue());
    }
    private IEnumerator FinishPosition()
    {
        GenerateGcode("G0", Xmax, 0, 0, plateQueue);
        GenerateGcode("G0", 0, Ymax, 0, nozzleQueue);
        GenerateGcode("G0", 0, 0, Zmax, rodQueue);

        yield return MoveNozzle(nozzleQueue.Dequeue());
        yield return MoveRod(rodQueue.Dequeue());
        yield return MovePlate(plateQueue.Dequeue());
    }
    public void StartProcess()
    {
        isPrinting = true; // �μ� ����
        workingTime = 0f; // �۾� �ð� �ʱ�ȭ
        StartCoroutine(PrintProcess());
        StartCoroutine(RotateFilament());
        StartCoroutine(UpdateWorkingTime());
        StartCoroutine(UpdateExpectedTime());
    }

    public void StopProcess()
    {
        StopAllCoroutines();
        isPrinting = false; // �μ� ����
        UpdateExpectedTime();
    }

    private IEnumerator PrintProcess()
    {
        while (true) // ���� ����
        {
            // ���� Y������ �պ� �
            yield return StartCoroutine(NozzleMovement());

            // �÷���Ʈ X������ �̵�
            yield return StartCoroutine(PlateRodMovement());
        }
    }

    private IEnumerator NozzleMovement()
    {
        // Y�� �պ�
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

    private void GenerateGcode(string gcommand, float x, float y, float z, Queue<string> queue)
    {
        queue.Enqueue($"{gcommand} X{x} Y{y} Z{z}");
=======
        // Y���� Ymin���� Ymax���� �պ��ϸ� G�ڵ带 ����
        for (float y = Ymin; y <= Ymax; y += 0.01f)
        {
            gcodeQueue.Enqueue($"G1 X{nozzle.localPosition.x} Y{y} Z{nozzle.localPosition.z}");
        }
        for (float y = Ymax; y >= Ymin; y -= 0.01f)
        {
            gcodeQueue.Enqueue($"G1 X{nozzle.localPosition.x} Y{y} Z{nozzle.localPosition.z}");
        }
    }

    public void ProcessGCode(string gcode)
    {
        string[] commands = gcode.Split('\n');
        foreach (string command in commands)
        {
            string trimmedCommand = command.Trim();
            if (string.IsNullOrEmpty(trimmedCommand)) continue;

            if (trimmedCommand.StartsWith("G0") || trimmedCommand.StartsWith("G1"))
            {
                HandleMovement(trimmedCommand);
            }
        }
    }

    private void HandleMovement(string command)
    {
        float x = nozzle.localPosition.x;
        float y = nozzle.localPosition.y;
        float z = nozzle.localPosition.z;

        string[] parts = command.Split(' ');
        foreach (string part in parts)
        {
            if (part.StartsWith("X")) x = Mathf.Clamp(float.Parse(part.Substring(1)), Xmin, Xmax);
            else if (part.StartsWith("Y")) y = Mathf.Clamp(float.Parse(part.Substring(1)), Ymin, Ymax);
            else if (part.StartsWith("Z")) z = Mathf.Clamp(float.Parse(part.Substring(1)), Zmin, Zmax);
        }

        nozzleY = y; // Y ��ġ ������Ʈ
        UpdatePositions();
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
        return new Vector3(x, y, z);
    }

    private IEnumerator RotateFilament()
    {
        while (true)
        {
            if (filament != null)
            {
                Quaternion currentRotation = filament.transform.localRotation;
                Quaternion deltaRotation = Quaternion.Euler(0, rotSpeed * Time.deltaTime, 0);
                filament.transform.localRotation = currentRotation * deltaRotation;
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
    private void UpdatePrintStatus()
    {
        // ������� ���������� ���
        int status = Mathf.FloorToInt((workingTime / totalExpectedTime) * 100);
        printingStatus.text = $"Printing Status \n{status:D2}%"; // ���������� ǥ��
        printingStatus.color = Color.cyan;
    }


    private void PrinterFinish()
    {
        printerExpectTime.text = "Expected Time \n 00:00:00";
        StopAllCoroutines();

        // �ʱ�ȭ ��ư Ȱ��ȭ
        resetBtn.SetActive(true);
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
        isPrinting = false; // �μ� ���� ���·� ����
        resetBtn.SetActive(false);
        printingStatus.color = Color.black;
        printerExpectTime.color = Color.black;
        printerWorkingTime.color = Color.black;
=======
        // ������ ��ǥ ��ġ�� �ε巴�� �̵�
        while (Vector3.Distance(nozzle.localPosition, targetPosition) > 0.01f)
        {
            nozzle.localPosition = Vector3.MoveTowards(nozzle.localPosition, targetPosition, moveSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        currentPosition = targetPosition; // ���� ��ġ ������Ʈ
        isMoving = false;
    }

    private void UpdatePositions()
    {
        nozzle.localPosition = new Vector3(nozzle.localPosition.x, nozzleY, nozzle.localPosition.z);
        rod.localPosition = new Vector3(rod.localPosition.x, rod.localPosition.y, rodZ);
        plate.localPosition = new Vector3(plateX, plate.localPosition.y, plate.localPosition.z);
>>>>>>> Stashed changes
    }
}
