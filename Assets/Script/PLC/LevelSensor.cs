using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSensor : MonoBehaviour
{
    public GameObject printingObject;
    private int collisionCnt = 0;
    private float changeValue = 0.1f;
    public int count = 0;
    public bool isDetected;

    public List<GameObject> plasticList = new List<GameObject>();
    private Coroutine shrinkCoroutine;

    private void Start()
    {
        printingObject.transform.localScale = new Vector3(0.8f, 0f, 0.8f);
    }

    private void Update()
    {
        PrintingObjectScaleUpdate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Plastic") && shrinkCoroutine == null)
        {
            collisionCnt++;
            plasticList.Add(collision.gameObject);
            print("충돌");
        }
    }

    void PrintingObjectScaleUpdate()
    {
        Vector3 changeScale = printingObject.transform.localScale;

        if (changeScale.y < 1f && count < 3 && collisionCnt >= 20)
        {
            collisionCnt = 0; // 충돌 수 초기화
            changeScale.y += changeValue;
            printingObject.transform.localScale = changeScale;
            count++;

            if (count >= 3 && shrinkCoroutine == null)
            {
                isDetected = true;
                shrinkCoroutine = StartCoroutine(Shrink());
            }
        }
    }

    IEnumerator Shrink()
    {
        Vector3 changeScale = printingObject.transform.localScale;

        // 크기를 0으로 줄이는 과정
        while (changeScale.y > 0f)
        {
            changeScale.y -= changeValue;
            printingObject.transform.localScale = changeScale;

            yield return new WaitForSeconds(2f); // 2초 대기
        }

        // 크기가 0이 되었을 때 최종 크기 보장
        changeScale.y = 0f;
        printingObject.transform.localScale = changeScale;

        // 씬에서 모든 플라스틱 제거
        RemoveRandomPlastics(0.9f); // 90%를 기준으로 제거
        print("크기 감소를 완료했습니다.");

        count = 0;
        isDetected = false;
        shrinkCoroutine = null;
    }

    void RemoveRandomPlastics(float percentage)
    {
        List<GameObject> allPlastics = new List<GameObject>(GameObject.FindGameObjectsWithTag("Plastic"));
        int totalCount = allPlastics.Count;
        int countToRemove = Mathf.CeilToInt(totalCount * percentage);

        // 플라스틱이 존재하는 경우에만 제거
        for (int i = 0; i < countToRemove && allPlastics.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, allPlastics.Count);
            GameObject plasticToRemove = allPlastics[randomIndex];

            if (plasticToRemove != null)
            {
                Destroy(plasticToRemove); // 씬에서 플라스틱 제거
                print("씬에서 플라스틱 제거됨"); // 로그 출력

                // 리스트에서 제거
                allPlastics.RemoveAt(randomIndex);
            }
        }
    }
}
