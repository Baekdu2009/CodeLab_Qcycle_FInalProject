using UnityEngine;

public class Powder : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
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
        if (collision.gameObject.CompareTag("PrintingObj") || collision.gameObject.CompareTag("PrintingPart"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PrintingObj") || other.gameObject.CompareTag("PrintingPart"))
        {
            Destroy(gameObject);
        }
    }
}
