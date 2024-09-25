using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.Animations;

public class LineAGV1 : MonoBehaviour
{
    public static LineAGV1 instance;    // �ν��Ͻ� ����(�ܺο��� ��밡�� ���� �ο�)
    public LineRenderer lineRenderer1; // LineRenderer1 ����
    public GameObject AGV1;
    public Transform target1; // ��ǥ ��ġ (Filament)
    public Transform target2; // ��ǥ ��ġ (3D Print)
    public Button moveButton1;
    public Button moveButton2;
    public float moveSpeed = 2.0f; // �̵� �ӵ�
    public bool isMovingForward = true; // �̵� ����
    private List<int> pointIndices = new List<int>(); // ������ �� �ε��� ����Ʈ
    private int currentPointIndex = 0; // ���� �� �ε���
    private float t = 0; // ���� ���� �̵��ϴ� ����

    public void Awake()            // start ���� ���� ����
    {
        if (instance == null)
            instance = this;
    }


    void Start()
    {
        /*Vector3 pos;
        pos = this.gameObject.transform.position;
        // Debug.Log(pos);*/

        /*  // ��ư Ŭ���� ���� �ٸ� ��ġ�� �̵�
          moveButton1.onClick.AddListener(() => MoveAGV1ToTarget(target1));
          moveButton2.onClick.AddListener(() => MoveAGV1ToTarget(target2));
  */
        // �ʱ� �� �ε����� ����Ʈ�� �߰�
        for (int i = 0; i < lineRenderer1.positionCount; i++)
        {
            pointIndices.Add(i);
        }


    }

    void Update()
    {

        if (isMovingForward && target1 != null)
        {
            if (lineRenderer1.positionCount > 1) // LineRenderer�� ���� �ִ� ���
            {
                // ���� ���� ���� ���� ������
                Vector3 startPos = lineRenderer1.GetPosition(pointIndices[currentPointIndex]);
                transform.LookAt(startPos);

                // ���� �� �ε��� ���
                int nextPointIndex = currentPointIndex + 1;

                //������ ������ üũ
                if (nextPointIndex < pointIndices.Count)
                {
                    Vector3 endPos = lineRenderer1.GetPosition(pointIndices[nextPointIndex]);
                    transform.LookAt(endPos);

                    // ���� �� �� ���̿��� ������Ʈ �̵�      ���� ���
                    t += Time.deltaTime * moveSpeed / Vector3.Distance(startPos, endPos);
                    transform.position = Vector3.Lerp(startPos, endPos, t); // �� ���� �̵�

                    // ���� ������ �̵�
                    if (t >= 1)
                    {
                        t = 0; // ���� �ʱ�ȭ
                        currentPointIndex++; // ���� ������ �̵�
                    }
                }
                else
                {
                    // ������ ���� �������� ��
                    isMovingForward = false; // �̵� ����
                    Debug.Log("AGV1 ����");
                }
                // ������ ������ üũ
                if (nextPointIndex < pointIndices.Count)
                {
                    Vector3 endPos = lineRenderer1.GetPosition(pointIndices[nextPointIndex]);
                }
            }
        }
    }
    /*public void MoveAGV1ToTarget(Transform target)
 {
     target1 = target; // ��ǥ ��ġ ����
     isMovingForward = true; // ��ư Ŭ�� �� �̵� ����
 }*/
}