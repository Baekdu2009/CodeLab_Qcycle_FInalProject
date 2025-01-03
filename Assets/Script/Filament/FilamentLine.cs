using UnityEngine;
using System.Collections;
using System.Linq;

public class FilamentLine : MonoBehaviour
{
    LineRenderer lineRenderer;
    public Transform[] transformPos;
    public Vector3[] vectorPos;

    public int managerNumber;
    public float lineWidth = 0.02f;
    public float drawDuration = 2f; // 각 선을 그리는 데 걸리는 시간
    public bool isOn;
    public bool isWorking;
    public bool isProblem = false;

    private Coroutine drawCoroutine;
  
    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.material.color = Color.red;
    }

    void Update()
    {
        // isOn이 true이고 drawCoroutine이 null일 때만 선 그리기를 시작
        if (isOn && drawCoroutine == null)
        {
            StartDrawing();
        }
        // isOn이 false일 때 StopDrawing 호출
        else if (!isOn && drawCoroutine != null)
        {
            StopDrawing();
        }
    }

    public void StartDrawing()
    {
        if (transformPos.Length > 0)
        {
            drawCoroutine = StartCoroutine(DrawLine(transformPos));
        }
        else if (vectorPos.Length > 0)
        {
            drawCoroutine = StartCoroutine(DrawLine(vectorPos));
        }
    }

    private void StopDrawing()
    {
        if (drawCoroutine != null)
        {
            StopCoroutine(drawCoroutine);
            drawCoroutine = null;
        }
        // 이 줄을 주석 처리하여 선을 초기화하지 않음
        // lineRenderer.positionCount = 0; // 이 줄은 주석 처리하세요
    }


    private IEnumerator DrawLine(Transform[] transforms)
    {
        for (int i = 0; i < transforms.Length - 1; i++)
        {
            // isOn이 false가 되면 그리기를 중단
            if (!isOn) yield break;

            lineRenderer.positionCount = i + 2; // 현재 위치와 다음 위치를 포함
            lineRenderer.SetPosition(i, transforms[i].position);
            lineRenderer.SetPosition(i + 1, transforms[i + 1].position);

            yield return StartCoroutine(DrawSegment(i, i + 1));
        }

      
    }

    private IEnumerator DrawLine(Vector3[] vectors)
    {
        for (int i = 0; i < vectors.Length - 1; i++)
        {
            // isOn이 false가 되면 그리기를 중단
            if (!isOn) yield break;

            lineRenderer.positionCount = i + 2; // 현재 위치와 다음 위치를 포함
            lineRenderer.SetPosition(i, vectors[i]);
            lineRenderer.SetPosition(i + 1, vectors[i + 1]);

            yield return StartCoroutine(DrawSegment(i, i + 1));
        }

        drawCoroutine = null; // 그리기가 완료된 후 코루틴 초기화
    }
    private IEnumerator DrawSegment(int startIndex, int endIndex)
    {
        Vector3 startPos = lineRenderer.GetPosition(startIndex);
        Vector3 endPos = lineRenderer.GetPosition(endIndex);
        float elapsedTime = 0f;

        while (elapsedTime < drawDuration)
        {
            float t = elapsedTime / drawDuration;
            lineRenderer.SetPosition(endIndex, Vector3.Lerp(startPos, endPos, t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        lineRenderer.SetPosition(endIndex, endPos); // 마지막 위치 설정
    }

    public bool LastPointArrive()
    {
        float distance = 0f;

        if (lineRenderer.positionCount > 0)
        {
            Vector3 lastPoint = lineRenderer.GetPosition(lineRenderer.positionCount - 1);

            distance = Vector3.Distance(lastPoint, transformPos[transformPos.Length - 1].position);
          
        }
        return distance < 0.01f;
    }
}