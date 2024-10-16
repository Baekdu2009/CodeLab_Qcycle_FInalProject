using UnityEngine;

public class BottleDestroy : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Material"))
        {
            Destroy(other.gameObject);
        }
    }
}
