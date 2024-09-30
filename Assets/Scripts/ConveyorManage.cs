using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ConveyorManage : MonoBehaviour
{
    [SerializeField] float speed = 0.5f;
    [SerializeField] List<Transform> positionList = new List<Transform>();

    public bool isMoving;
    public List<GameObject> pusherList = new List<GameObject>();
    public List<GameObject> clintList = new List<GameObject>();
    private LineRenderer lineRenderer;

    private int currentTargetIndex = 0; // 현재 목표 인덱스
    private float movementProgress = 0f; // 이동 진행률

    void Start()
    {
        // LineRenderer 초기화 및 경로 설정
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = positionList.Count; // 위치 수 설정
        for (int i = 0; i < positionList.Count; i++)
        {
            lineRenderer.SetPosition(i, positionList[i].position);
        }

        // LineRenderer 스타일 설정
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // 기본 재질 사용
        lineRenderer.startColor = Color.red; // 시작 색상
        lineRenderer.endColor = Color.red; // 끝 색상

        GameObject[] allClints = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject clint in allClints)
        {
            if (clint.name.Contains("CLINT"))
            {
                clintList.Add(clint);
            }
        }
    }

    void Update()
    {
        if (isMoving)
        {
            MovePushers();
        }
    }
    public void BoolBtn()
    {
        isMoving = !isMoving;
    }

    private void MovePushers()
    {
        foreach (var pusher in pusherList)
        {
            if (currentTargetIndex < positionList.Count - 1)
            {
                // 목표 위치 계산
                Vector3 startPosition = positionList[currentTargetIndex].position;
                Vector3 targetPosition = positionList[currentTargetIndex + 1].position;

                // 이동 진행률 업데이트
                movementProgress += speed * Time.deltaTime / Vector3.Distance(startPosition, targetPosition);
                pusher.transform.position = Vector3.Lerp(startPosition, targetPosition, movementProgress);

                // 목표에 도달했는지 확인
                if (movementProgress >= 1f)
                {
                    // 회전 로직 추가
                    if (currentTargetIndex == 0) // midposition1에 도달
                    {
                        pusher.transform.rotation = Quaternion.Euler(45, 0, 0);
                    }
                    else if (currentTargetIndex == 1) // midposition2에 도달
                    {
                        pusher.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }

                    movementProgress = 0f; // 진행률 초기화
                    currentTargetIndex++; // 다음 목표 인덱스로 이동
                }
            }
        }
    }
}
