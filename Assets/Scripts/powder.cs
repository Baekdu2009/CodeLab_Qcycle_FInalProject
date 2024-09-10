using UnityEngine;

public class Powder : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
/*        if (collision.gameObject.CompareTag("Plate") || collision.gameObject.CompareTag("Powder"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                //rb.freezeRotation = true; // 회전 고정
                rb.constraints = RigidbodyConstraints.FreezeAll; // 위치 고정
                //rb.isKinematic = true;
            }
        }*/
        if (collision.gameObject.CompareTag("PrintingObj"))
        {
            Destroy(gameObject);
        }
    }
}
