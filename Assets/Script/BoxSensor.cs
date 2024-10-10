using System;
using UnityEngine;

public class BoxSensor : MonoBehaviour
{
    [Header("Boxing")]
    [Tooltip("�ڽ� �Ѹ��� ���� �뵵")]
    [SerializeField] GameObject boxHander; // BoxHander ������Ʈ
    [SerializeField] GameObject UpRight; // UpRight ������Ʈ
    [SerializeField] GameObject UpLeft; // UpLeft ������Ʈ
    [SerializeField] GameObject UpFront; // UpFront ������Ʈ
    [SerializeField] GameObject UpBack; // UpBack ������Ʈ
    [Space(10f)]
    [SerializeField] GameObject BoxTape;


    private bool hasRotated = false; // ȸ�� ���θ� �����ϴ� ����
    private Quaternion initialRotation; // �ʱ� ȸ�� �� ����
    private bool hasTaping = false; // ������ ���θ� �����ϴ� ����
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
                break;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            hasRotated = false;
            // boxHander.transform.Rotate(45, 0, 0);
            boxHander.transform.rotation = initialRotation; // boxHander �ʱ� �� �ҷ�����
        }
    }
}

