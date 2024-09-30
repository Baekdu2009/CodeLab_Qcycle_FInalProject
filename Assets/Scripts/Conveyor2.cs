using UnityEngine;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;

public class Conveyor2 : MonoBehaviour
{
    public List<GameObject> clintList = new List<GameObject>();
    private List<Transform> transformList = new List<Transform>();
    public List<Vector3> vectorList = new List<Vector3>();
    private SerializedDictionary<string, bool> clintStatus = new SerializedDictionary<string, bool>();
    public int[] currentPointIndices;

    public float speed = 0.2f;
    private bool isMoving = false;

    private void Start()
    {
        ClintExtract();
        currentPointIndices = new int[clintList.Count]; // 배열 초기화
        for (int i = 0; i < clintList.Count; i++)
        {
            currentPointIndices[i] = i;
        }
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
                    clintStatus[clint.name] = false; // 초기 상태는 이동하지 않음
                }
            }
        }

        // 올림차순 정렬
        clintList.Sort((a, b) => int.Parse(a.name.Split('_')[1]).CompareTo(int.Parse(b.name.Split('_')[1])));
        transformList.Sort((a, b) => int.Parse(a.name.Split('_')[1]).CompareTo(int.Parse(b.name.Split('_')[1])));

        // vectorList를 transformList에 따라 올림차순으로 정렬
        vectorList.Clear();
        foreach (var transform in transformList)
        {
            vectorList.Add(transform.position);
        }

        // bool 상태도 올림차순으로 정렬
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
                if (currentPointIndices[i] < transformList.Count)
                {
                    ClintMove(clintList[i], i);
                    print($"{clintList[i].name}가 {i}번째로 이동 중");
                }
            }
        }
    }

    private void ClintMove(GameObject gameObject, int index)
    {
        if (currentPointIndices[index] < transformList.Count)
        {
            Transform targetPos = transformList[currentPointIndices[index]];

            if (Vector3.Distance(gameObject.transform.position, targetPos.position) > 0.01f)
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPos.position, speed * Time.deltaTime);
            }
            else
            {
                currentPointIndices[index]++;

                if(currentPointIndices[index] >= transformList.Count)
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
