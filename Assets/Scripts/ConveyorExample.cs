using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ConveyorExample : MonoBehaviour
{
    [SerializeField] float speed = 1.5f;

    [SerializeField] Transform StartPosition;
    [SerializeField] Transform EndPosition;

    public bool isMoving;
    public List<GameObject> clintList = new List<GameObject>();
    List<Vector3> clintPosition = new List<Vector3>();

    private void Start()
    {
        // "CLINT" 이름을 포함하는 모든 게임 오브젝트를 찾기
        GameObject[] allClints = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject clint in allClints)
        {
            if (clint.name.Contains("CLINT"))
            {
                clintList.Add(clint);
            }
        }

        // clintList의 위치를 clintPosition에 저장
        clintPosition.Clear(); // 클리어하여 이전 데이터를 제거
        for (int i = 0; i < clintList.Count; i++)
        {
            clintPosition.Add(clintList[i].transform.position);
            print($"{i}번째 위치: {clintPosition[i]}");
        }
    }

    private void Update()
    {
        
    }

    // A에서 B지점까지 이동 시작/정지
    //public void GoRight()
    //{
    //    isMoving = !isMoving;

    //    if (isMoving)
    //    {
    //        StartCoroutine(MoveRight());
    //    }
    //}

    //private IEnumerator MoveRight()
    //{
    //    while (isMoving)
    //    {
    //        // 방향 벡터 계산
    //        Vector3 direction = EndPosition.position - StartPosition.position;
    //        direction.y = 0; // y 성분 무시
    //        direction.x = 0; // x 성분 무시

    //        // 방향 벡터의 단위 벡터로 변환
    //        Vector3 normalizedDirection = direction.normalized;

    //        // GameObject 이동 (z 방향으로만)
    //        transform.position += normalizedDirection * speed * Time.deltaTime;

    //        // 현재 위치와 EndPosition 사이의 거리 계산
    //        float distance = (EndPosition.position - transform.position).magnitude;

    //        // EndPosition에 도달했는지 확인
    //        if (distance < 0.1f)
    //        {
    //            // EndPosition에 도달한 후 StartPosition으로 돌아감
    //            transform.position = StartPosition.position;
    //        }

    //        yield return new WaitForEndOfFrame();
    //    }
    //}

    //public void PositionCheck()
    //{
    //    print(transform.position);
    //    print(transform.localPosition);
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Metal")) // "Metal" 태그 확인
    //    {
    //        other.transform.parent = this.transform; // 부모로 설정
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (transform.childCount > 0)
    //    {
    //        for (int i = 0; i < transform.childCount; i++)
    //        {
    //            Transform child = transform.GetChild(i); // 첫 번째 자식 가져오기
    //            child.parent = null; // 부모 제거
    //        }
    //    }
    //}
}