using Unity.VisualScripting;
using UnityEngine;

public class SuckingAction : MonoBehaviour
{
    public bool isSuctionOn; // 각 스텝의 흡입 상태
    public bool isAttached; // 물체가 붙어 있는지 여부

    private Rigidbody rb; // 충돌한 물체의 Rigidbody
    public RobotArmControl robotArmControl; // RobotArmControl 인스턴스

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
        int currentStepIndex = robotArmControl.GetCurrentStepIndex(); // 현재 스텝 인덱스 가져오기
        
        if (currentStepIndex >= 0 && currentStepIndex < robotArmControl.GetSteps().Count)
        {
            isSuctionOn = robotArmControl.GetSteps()[currentStepIndex].actionBool;
        }
    }
}
