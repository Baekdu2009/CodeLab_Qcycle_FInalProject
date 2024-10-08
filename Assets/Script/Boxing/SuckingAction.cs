using Unity.VisualScripting;
using UnityEngine;

public class SuckingAction : MonoBehaviour
{
    public bool isSuctionOn; // �� ������ ���� ����
    public bool isAttached; // ��ü�� �پ� �ִ��� ����

    private Rigidbody rb; // �浹�� ��ü�� Rigidbody
    public RobotArmControl robotArmControl; // RobotArmControl �ν��Ͻ�

    private void Start()
    {
        
    }

    private void Update()
    {
        ActionBoolUpdate();

        if (!isSuctionOn && isAttached)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isSuctionOn)
        {
            if (other.tag.Contains("Box"))
            {
                rb = other.GetComponent<Rigidbody>();

                rb.isKinematic = true;
                rb.useGravity = false;

                other.transform.SetParent(transform);

                isAttached = true;
            }
        }
        else
        {
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;

                other.transform.SetParent(null);

                isAttached = false;
            }
        }
    }

    private void ActionBoolUpdate()
    {
        int currentStepIndex = robotArmControl.GetCurrentStepIndex(); // ���� ���� �ε��� ��������
        
        if (currentStepIndex >= 0 && currentStepIndex < robotArmControl.GetSteps().Count)
        {
            isSuctionOn = robotArmControl.GetSteps()[currentStepIndex].actionBool;
        }
    }
}
