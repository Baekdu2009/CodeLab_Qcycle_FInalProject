using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using static UnityEditor.PlayerSettings;

public class MovetoPointExample : MonoBehaviour
{
    public List<GameObject> cubeObjs = new List<GameObject>();
    public List<Transform> points = new List<Transform>();
    bool isMoving;
    float speed = 1.0f;
    public int[] currentPointIndices; // �� ť���� ���� ��ǥ �ε����� ������ �迭

    private void Start()
    {
        PositionExtract();
        if (points.Count > 0)
        {
            isMoving = true; // �̵� ����
            currentPointIndices = new int[cubeObjs.Count]; // �ε��� �ʱ�ȭ

            // ť���� �ʱ� ��ġ�� ������ ��ġ�� ����
            for (int i = 0; i < cubeObjs.Count; i++)
            {
                if (i < points.Count)
                {
                    cubeObjs[i].transform.position = points[i].position; // �� ť�긦 �� ����Ʈ�� �ʱ�ȭ
                }
            }
        }
    }

    private void PositionExtract()
    {
        GameObject[] allPos = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject pos in allPos)
        {
            if (pos.name.Contains("TestPos"))
            {
                // �̸����� ���ڸ� ����
                string splitName = pos.name;
                string[] splitParts = splitName.Split('_');
                if (splitParts.Length > 1 && int.TryParse(splitParts[1], out int number))
                {
                    points.Add(pos.transform);
                }
            }
        }

        GameObject[] allCube = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject cube in allCube)
        {
            if (cube.name.Contains("Cube"))
            {
                string splitName = cube.name;
                string[] splitParts = splitName.Split("_");
                if (splitParts.Length > 1 && int.TryParse(splitParts[1], out int number))
                {
                    cubeObjs.Add(cube);
                }
            }
        }

        // Transform�� �̸��� ���ڿ� ���� ����
        points.Sort((a, b) =>
        {
            int aNumber = int.Parse(a.name.Split('_')[1]);
            int bNumber = int.Parse(b.name.Split('_')[1]);
            return aNumber.CompareTo(bNumber);
        });
    }

    private void Update()
    {
        if (isMoving)
        {
            for (int i = 0; i < cubeObjs.Count; i++)
            {
                // ť�갡 �ڽ��� �ε������� �����ϵ��� ����
                if (currentPointIndices[i] < points.Count)
                {
                    ObjectMove(cubeObjs[i], i);
                }
            }
        }
    }

    private void ObjectMove(GameObject cubeObj, int index)
    {
        // ���� ��ǥ �ε����� ��ȿ���� Ȯ��
        if (currentPointIndices[index] < points.Count)
        {
            Transform targetPos = points[currentPointIndices[index]];

            // ���� ��ġ�� ��ǥ ��ġ�� ���� ������ �̵�
            if (Vector3.Distance(cubeObj.transform.position, targetPos.position) > 0.01f)
            {
                cubeObj.transform.position = Vector3.MoveTowards(cubeObj.transform.position, targetPos.position, speed * Time.deltaTime);
            }
            else
            {
                // ��ǥ ��ġ�� �����ϸ� ���� ��ǥ�� �̵�
                currentPointIndices[index]++;

                // ������ ����Ʈ�� �������� �� �ε����� 0���� ����
                if (currentPointIndices[index] >= points.Count)
                {
                    currentPointIndices[index] = 0;
                }
            }
        }
    }
}
