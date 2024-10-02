using UnityEngine;

public class ConveyorMovement : MonoBehaviour
{
    public Transform[] items; // �����̴��� �ڽ� ��ü��
    public Vector3[] initialPositions; // �ʱ� ��ġ �迭
    public Quaternion[] initialRotations; // �ʱ� ȸ�� �迭
    public float moveSpeed = 2f; // �̵� �ӵ�
    public float rotationSpeed = 50f; // ȸ�� �ӵ�
    public float moveDuration = 2f; // �̵��� �ɸ��� �ð� (��)
    private float timer = 0f; // Ÿ�̸�
    public bool isRunning;

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
        isRunning = !isRunning; // ��ư Ŭ�� �� �����̴� ����
    }

    void Update()
    {
        if (isRunning)
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