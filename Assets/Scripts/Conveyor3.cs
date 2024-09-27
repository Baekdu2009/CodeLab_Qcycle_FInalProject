using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class Conveyor3 : MonoBehaviour
{

    public List<GameObject> clintList = new List<GameObject>();
    private List<Transform> transformList = new List<Transform>();
    public List<Vector3> vectorList = new List<Vector3>();
    private SerializedDictionary<string, bool> clintStatus = new SerializedDictionary<string, bool>();

    public float speed = 0.2f;
    private bool isMoving = false;

    
    void Start()
    {
        ClintExtract();
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
    
    void Update()
    {
        for (int i = 0; i < clintList.Count; i++)
        {
            ClintMove(i);
        }
    }

    void ClintMove(int index)
    {
        if (index < transformList.Count - 1)
        {
            Transform targetPos = transformList[index + 1];
            Transform currentPos = transformList[index];

            if (Vector3.Distance(clintList[index].transform.position, targetPos.position) > 0.01f)
            {
                clintList[index].transform.position = Vector3.MoveTowards(clintList[index].transform.position, targetPos.position, speed * Time.deltaTime);
            }
            else
            {
                currentPos = targetPos;
            }
        }
        else if (index > transformList.Count - 1)
        {
            index = 0;
        }

    }
}
