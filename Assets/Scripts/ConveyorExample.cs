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
        //    isMoving = false; // �� ���� �����ϵ��� ����
        //}
    }

    private void ClintExtract()
    {
        // "CLINT" �̸��� �����ϴ� ��� ���� ������Ʈ�� ã��
        GameObject[] allClints = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject clint in allClints)
        {
            if (clint.name.Contains("CLINT"))
            {
                string[] splitParts = clint.name.Split('_');
                // clint�� �� ��ǥ�� ����
                if (splitParts.Length > 1 && int.TryParse(splitParts[1], out int number))
                {
                    clintList.Add(clint);
                    transformList.Add(clint.transform); // Transform �߰�
                    clintStatus[clint.name] = false; // �ʱ� ���´� �̵����� ����
                }
            }
        }

        // �ø����� ����
        clintList.Sort((a, b) =>
        {
            int aNumber = int.Parse(a.name.Split('_')[1]);
            int bNumber = int.Parse(b.name.Split('_')[1]);
            return aNumber.CompareTo(bNumber); // �⺻������ �ø����� ����
        });

        transformList.Sort((a, b) =>
        {
            int aNumber = int.Parse(a.name.Split('_')[1]);
            int bNumber = int.Parse(b.name.Split('_')[1]);
            return aNumber.CompareTo(bNumber); // �⺻������ �ø����� ����
        });

        // vectorList�� transformList�� ���� �ø��������� ����
        vectorList.Clear(); // ���� vectorList�� �ʱ�ȭ
        foreach (var transform in transformList)
        {
            vectorList.Add(transform.position); // ���ĵ� transformList�� ���� position �߰�
        }

        // bool ���µ� �ø��������� ����
        var sortedMovementStatus = new SerializedDictionary<string, bool>();
        foreach (var clint in clintList)
        {
            sortedMovementStatus[clint.name] = clintStatus[clint.name];
        }
        clintStatus = sortedMovementStatus;

    }

    //private IEnumerator ClintMove(GameObject clint)
    //{
    //    string clintName = clint.name; // Ŭ��Ʈ �̸� ����
    //    int currentIndex = clintList.IndexOf(clint) ; // �� Ŭ��Ʈ���� �������� �ε��� �ʱ�ȭ

    //    while (true) // ���� ������ ����Ͽ� ��� �̵�
    //    {
    //        if (currentIndex < transformList.Count)
    //        {
    //            clintStatus[clintName] = true; // �̵� ���� ����

    //            Vector3 targetPos = transformList[currentIndex].position; // ���� ��ǥ ��ġ

    //            // ��ǥ ��ġ���� �̵�
    //            while (Vector3.Distance(clint.transform.position, targetPos) > 0.1f)
    //            {
    //                // Ŭ��Ʈ�� �ε巴�� �̵�
    //                clint.transform.position = Vector3.MoveTowards(clint.transform.position, targetPos, speed * Time.deltaTime);
    //                yield return null; // ���� �����ӱ��� ���
    //                print($"{currentIndex}��° �̵� ��");
    //            }

    //            // ��Ȯ�� ��ġ ����
    //            clintStatus[clintName] = false; // ��ǥ ��ġ ���� �� ���� ������Ʈ

    //            currentIndex++; // ���� �ε����� �̵�

    //            // ������ ��ġ�� �����ϸ� ó������ ���ư���
    //            if (currentIndex >= transformList.Count)
    //            {
    //                currentIndex = 0; // ó�� ��ġ�� ���ư���
    //            }
    //        }
    //    }
    //}
    private IEnumerator ClintMethod(GameObject clint)
    {
        int currentIndex = clintList.IndexOf(clint);

        // clint�� clintList�� ���� ��� currentIndex�� -1�� �ǹǷ�, 0���� ����
        if (currentIndex < 0 || currentIndex >= clintList.Count)
        {
            currentIndex = 0;
        }

        clint = clintList[currentIndex];

        Transform currentPos = clint.transform;
        print($"����������: {currentIndex}��° {currentPos.position}");

        // ���� �ε����� ���, ������ �ʰ��� ��� 0���� ����
        int nextIndex = (currentIndex + 1) % clintList.Count;
        GameObject nextClint = clintList[nextIndex];
        Transform nextPos = clintList[nextIndex].transform;
        print($"����������: {nextIndex}��° {nextPos.position}");

        clint.transform.position = Vector3.MoveTowards(currentPos.position, nextPos.position, speed * Time.deltaTime);

        yield return null; // ���� ���������� ���
    }


    public void StartMoving()
    {
        isMoving = true; // �̵� ����
    }

    public void StopMoving()
    {
        isMoving = false; // �̵� ����
    }
}
