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
            print("�浹");
        }
    }

    void PrintingObjectScaleUpdate()
    {
        Vector3 changeScale = printingObject.transform.localScale;

        if (changeScale.y < 1f && count < 3 && collisionCnt >= 20)
        {
            collisionCnt = 0; // �浹 �� �ʱ�ȭ
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

        // ũ�⸦ 0���� ���̴� ����
        while (changeScale.y > 0f)
        {
            changeScale.y -= changeValue;
            printingObject.transform.localScale = changeScale;

            yield return new WaitForSeconds(2f); // 2�� ���
        }

        // ũ�Ⱑ 0�� �Ǿ��� �� ���� ũ�� ����
        changeScale.y = 0f;
        printingObject.transform.localScale = changeScale;

        // ������ ��� �ö�ƽ ����
        RemoveRandomPlastics(0.9f); // 90%�� �������� ����
        print("ũ�� ���Ҹ� �Ϸ��߽��ϴ�.");

        count = 0;
        isDetected = false;
        shrinkCoroutine = null;
    }

    void RemoveRandomPlastics(float percentage)
    {
        List<GameObject> allPlastics = new List<GameObject>(GameObject.FindGameObjectsWithTag("Plastic"));
        int totalCount = allPlastics.Count;
        int countToRemove = Mathf.CeilToInt(totalCount * percentage);

        // �ö�ƽ�� �����ϴ� ��쿡�� ����
        for (int i = 0; i < countToRemove && allPlastics.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, allPlastics.Count);
            GameObject plasticToRemove = allPlastics[randomIndex];

            if (plasticToRemove != null)
            {
                Destroy(plasticToRemove); // ������ �ö�ƽ ����
                print("������ �ö�ƽ ���ŵ�"); // �α� ���

                // ����Ʈ���� ����
                allPlastics.RemoveAt(randomIndex);
            }
        }
    }
}
