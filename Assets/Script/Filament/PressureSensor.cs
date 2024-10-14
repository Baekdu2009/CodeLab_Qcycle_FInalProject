using UnityEngine;

public class PressureSensor : MonoBehaviour
{
    [SerializeField] public bool isPressing = false;

    void Start()
    {

    }

    void Update()
    {
      
        // 현재 트리거 영역 내에 Plastic1 태그를 가진 오브젝트가 있는지 확인
         Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity);
         bool foundPlastic = false;

         foreach (var collider in hitColliders)
         {
             if (collider.CompareTag("Plastic2")|| collider.CompareTag("Plastic4"))
             {
                 foundPlastic = true;
                 break;
             }
         }

         // foundPlastic에 따라 isPressing 상태 업데이트
         if (foundPlastic && !isPressing)
         {
             isPressing = true;
             //Debug.Log("Pressing: " + isPressing);
         }
         else if (!foundPlastic && isPressing)
         {
             isPressing = false;
             //Debug.Log("Pressing: " + isPressing);
         }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Plastic2") || other.CompareTag("Plastic4"))
        {
            isPressing = true;
            //Debug.Log("Pressing: " + isPressing);
        }
    }


    void OnTriggerExit(Collider other)
     {
         if (other.CompareTag("Plastic2") || other.CompareTag("Plastic4"))
         {
             isPressing = false;
             //Debug.Log("Pressing: " + isPressing);
         }
     }
}