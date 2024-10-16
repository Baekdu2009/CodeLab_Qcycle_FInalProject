using UnityEngine;

public class BoxLine : MonoBehaviour
{
    private Animator animator; // Animator를 저장할 변수
    private bool hasFolded = false; // 애니메이션 실행 여부를 추적하는 변수

    private void Start()
    {
        // Animator 컴포넌트를 가져옵니다.
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트의 태그가 "BoxFoldGuid"인지 확인
        if (other.gameObject.CompareTag("BoxFoldGuid") && !hasFolded)
        {
            // 애니메이션 실행
            animator.SetTrigger("Fold"); // Trigger 파라미터를 설정하여 애니메이션 실행
            hasFolded = true; // 애니메이션이 한 번 실행되었음을 기록
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 박스가 그리퍼에서 떨어지면 hasFolded를 reset 할 수도 있습니다.
        if (other.gameObject.CompareTag("BoxFoldGuid"))
        {
            hasFolded = false; // 필요에 따라 리셋
        }
    }
}
