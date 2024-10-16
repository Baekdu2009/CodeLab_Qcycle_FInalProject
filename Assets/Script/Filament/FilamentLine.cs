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
    public float drawDuration = 2f; // �� ���� �׸��� �� �ɸ��� �ð�
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
        // isOn�� true�̰� drawCoroutine�� null�� ���� �� �׸��⸦ ����
        if (isOn && drawCoroutine == null)
        {
            StartDrawing();
        }
        // isOn�� false�� �� StopDrawing ȣ��
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
        // �� ���� �ּ� ó���Ͽ� ���� �ʱ�ȭ���� ����
        // lineRenderer.positionCount = 0; // �� ���� �ּ� ó���ϼ���
    }


    private IEnumerator DrawLine(Transform[] transforms)
    {
        for (int i = 0; i < transforms.Length - 1; i++)
        {
            // isOn�� false�� �Ǹ� �׸��⸦ �ߴ�
            if (!isOn) yield break;

            lineRenderer.positionCount = i + 2; // ���� ��ġ�� ���� ��ġ�� ����
            lineRenderer.SetPosition(i, transforms[i].position);
            lineRenderer.SetPosition(i + 1, transforms[i + 1].position);

            yield return StartCoroutine(DrawSegment(i, i + 1));
        }

      
    }

    private IEnumerator DrawLine(Vector3[] vectors)
    {
        for (int i = 0; i < vectors.Length - 1; i++)
        {
            // isOn�� false�� �Ǹ� �׸��⸦ �ߴ�
            if (!isOn) yield break;

            lineRenderer.positionCount = i + 2; // ���� ��ġ�� ���� ��ġ�� ����
            lineRenderer.SetPosition(i, vectors[i]);
            lineRenderer.SetPosition(i + 1, vectors[i + 1]);

            yield return StartCoroutine(DrawSegment(i, i + 1));
        }

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