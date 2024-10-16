using UnityEngine;

public class BoxLine : MonoBehaviour
{
    private Animator animator; // Animator�� ������ ����
    private bool hasFolded = false; // �ִϸ��̼� ���� ���θ� �����ϴ� ����

    private void Start()
    {
        // Animator ������Ʈ�� �����ɴϴ�.
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� �±װ� "BoxFoldGuid"���� Ȯ��
        if (other.gameObject.CompareTag("BoxFoldGuid") && !hasFolded)
        {
            // �ִϸ��̼� ����
            animator.SetTrigger("Fold"); // Trigger �Ķ���͸� �����Ͽ� �ִϸ��̼� ����
            hasFolded = true; // �ִϸ��̼��� �� �� ����Ǿ����� ���
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �ڽ��� �׸��ۿ��� �������� hasFolded�� reset �� ���� �ֽ��ϴ�.
        if (other.gameObject.CompareTag("BoxFoldGuid"))
        {
            hasFolded = false; // �ʿ信 ���� ����
        }
    }
}
