using UnityEngine;

public class BoxChange : MonoBehaviour
{
    public GameObject ChangeBox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Ãæµ¹ {other.gameObject.name}");
        if (other.CompareTag("Box") && other.gameObject.name.Contains("Box1_Flat"))
        {

            Destroy(other.gameObject);

            BoxChanging();


        }
    }

    private void BoxChanging()
    {
        Quaternion BoxSqawnRotate = Quaternion.Euler(-90, 180, 0);
        Instantiate(ChangeBox, transform.position, BoxSqawnRotate);

    }

    // -121.914 168.596 189.716
}
