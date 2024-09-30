using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class ConveyorExample : MonoBehaviour
{
    [SerializeField] float speed = 0.5f;

    public bool isMoving;
    public List<GameObject> clintList = new List<GameObject>();
    private List<Transform> transformList = new List<Transform>();
    public List<Vector3> vectorList = new List<Vector3>();
    public SerializedDictionary<string, bool> clintStatus = new SerializedDictionary<string, bool>();

    private void Start()
    {
        ClintExtract();
        foreach (var clint in clintList)
        {
            StartCoroutine(ClintMethod(clint));
        }
    }

    private void Update()
    {
        //if (isMoving)
        //{
        //    //foreach (var clint in clintList)
        //    //{
        //    //    StartCoroutine(ClintMove(clint));
        //    //}
        //    StartCoroutine(ClintMove(clintList[1]));
        //    StartCoroutine(ClintMove(clintList[0]));
        //    StartCoroutine(ClintMove(clintList[35]));
        //    isMoving = false; // 한 번만 시작하도록 설정
        //}
    }

    private void ClintExtract()
    {
        // "CLINT" 이름을 포함하는 모든 게임 오브젝트를 찾기
        GameObject[] allClints = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject clint in allClints)
        {
            if (clint.name.Contains("CLINT"))
            {
                string[] splitParts = clint.name.Split('_');
                // clint의 각 좌표를 저장
                if (splitParts.Length > 1 && int.TryParse(splitParts[1], out int number))
                {
                    clintList.Add(clint);
                    transformList.Add(clint.transform); // Transform 추가
                    clintStatus[clint.name] = false; // 초기 상태는 이동하지 않음
                }
            }
        }

        // 올림차순 정렬
        clintList.Sort((a, b) =>
        {
            int aNumber = int.Parse(a.name.Split('_')[1]);
            int bNumber = int.Parse(b.name.Split('_')[1]);
            return aNumber.CompareTo(bNumber); // 기본적으로 올림차순 정렬
        });

        transformList.Sort((a, b) =>
        {
            int aNumber = int.Parse(a.name.Split('_')[1]);
            int bNumber = int.Parse(b.name.Split('_')[1]);
            return aNumber.CompareTo(bNumber); // 기본적으로 올림차순 정렬
        });

        // vectorList를 transformList에 따라 올림차순으로 정렬
        vectorList.Clear(); // 기존 vectorList를 초기화
        foreach (var transform in transformList)
        {
            vectorList.Add(transform.position); // 정렬된 transformList에 따라 position 추가
        }

        // bool 상태도 올림차순으로 정렬
        var sortedMovementStatus = new SerializedDictionary<string, bool>();
        foreach (var clint in clintList)
        {
            sortedMovementStatus[clint.name] = clintStatus[clint.name];
        }
        clintStatus = sortedMovementStatus;

    }

    //private IEnumerator ClintMove(GameObject clint)
    //{
    //    string clintName = clint.name; // 클린트 이름 저장
    //    int currentIndex = clintList.IndexOf(clint) ; // 각 클린트마다 독립적인 인덱스 초기화

    //    while (true) // 무한 루프를 사용하여 계속 이동
    //    {
    //        if (currentIndex < transformList.Count)
    //        {
    //            clintStatus[clintName] = true; // 이동 시작 상태

    //            Vector3 targetPos = transformList[currentIndex].position; // 현재 목표 위치

    //            // 목표 위치까지 이동
    //            while (Vector3.Distance(clint.transform.position, targetPos) > 0.1f)
    //            {
    //                // 클린트를 부드럽게 이동
    //                clint.transform.position = Vector3.MoveTowards(clint.transform.position, targetPos, speed * Time.deltaTime);
    //                yield return null; // 다음 프레임까지 대기
    //                print($"{currentIndex}번째 이동 중");
    //            }

    //            // 정확한 위치 설정
    //            clintStatus[clintName] = false; // 목표 위치 도달 후 상태 업데이트

    //            currentIndex++; // 다음 인덱스로 이동

    //            // 마지막 위치에 도달하면 처음으로 돌아가기
    //            if (currentIndex >= transformList.Count)
    //            {
    //                currentIndex = 0; // 처음 위치로 돌아가기
    //            }
    //        }
    //    }
    //}
    private IEnumerator ClintMethod(GameObject clint)
    {
        int currentIndex = clintList.IndexOf(clint);

        // clint가 clintList에 없을 경우 currentIndex는 -1이 되므로, 0으로 설정
        if (currentIndex < 0 || currentIndex >= clintList.Count)
        {
            currentIndex = 0;
        }

        clint = clintList[currentIndex];

        Transform currentPos = clint.transform;
        print($"현재포지션: {currentIndex}번째 {currentPos.position}");

        // 다음 인덱스를 계산, 범위를 초과할 경우 0으로 리셋
        int nextIndex = (currentIndex + 1) % clintList.Count;
        GameObject nextClint = clintList[nextIndex];
        Transform nextPos = clintList[nextIndex].transform;
        print($"다음포지션: {nextIndex}번째 {nextPos.position}");

        clint.transform.position = Vector3.MoveTowards(currentPos.position, nextPos.position, speed * Time.deltaTime);

        yield return null; // 다음 프레임으로 대기
    }


    public void StartMoving()
    {
        isMoving = true; // 이동 시작
    }

    public void StopMoving()
    {
        isMoving = false; // 이동 중지
    }
}
