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
        // "CLINT" �̸��� �����ϴ� ��� ���� ������Ʈ�� ã��
        GameObject[] allClints = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject clint in allClints)
        {
            if (clint.name.Contains("CLINT"))
            {
                clintList.Add(clint);
            }
        }

        // clintList�� ��ġ�� clintPosition�� ����
        clintPosition.Clear(); // Ŭ�����Ͽ� ���� �����͸� ����
        for (int i = 0; i < clintList.Count; i++)
        {
            clintPosition.Add(clintList[i].transform.position);
            print($"{i}��° ��ġ: {clintPosition[i]}");
        }
    }

    private void Update()
    {
        
    }

    // A���� B�������� �̵� ����/����
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
    //        // ���� ���� ���
    //        Vector3 direction = EndPosition.position - StartPosition.position;
    //        direction.y = 0; // y ���� ����
    //        direction.x = 0; // x ���� ����

    //        // ���� ������ ���� ���ͷ� ��ȯ
    //        Vector3 normalizedDirection = direction.normalized;

    //        // GameObject �̵� (z �������θ�)
    //        transform.position += normalizedDirection * speed * Time.deltaTime;

    //        // ���� ��ġ�� EndPosition ������ �Ÿ� ���
    //        float distance = (EndPosition.position - transform.position).magnitude;

    //        // EndPosition�� �����ߴ��� Ȯ��
    //        if (distance < 0.1f)
    //        {
    //            // EndPosition�� ������ �� StartPosition���� ���ư�
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
    //    if (other.CompareTag("Metal")) // "Metal" �±� Ȯ��
    //    {
    //        other.transform.parent = this.transform; // �θ�� ����
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (transform.childCount > 0)
    //    {
    //        for (int i = 0; i < transform.childCount; i++)
    //        {
    //            Transform child = transform.GetChild(i); // ù ��° �ڽ� ��������
    //            child.parent = null; // �θ� ����
    //        }
    //    }
    //}
}