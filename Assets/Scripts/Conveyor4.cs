using UnityEngine;
using System.Collections;

public class Conveyor4 : MonoBehaviour
{
    public float speed = 1.0f; // �����̾� ��Ʈ �ӵ�
    public GameObject[] clints; // clint ������Ʈ �迭
    private Vector3[] positions; // clint�� ��ġ �迭

    void Start()
    {
        // clint ������Ʈ�� ã��
        clints = new GameObject[36];
        positions = new Vector3[36];

        for (int i = 0; i < 36; i++)
        {
            string clintName = $"CLINT_{i:D2}"; // CLINT_00 �������� �̸� ����
            clints[i] = GameObject.Find(clintName);
            if (clints[i] != null)
            {
                // clint�� �ʱ� ��ġ ����
                positions[i] = clints[i].transform.position;
            }
            else
            {
                Debug.LogWarning($"Ŭ��Ʈ {clintName}�� ã�� �� �����ϴ�.");
            }
        }

        // ��� Ŭ��Ʈ�� ���ÿ� �̵� ����
        for (int i = 0; i < clints.Length; i++)
        {
            if (clints[i] != null)
            {
                StartCoroutine(MoveClint(i)); // �� Ŭ��Ʈ�� ���� �ڷ�ƾ ����
            }
        }
    }

    void Update()
    {
        // ���� ��ġ�� positions �迭�� ����
        for (int i = 0; i < clints.Length; i++)
        {
            if (clints[i] != null)
            {
                positions[i] = clints[i].transform.position;
            }
        }
    }

    private IEnumerator MoveClint(int index)
    {
        while (true) // ���� ����
        {
            int nextIndex = (index + 1) % clints.Length; // ���� Ŭ��Ʈ �ε���
            GameObject clint = clints[index];
            Vector3 targetPosition = positions[nextIndex];

            // ��ǥ ��ġ�� �̵�
            while (Vector3.Distance(clint.transform.position, targetPosition) > 0.01f)
            {
                clint.transform.position = Vector3.MoveTowards(clint.transform.position, targetPosition, speed * Time.deltaTime);
                yield return null; // ���� �����ӱ��� ���
            }

            // �̵� �Ϸ� �� ��ġ ����
            clint.transform.position = targetPosition;

            // ���� ��ǥ ��ġ ����
            index = nextIndex; // �ε��� ������Ʈ

            // ��� ��� (�̵��� �Ϸ�� ��)
            yield return new WaitForSeconds(0.1f); // ���ϴ� ��� �ð� ���� ����
        }
    }
}
