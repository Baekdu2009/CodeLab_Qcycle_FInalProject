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
        // "CLINT" �̸��� �����ϴ� ��� ���� ������Ʈ�� ã��
        GameObject[] allClints = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject clint in allClints)
        {
            if (clint.name.Contains("CLINT"))
            {
                clintList.Add(clint);
            }
        }

        // LineRenderer �ʱ�ȭ �� ��� ����
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 4; // 4���� ��
        lineRenderer.SetPosition(0, StartPosition.position);
        lineRenderer.SetPosition(1, MidPosition1.position);
        lineRenderer.SetPosition(2, MidPosition2.position);
        lineRenderer.SetPosition(3, EndPosition.position);

        // LineRenderer ��Ÿ�� ����
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // �⺻ ���� ���
        lineRenderer.startColor = Color.green; // ���� ����
        lineRenderer.endColor = Color.green; // �� ����
    }

    private void Update()
    {
        // ������Ʈ ���� (�ʿ�� �߰�)
    }

    // A���� B�������� �̵� ����/����
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
        lineRenderer.GetPositions(positions); // LineRenderer�� ��ġ ��������

        // ��θ� ���� �̵�
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

                // �̵� �Ϸ� ��
                if (fractionOfJourney >= 1)
                {
                    break;
                }

                yield return null;
            }
        }

        // �̵� �Ϸ� �� Ŭ����
        clint.transform.position = positions[positions.Length - 1]; // ������ ��ġ�� ����
    }
}
