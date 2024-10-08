using UnityEngine;

public class Conveyor : MonoBehaviour
{
    private Transform[] items; // 슬라이더의 자식 개체들
    private Vector3[] initialPositions; // 초기 위치 배열
    private Quaternion[] initialRotations; // 초기 회전 배열
    public float moveSpeed = 500f; // 이동 속도
    public float rotationSpeed = 300f; // 회전 속도
    public float moveDuration = 0.5f; // 이동에 걸리는 시간 (초)
    private float timer = 0f; // 타이머
    public bool conveyorRunning;
    public bool shredderRunning;
    public bool conveyorIsProblem = false;
    public bool shredderIsProblem = false;

    void Start()
    {
        // 슬라이더의 자식 개체들을 가져옵니다.
        int childCount = transform.childCount;
        items = new Transform[childCount];
        initialPositions = new Vector3[childCount];
        initialRotations = new Quaternion[childCount];

        for (int i = 0; i < childCount; i++)
        {
            items[i] = transform.GetChild(i);
            // 초기 위치와 회전 저장
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
        if (items.Length <= 1) return; // 자식 개체가 없으면 종료

        // 타이머 업데이트
        timer += Time.deltaTime;

        // 이동 비율 계산
        float moveProgress = Mathf.Clamp01(timer / moveDuration);

        // 모든 아이템을 동시에 이동시킵니다.
        for (int i = 0; i < items.Length; i++)
        {
            Transform currentItem = items[i];
            // 이전 인덱스 계산
            int previousIndex = (i - 1 + items.Length) % items.Length; // 이전 인덱스

            // 목표 위치와 회전
            Vector3 targetPosition = initialPositions[previousIndex];
            Quaternion targetRotation = initialRotations[previousIndex];

            // 이동 및 회전
            currentItem.position = Vector3.Lerp(currentItem.position, targetPosition, moveProgress);
            currentItem.rotation = Quaternion.RotateTowards(currentItem.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        // 모든 아이템이 이동을 마쳤다면 인덱스를 업데이트
        if (moveProgress >= 1f)
        {
            // 인덱스를 순환하여 위치 업데이트
            Vector3 firstPosition = initialPositions[items.Length - 1]; // 마지막 인덱스의 위치
            Quaternion firstRotation = initialRotations[items.Length - 1]; // 마지막 인덱스의 회전

            for (int i = items.Length - 1; i > 0; i--)
            {
                initialPositions[i] = initialPositions[i - 1];
                initialRotations[i] = initialRotations[i - 1];
            }

            // 0번 인덱스에 마지막 개체의 위치와 회전 저장
            initialPositions[0] = firstPosition;
            initialRotations[0] = firstRotation;

            // 타이머 초기화
            timer = 0f; // 타이머 초기화
        }
    }
}