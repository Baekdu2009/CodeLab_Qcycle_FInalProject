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

    private int currentTargetIndex = 0; // ���� ��ǥ �ε���
    private float movementProgress = 0f; // �̵� �����

    void Start()
    {
        // LineRenderer �ʱ�ȭ �� ��� ����
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = positionList.Count; // ��ġ �� ����
        for (int i = 0; i < positionList.Count; i++)
        {
            lineRenderer.SetPosition(i, positionList[i].position);
        }

        // LineRenderer ��Ÿ�� ����
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // �⺻ ���� ���
        lineRenderer.startColor = Color.red; // ���� ����
        lineRenderer.endColor = Color.red; // �� ����

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
                // ��ǥ ��ġ ���
                Vector3 startPosition = positionList[currentTargetIndex].position;
                Vector3 targetPosition = positionList[currentTargetIndex + 1].position;

                // �̵� ����� ������Ʈ
                movementProgress += speed * Time.deltaTime / Vector3.Distance(startPosition, targetPosition);
                pusher.transform.position = Vector3.Lerp(startPosition, targetPosition, movementProgress);

                // ��ǥ�� �����ߴ��� Ȯ��
                if (movementProgress >= 1f)
                {
                    // ȸ�� ���� �߰�
                    if (currentTargetIndex == 0) // midposition1�� ����
                    {
                        pusher.transform.rotation = Quaternion.Euler(45, 0, 0);
                    }
                    else if (currentTargetIndex == 1) // midposition2�� ����
                    {
                        pusher.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }

                    movementProgress = 0f; // ����� �ʱ�ȭ
                    currentTargetIndex++; // ���� ��ǥ �ε����� �̵�
                }
            }
        }
    }
}
