using UnityEngine;

public class BoxSensor : MonoBehaviour
{
    [SerializeField] GameObject UpRight; // ȸ���� ������Ʈ
    [SerializeField] GameObject boxHander; // BoxHander ������Ʈ
    [SerializeField] GameObject UpLeft; // UpLeft ������Ʈ
    [SerializeField] GameObject UpFront; // UpFront ������Ʈ
    [SerializeField] GameObject UpBack; // UpBack ������Ʈ

    private bool hasRotated = false; // ȸ�� ���θ� �����ϴ� ����

    private void OnTriggerEnter(Collider other)
    {
        if (!hasRotated && other.CompareTag("Box"))
        {
            hasRotated = true; // ȸ���� ����Ǿ����� ǥ��

            // boxHander�� x������ -90�� ȸ��
            boxHander.transform.Rotate(-90, 0, 0);

            // UpRight�� y������ 90�� ȸ��
            UpRight.transform.Rotate(0, 90, 0);
            UpFront.transform.Rotate(0, -90, 0);
            UpBack.transform.Rotate(0, 90, 0);

            // UpLeft ȸ�� ó��
            RotateUpLeftIfColliding();

            Debug.Log("ȸ�� �Ϸ�");
        }
    }

    private void RotateUpLeftIfColliding()
    {
        // boxHander�� UpLeft�� �浹 ����
        Collider[] colliders = Physics.OverlapBox(boxHander.transform.position, boxHander.transform.localScale / 2, boxHander.transform.rotation);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject == UpLeft)
            {
                // UpLeft�� boxHander�� �浹���� ��� x������ 90�� ȸ��
                UpLeft.transform.Rotate(0, 90, 0);
                Debug.Log("UpLeft ȸ�� �Ϸ�");
                break;
            }
        }
    }
}
