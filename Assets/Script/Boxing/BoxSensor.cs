using System;
using UnityEngine;

public class BoxSensor : MonoBehaviour
{
    [Header("Boxing")]
    [Tooltip("�ڽ� �Ѹ��� ���� �뵵")]
    [SerializeField] GameObject boxHander; // BoxHander ������Ʈ

    private bool hasRotated = false; // ȸ�� ���θ� �����ϴ� ����
    private Quaternion initialRotation; // �ʱ� ȸ�� �� ����

    private void OnTriggerEnter(Collider other)
    {
        if (!hasRotated && other.CompareTag("Box"))
        {
            hasRotated = true; // ȸ���� ����Ǿ����� ǥ��

            // boxHander�� x������ -90�� ȸ��
            boxHander.transform.Rotate(-90, 0, 0);
        }
    }


    private void OnTriggerExit(Collider other)
    {
            hasRotated = false;
            // boxHander.transform.Rotate(45, 0, 0);
            boxHander.transform.rotation = initialRotation; // boxHander �ʱ� �� �ҷ�����
        
    }

}

