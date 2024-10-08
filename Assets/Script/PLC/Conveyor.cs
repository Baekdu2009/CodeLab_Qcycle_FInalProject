using UnityEngine;

public class Conveyor : MonoBehaviour
{
    private Transform[] items; // �����̴��� �ڽ� ��ü��
    private Vector3[] initialPositions; // �ʱ� ��ġ �迭
    private Quaternion[] initialRotations; // �ʱ� ȸ�� �迭
    public float moveSpeed = 500f; // �̵� �ӵ�
    public float rotationSpeed = 300f; // ȸ�� �ӵ�
    public float moveDuration = 0.5f; // �̵��� �ɸ��� �ð� (��)
    private float timer = 0f; // Ÿ�̸�
    public bool conveyorRunning;
    public bool shredderRunning;
    public bool conveyorIsProblem = false;
    public bool shredderIsProblem = false;

    void Start()
    {
        // �����̴��� �ڽ� ��ü���� �����ɴϴ�.
        int childCount = transform.childCount;
        items = new Transform[childCount];
        initialPositions = new Vector3[childCount];
        initialRotations = new Quaternion[childCount];

        for (int i = 0; i < childCount; i++)
        {
            items[i] = transform.GetChild(i);
            // �ʱ� ��ġ�� ȸ�� ����
            initialPositions[i] = items[i].position;
            initialRotations[i] = items[i].rotation;
        }
    }

    public void OnConveyorBtnClkEvent()
    {
        conveyorRunning = !conveyorRunning;
    }

    public void OnShredder()
    {
        shredderRunning = !shredderRunning;
    }

    void Update()
    {
        if (conveyorRunning)
        {
            MoveItems();
        }
    }

    void MoveItems()
    {
        if (items.Length <= 1) return; // �ڽ� ��ü�� ������ ����

        // Ÿ�̸� ������Ʈ
        timer += Time.deltaTime;

        // �̵� ���� ���
        float moveProgress = Mathf.Clamp01(timer / moveDuration);

        // ��� �������� ���ÿ� �̵���ŵ�ϴ�.
        for (int i = 0; i < items.Length; i++)
        {
            Transform currentItem = items[i];
            // ���� �ε��� ���
            int previousIndex = (i - 1 + items.Length) % items.Length; // ���� �ε���

            // ��ǥ ��ġ�� ȸ��
            Vector3 targetPosition = initialPositions[previousIndex];
            Quaternion targetRotation = initialRotations[previousIndex];

            // �̵� �� ȸ��
            currentItem.position = Vector3.Lerp(currentItem.position, targetPosition, moveProgress);
            currentItem.rotation = Quaternion.RotateTowards(currentItem.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        // ��� �������� �̵��� ���ƴٸ� �ε����� ������Ʈ
        if (moveProgress >= 1f)
        {
            // �ε����� ��ȯ�Ͽ� ��ġ ������Ʈ
            Vector3 firstPosition = initialPositions[items.Length - 1]; // ������ �ε����� ��ġ
            Quaternion firstRotation = initialRotations[items.Length - 1]; // ������ �ε����� ȸ��

            for (int i = items.Length - 1; i > 0; i--)
            {
                initialPositions[i] = initialPositions[i - 1];
                initialRotations[i] = initialRotations[i - 1];
            }

            // 0�� �ε����� ������ ��ü�� ��ġ�� ȸ�� ����
            initialPositions[0] = firstPosition;
            initialRotations[0] = firstRotation;

            // Ÿ�̸� �ʱ�ȭ
            timer = 0f; // Ÿ�̸� �ʱ�ȭ
        }
    }
}