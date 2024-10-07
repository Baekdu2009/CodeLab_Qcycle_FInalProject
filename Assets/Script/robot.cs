using UnityEngine;

public class robot : MonoBehaviour
{
    public GameObject box; // ���� �ڽ�
    public Vector3 foldedPosition; // �����ִ� ������ ��ġ
    public Vector3 unfoldedPosition; // ���� ������ ��ġ
    private bool isHolding = false; // �ڽ��� ��� �ִ��� ����

    void Start()
    {
        // �����ִ� ��ġ�� ���� ��ġ ����
        foldedPosition = box.transform.position; // ���� ��ġ�� �����ִ� ��ġ�� ����
        unfoldedPosition = new Vector3(foldedPosition.x, foldedPosition.y + 1f, foldedPosition.z); // ����: Y������ 1��ŭ �ø���
        box.transform.position = foldedPosition; // �ʱ� ���¸� �����ִ� ���·� ����
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == box && !isHolding)
        {
            // �ڽ��� �׸����� �ڽ����� ����
            box.transform.SetParent(transform);
            isHolding = true; // �ڽ��� ��� �ִ� ���·� ����

            // �ڽ��� ���
            UnfoldBox();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == box && isHolding)
        {
            // �ڽ��� �θ� ����
            box.transform.SetParent(null);
            isHolding = false; // �ڽ��� ��� ���� ���� ���·� ����
        }
    }

    void UnfoldBox()
    {
        // �ڽ��� ���� ��ġ�� �̵�
        box.transform.position = unfoldedPosition;
    }
}
