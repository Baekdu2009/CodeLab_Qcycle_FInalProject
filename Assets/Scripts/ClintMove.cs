using System.Collections;
using System.Collections.Generic;
using System.Linq; // Skip 메서드를 사용하기 위해 추가
using UnityEngine;

public class ClintMove : MonoBehaviour
{
    public List<Transform> transformList = new List<Transform>();
    public int currentNum;
    public float speed = 0.5f;

    void Start()
    {
        TransformExtract();
        

        // currentNum이 transformList의 범위 내에 있는지 확인
        if (currentNum >= transformList.Count)
        {
            currentNum = 0; // 범위를 초과하면 0으로 초기화
        }

        // 목표가 두 개 이상일 때만 이동 시작
        //if (transformList.Count > 1)
        //{
        //    StartCoroutine(Move(transform.position, GetNextTargetPosition()));
        //}
    }

    private void TransformExtract()
    {
        GameObject[] allClints = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject clint in allClints)
        {
            if (clint.name.Contains("CLINT"))
            {
                string[] splitParts = clint.name.Split('_');
                if (splitParts.Length > 1 && int.TryParse(splitParts[1], out int number))
                {
                    transformList.Add(clint.transform);

                    // currentNum을 업데이트
                    if (number > currentNum)
                    {
                        currentNum = number;
                    }
                }
            }
        }

        // 전체 리스트를 정렬 (currentNum을 기준으로)
        transformList.Sort((a, b) =>
        {
            int aNum = int.Parse(a.name.Split('_')[1]);
            int bNum = int.Parse(b.name.Split('_')[1]);

            // currentNum을 기준으로 오름차순 정렬
            if (aNum >= currentNum && bNum >= currentNum)
            {
                return aNum.CompareTo(bNum);
            }
            else if (aNum < currentNum && bNum < currentNum)
            {
                return aNum.CompareTo(bNum);
            }
            else if (aNum >= currentNum)
            {
                return 1; // a가 currentNum 이상이면 b 다음
            }
            else
            {
                return -1; // b가 currentNum 이상이면 a 다음
            }
        });
    }

    private Vector3 GetNextTargetPosition()
    {
        // currentNum을 시작으로 transformList를 재정렬
        List<Transform> sortedList = new List<Transform>(transformList);
        sortedList.Sort((a, b) => int.Parse(a.name.Split('_')[1]).CompareTo(int.Parse(b.name.Split('_')[1])));

        // currentNum을 기준으로 리스트를 회전
        int startIndex = sortedList.FindIndex(t => int.Parse(t.name.Split('_')[1]) == currentNum);
        if (startIndex == -1) startIndex = 0; // currentNum이 없을 경우 0으로 설정

        // 리스트를 회전
        List<Transform> rotatedList = new List<Transform>();
        rotatedList.AddRange(sortedList.Skip(startIndex));
        rotatedList.AddRange(sortedList.Take(startIndex));

        int nextNum = (1) % rotatedList.Count; // 다음 인덱스 계산 (순환)

        return rotatedList[nextNum].position; // 다음 목표 위치 반환
    }

    private IEnumerator Move(Vector3 currentPos, Vector3 targetPos)
    {
        while (Vector3.Distance(currentPos, targetPos) > 0.01f)
        {
            currentPos = Vector3.MoveTowards(currentPos, targetPos, speed * Time.deltaTime);
            transform.position = currentPos; // 클린트의 위치 업데이트
            yield return null; // 다음 프레임까지 대기
        }

        // 목표 위치에 도달했을 때 다음 목표로 이동
        currentNum = (currentNum + 1) % transformList.Count; // 다음 인덱스 계산 (순환)
        yield return StartCoroutine(Move(currentPos, GetNextTargetPosition())); // 다음 위치로 이동
    }

    private void Update()
    {
        // Update 메서드에서 이동을 시작하지 않도록 합니다.
        // 모든 이동은 StartCoroutine에서 처리됩니다.
    }
}
