using System.Collections;
using System.Collections.Generic;
using System.Linq; // Skip �޼��带 ����ϱ� ���� �߰�
using UnityEngine;

public class ClintMove : MonoBehaviour
{
    public List<Transform> transformList = new List<Transform>();
    public int currentNum;
    public float speed = 0.5f;

    void Start()
    {
        TransformExtract();
        

        // currentNum�� transformList�� ���� ���� �ִ��� Ȯ��
        if (currentNum >= transformList.Count)
        {
            currentNum = 0; // ������ �ʰ��ϸ� 0���� �ʱ�ȭ
        }

        // ��ǥ�� �� �� �̻��� ���� �̵� ����
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

                    // currentNum�� ������Ʈ
                    if (number > currentNum)
                    {
                        currentNum = number;
                    }
                }
            }
        }

        // ��ü ����Ʈ�� ���� (currentNum�� ��������)
        transformList.Sort((a, b) =>
        {
            int aNum = int.Parse(a.name.Split('_')[1]);
            int bNum = int.Parse(b.name.Split('_')[1]);

            // currentNum�� �������� �������� ����
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
                return 1; // a�� currentNum �̻��̸� b ����
            }
            else
            {
                return -1; // b�� currentNum �̻��̸� a ����
            }
        });
    }

    private Vector3 GetNextTargetPosition()
    {
        // currentNum�� �������� transformList�� ������
        List<Transform> sortedList = new List<Transform>(transformList);
        sortedList.Sort((a, b) => int.Parse(a.name.Split('_')[1]).CompareTo(int.Parse(b.name.Split('_')[1])));

        // currentNum�� �������� ����Ʈ�� ȸ��
        int startIndex = sortedList.FindIndex(t => int.Parse(t.name.Split('_')[1]) == currentNum);
        if (startIndex == -1) startIndex = 0; // currentNum�� ���� ��� 0���� ����

        // ����Ʈ�� ȸ��
        List<Transform> rotatedList = new List<Transform>();
        rotatedList.AddRange(sortedList.Skip(startIndex));
        rotatedList.AddRange(sortedList.Take(startIndex));

        int nextNum = (1) % rotatedList.Count; // ���� �ε��� ��� (��ȯ)

        return rotatedList[nextNum].position; // ���� ��ǥ ��ġ ��ȯ
    }

    private IEnumerator Move(Vector3 currentPos, Vector3 targetPos)
    {
        while (Vector3.Distance(currentPos, targetPos) > 0.01f)
        {
            currentPos = Vector3.MoveTowards(currentPos, targetPos, speed * Time.deltaTime);
            transform.position = currentPos; // Ŭ��Ʈ�� ��ġ ������Ʈ
            yield return null; // ���� �����ӱ��� ���
        }

        // ��ǥ ��ġ�� �������� �� ���� ��ǥ�� �̵�
        currentNum = (currentNum + 1) % transformList.Count; // ���� �ε��� ��� (��ȯ)
        yield return StartCoroutine(Move(currentPos, GetNextTargetPosition())); // ���� ��ġ�� �̵�
    }

    private void Update()
    {
        // Update �޼��忡�� �̵��� �������� �ʵ��� �մϴ�.
        // ��� �̵��� StartCoroutine���� ó���˴ϴ�.
    }
}
