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
    public int[] currentPointIndices; // 각 큐브의 현재 목표 인덱스를 저장할 배열

    private void Start()
    {
        PositionExtract();
        if (points.Count > 0)
        {
            isMoving = true; // 이동 시작
            currentPointIndices = new int[cubeObjs.Count]; // 인덱스 초기화

            // 큐브의 초기 위치를 각자의 위치로 설정
            for (int i = 0; i < cubeObjs.Count; i++)
            {
                if (i < points.Count)
                {
                    cubeObjs[i].transform.position = points[i].position; // 각 큐브를 각 포인트로 초기화
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
                // 이름에서 숫자를 추출
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

        // Transform을 이름의 숫자에 따라 정렬
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
                // 큐브가 자신의 인덱스부터 시작하도록 설정
                if (currentPointIndices[i] < points.Count)
                {
                    ObjectMove(cubeObjs[i], i);
                }
            }
        }
    }

    private void ObjectMove(GameObject cubeObj, int index)
    {
        // 현재 목표 인덱스가 유효한지 확인
        if (currentPointIndices[index] < points.Count)
        {
            Transform targetPos = points[currentPointIndices[index]];

            // 현재 위치와 목표 위치가 같지 않으면 이동
            if (Vector3.Distance(cubeObj.transform.position, targetPos.position) > 0.01f)
            {
                cubeObj.transform.position = Vector3.MoveTowards(cubeObj.transform.position, targetPos.position, speed * Time.deltaTime);
            }
            else
            {
                // 목표 위치에 도착하면 다음 목표로 이동
                currentPointIndices[index]++;

                // 마지막 포인트에 도달했을 때 인덱스를 0으로 리셋
                if (currentPointIndices[index] >= points.Count)
                {
                    currentPointIndices[index] = 0;
                }
            }
        }
    }
}
