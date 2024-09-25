using UnityEngine;

public class Character : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3 characterPosition;

    // inspector���� �� �� �ִ� �迭
    public Vector3[] savedPositions = new Vector3[5];

    // Update is called once per frame
    void Update()
    {
        MoveCharacter();
        LogPosition();
    }
    
    void MoveCharacter()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        transform.position += movement * moveSpeed * Time.deltaTime;
    }

    void LogPosition()
    {
        // ���� ĳ���� ��ġ�� ������
        characterPosition = transform.position;

        // Ư�� Ű�� ������ �� ��ǥ ����
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("��ǥ ��� ��");

            SavePosition();
        }
    }

    void SavePosition()
    {
        // ���� ������ �ε����� ã��
        for(int i = 0; i < savedPositions.Length; i++)
        {
            if (savedPositions[i] == Vector3.zero)
            {
                savedPositions[i] = characterPosition;
                Debug.Log($"��ġ ���� : {savedPositions[i]}");
                return;
            }
        }

        // �迭�� �� á�� ��� �ʱ�ȭ �� ���ο� ��ġ ����
        ResetPositions();
        savedPositions[0] = characterPosition;
               /* // ��ġ�� �����ϴ� ����
        savedPosition = characterPosition; // ���� ��ġ�� savedPosition�� ����

        // ����� ��ġ�� �ֿܼ� ���
        Debug.Log($"��ġ ���� : {savedPosition}");*/
    }

    void ResetPositions()
    {
        // ��� ��ġ�� �⺻������ �ʱ�ȭ
        for(int i = 0; i < savedPositions.Length; i++)
        {
            savedPositions[i] = Vector3.zero;
        }
    }
}

