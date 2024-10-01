using System.Collections;
using UnityEngine;

public class Slider : MonoBehaviour
{
    [SerializeField] float speed;

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    [SerializeField] Transform StartPosition;
    [SerializeField] Transform EndPosition;

    public bool isMoving;

    private void Start()
    {
        // �ʱ� ��ġ�� ���� (�ʿ�� ���)
        transform.position = StartPosition.position;
    }

    // A���� B�������� �̵� ����/����
    public void GoRight()
    {
        isMoving = !isMoving;

        if (isMoving)
        {
            StartCoroutine(MoveRight());
        }
    }

    private IEnumerator MoveRight()
    {
        while (isMoving)
        {
            // ���� ���� ���
            Vector3 direction = EndPosition.position - StartPosition.position;
            direction.y = 0; // y ���� ����
            direction.x = 0; // x ���� ����

            // ���� ������ ���� ���ͷ� ��ȯ
            Vector3 normalizedDirection = direction.normalized;

            // GameObject �̵� (z �������θ�)
            transform.position += normalizedDirection * speed * Time.deltaTime;

            // ���� ��ġ�� EndPosition ������ �Ÿ� ���
            float distance = (EndPosition.position - transform.position).magnitude;

            // EndPosition�� �����ߴ��� Ȯ��
            if (distance < 0.1f)
            {
                // EndPosition�� ������ �� StartPosition���� ���ư�
                transform.position = StartPosition.position;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Metal")) // "Metal" �±� Ȯ��
        {
            other.transform.parent = this.transform; // �θ�� ����
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i); // ù ��° �ڽ� ��������
                child.parent = null; // �θ� ����
            }
        }
    }
}
