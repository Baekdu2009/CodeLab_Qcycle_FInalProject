using UnityEngine;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;

public class Conveyor2 : MonoBehaviour
{
    public List<GameObject> clintList = new List<GameObject>();
    private List<Transform> transformList = new List<Transform>();
    public List<Vector3> vectorList = new List<Vector3>();
    public SerializedDictionary<string, bool> clintStatus = new SerializedDictionary<string, bool>();
    private int[] currentPointIndices;

    public float speed = 0.2f;
    private bool isMoving = false;

    private void Start()
    {
        ClintExtract();
        currentPointIndices = new int[clintList.Count]; // �迭 �ʱ�ȭ
    }

    private void ClintExtract()
    {
        GameObject[] allClints = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject clint in allClints)
        {
            if (clint.name.Contains("CLINT"))
            {
                string[] splitParts = clint.name.Split('_');
                if (splitParts.Length > 1 && int.TryParse(splitParts[1], out int number))
                {
                    clintList.Add(clint);
                    transformList.Add(clint.transform);
                    clintStatus[clint.name] = false; // �ʱ� ���´� �̵����� ����
                }
            }
        }

        // �ø����� ����
        clintList.Sort((a, b) => int.Parse(a.name.Split('_')[1]).CompareTo(int.Parse(b.name.Split('_')[1])));
        transformList.Sort((a, b) => int.Parse(a.name.Split('_')[1]).CompareTo(int.Parse(b.name.Split('_')[1])));

        // vectorList�� transformList�� ���� �ø��������� ����
        vectorList.Clear();
        foreach (var transform in transformList)
        {
            vectorList.Add(transform.position);
        }

        // bool ���µ� �ø��������� ����
        var sortedMovementStatus = new SerializedDictionary<string, bool>();
        foreach (var clint in clintList)
        {
            sortedMovementStatus[clint.name] = clintStatus[clint.name];
        }
        clintStatus = sortedMovementStatus;
    }

    private void Update()
    {
        if (isMoving)
        {
            for (int i = 0; i < clintList.Count; i++)
            {
                ClintMove(clintList[i], i);
            }
        }
    }

    private void ClintMove(GameObject gameObject, int index)
    {
        if (currentPointIndices[index] < transformList.Count) // ��ǥ �ε��� üũ
        {
            Transform targetPos = transformList[currentPointIndices[index]];

            // �α׷� ��ǥ ������ ����Ͽ� �����
            Debug.Log($"Clint {index} moving towards: {targetPos.position}");

            // ���� ��ġ�� ��ǥ ��ġ�� ���� ������ �̵�
            if (Vector3.Distance(gameObject.transform.position, targetPos.position) > 0.01f)
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPos.position, speed * Time.deltaTime);
            }
            else
            {
                // ��ǥ ��ġ�� �����ϸ� ���� ��ǥ�� �̵�
                currentPointIndices[index]++;

                // ������ ����Ʈ�� �������� �� �ε����� 0���� ����
                if (currentPointIndices[index] >= transformList.Count)
                {
                    currentPointIndices[index] = 0;
                }
            }
        }
    }

    public void MoveStart()
    {
        isMoving = true;
    }

    public void MoveStop()
    {
        isMoving = false;
    }
}
