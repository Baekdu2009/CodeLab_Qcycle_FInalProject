using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorExample : MonoBehaviour
{
    [SerializeField] float speed = 1.5f;
    [SerializeField] Transform StartPosition;
    [SerializeField] Transform MidPosition1;
    [SerializeField] Transform MidPosition2;
    [SerializeField] Transform EndPosition;

    public bool isMoving;
    public List<GameObject> clintList = new List<GameObject>();
    public List<GameObject> pusherList = new List<GameObject>();
    private LineRenderer lineRenderer;

    private void Start()
    {
        // "CLINT" 이름을 포함하는 모든 게임 오브젝트를 찾기
        GameObject[] allClints = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject clint in allClints)
        {
            if (clint.name.Contains("CLINT"))
            {
                clintList.Add(clint);
            }
        }

        // LineRenderer 초기화 및 경로 설정
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 4; // 4개의 점
        lineRenderer.SetPosition(0, StartPosition.position);
        lineRenderer.SetPosition(1, MidPosition1.position);
        lineRenderer.SetPosition(2, MidPosition2.position);
        lineRenderer.SetPosition(3, EndPosition.position);

        // LineRenderer 스타일 설정
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // 기본 재질 사용
        lineRenderer.startColor = Color.green; // 시작 색상
        lineRenderer.endColor = Color.green; // 끝 색상
    }

    private void Update()
    {
        // 업데이트 로직 (필요시 추가)
    }

    // A에서 B지점까지 이동 시작/정지
    public void GoRight()
    {
        isMoving = !isMoving;

        if (isMoving)
        {
            foreach (var clint in clintList)
            {
                StartCoroutine(MoveClint(clint));
            }
        }
    }

    private IEnumerator MoveClint(GameObject clint)
    {
        Vector3[] positions = new Vector3[4];
        lineRenderer.GetPositions(positions); // LineRenderer의 위치 가져오기

        // 경로를 따라 이동
        for (int i = 0; i < positions.Length - 1; i++)
        {
            Vector3 start = positions[i];
            Vector3 end = positions[i + 1];
            float journeyLength = Vector3.Distance(start, end);
            float startTime = Time.time;

            while (clint.transform.position != end)
            {
                float distCovered = (Time.time - startTime) * speed;
                float fractionOfJourney = distCovered / journeyLength;

                clint.transform.position = Vector3.Lerp(start, end, fractionOfJourney);

                // 이동 완료 시
                if (fractionOfJourney >= 1)
                {
                    break;
                }

                yield return null;
            }
        }

        // 이동 완료 후 클리어
        clint.transform.position = positions[positions.Length - 1]; // 마지막 위치로 설정
    }
}
