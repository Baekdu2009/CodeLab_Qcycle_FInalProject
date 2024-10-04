using UnityEngine;
using System.Collections;

public class FilamentLine : MonoBehaviour
{
    LineRenderer lineRenderer;
    public Transform[] transformPos;
    public Vector3[] vectorPos;

    public float lineWidth = 0.02f;
    public float drawDuration = 0.5f; // �� ���� �׸��� �� �ɸ��� �ð�
    public bool isOn;

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
        // isOn�� ���°� ����Ǹ� �� �׸��⸦ �����ϰų� �ߴ�
        if (isOn && drawCoroutine == null)
        {
            StartDrawing();
        }
        else if (!isOn && drawCoroutine != null)
        {
            StopDrawing();
        }
    }

    private void StartDrawing()
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
        lineRenderer.positionCount = 0; // ���� �ʱ�ȭ
    }

    private IEnumerator DrawLine(Transform[] transforms)
    {
        for (int i = 0; i < transforms.Length - 1; i++)
        {
            lineRenderer.positionCount = i + 2; // ���� ��ġ�� ���� ��ġ�� ����
            lineRenderer.SetPosition(i, transforms[i].position);
            lineRenderer.SetPosition(i + 1, transforms[i + 1].position);

            yield return StartCoroutine(DrawSegment(i, i + 1));
        }

        // ������ ���� �����ϸ� isOn�� false�� ����
        isOn = false;
        drawCoroutine = null; // �׸��Ⱑ �Ϸ�� �� �ڷ�ƾ �ʱ�ȭ
    }

    private IEnumerator DrawLine(Vector3[] vectors)
    {
        for (int i = 0; i < vectors.Length - 1; i++)
        {
            lineRenderer.positionCount = i + 2; // ���� ��ġ�� ���� ��ġ�� ����
            lineRenderer.SetPosition(i, vectors[i]);
            lineRenderer.SetPosition(i + 1, vectors[i + 1]);

            yield return StartCoroutine(DrawSegment(i, i + 1));
        }

        // ������ ���� �����ϸ� isOn�� false�� ����
        isOn = false;
        drawCoroutine = null; // �׸��Ⱑ �Ϸ�� �� �ڷ�ƾ �ʱ�ȭ
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

        lineRenderer.SetPosition(endIndex, endPos); // ������ ��ġ ����
    }
}
