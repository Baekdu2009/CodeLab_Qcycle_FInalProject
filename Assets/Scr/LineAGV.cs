using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineAGV : MonoBehaviour
{
    public LineRenderer lineRenderer; // LineRenderer ����
    public float speed = 2.0f; // �̵� �ӵ�
    private List<int> pointIndices = new List<int>(); // ������ �� �ε��� ����Ʈ
    private int currentPointIndex = 0; // ���� �� �ε���
    private float t = 0; // ���� ���� �̵��ϴ� ����
    private bool isMovingForward = true; // �̵� ����


    void Start()
    {
        // �ʱ� �� �ε����� ����Ʈ�� �߰�
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            pointIndices.Add(i);
        }
    }

    void Update()
    {
        if (lineRenderer.positionCount > 1) // LineRenderer�� ���� �ִ� ���
        {
            // ���� ���� ���� ���� ������
            Vector3 startPos = lineRenderer.GetPosition(pointIndices[currentPointIndex]);
            Vector3 endPos = lineRenderer.GetPosition(pointIndices[(currentPointIndex + (isMovingForward ? 1 : -1) + lineRenderer.positionCount) % lineRenderer.positionCount]);

            // ���� �� �� ���̿��� ������Ʈ �̵�
            t += Time.deltaTime * speed / Vector3.Distance(startPos, endPos); // ���� ���
            transform.position = Vector3.Lerp(startPos, endPos, t); // ���� ���� �̵�

            // ���� ������ �̵�
            if (t >= 1)
            {
                t = 0; // ���� �ʱ�ȭ

                // �̵� ���⿡ ���� �ε��� ����
                if (isMovingForward)
                {
                    currentPointIndex++; // ���� ������ �̵�

                    // ������ ���� �������� ��
                    if (currentPointIndex >= pointIndices.Count - 1)
                    {
                        isMovingForward = false; // �ݴ� �������� �̵�
                    }
                }
                else
                {
                    currentPointIndex--; // ���� ������ �̵�

                    // ó�� ���� �������� ��
                    if (currentPointIndex <= 0)
                    {
                        isMovingForward = true; // �ٽ� ������ �̵�


                    }
                }
            }
        }
    }
}