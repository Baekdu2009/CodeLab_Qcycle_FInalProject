using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrinterGcode : MonoBehaviour
{
    public Transform nozzle;    // ����
    public Transform rod;       // �ε�
    public Transform plate;     // �÷���Ʈ
    public GameObject filament; // �ʶ��Ʈ
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

    public float moveSpeed = 0.1f;  // �̵��ӵ�
    public float filamentRotSpeed = 200; // �ʶ��Ʈ ȸ�� �ӵ�
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
        while (true) // ���� ����
        {
            // ���� Y������ �պ� �
            yield return StartCoroutine(NozzleYMovement());

            // �÷���Ʈ X������ �̵�
            yield return StartCoroutine(PlateXMovement());

            // �ε� Z������ �̵�
            // yield return StartCoroutine(RodZMovement());

            // �÷���Ʈ�� Xmax�� �����ϸ� ����� �÷���Ʈ�� �ʱ� ��ġ�� �̵�
            yield return StartCoroutine(ResetPositions());
        }
    }

    private IEnumerator NozzleYMovement()
    {
        isMovingNozzle = true;

        // Y�� �պ�
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

    //    // Z������ �̵�
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
        // �÷���Ʈ�� �ʱ� ��ġ�� �̵�
        Vector3 plateResetPosition = new Vector3(plate.localPosition.x, 0, 0);
        plateQueue.Enqueue(plateResetPosition);
        yield return MovePlate(plateQueue.Dequeue());

        // ������ Ymin���� �̵�
        Vector3 nozzleResetPosition = new Vector3(0, Ymin, 0);
        nozzleQueue.Enqueue(nozzleResetPosition);
        yield return MoveNozzle(nozzleQueue.Dequeue());

        // �ε带 Z������ printingResolution��ŭ �̵�
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

        yield return null; // ���� ���������� �Ѿ��
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
            // ���� ȸ�� ���¸� ������
            Quaternion currentRotation = filament.transform.localRotation;

            // Y���� �������� ȸ���� ���� ���
            Quaternion deltaRotation = Quaternion.Euler(0, filamentRotSpeed * Time.deltaTime, 0);

            // ���ο� ȸ�� ���� ���
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
