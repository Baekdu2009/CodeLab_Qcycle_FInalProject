using UnityEngine;

public class BoxChange : MonoBehaviour
{
    public GameObject ChangeBox;

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log($"Ãæµ¹ {other.gameObject.name}");
        if (other.gameObject.name.Contains("Flat"))
        {
            Destroy(other.gameObject);
            BoxChanging(other.transform);
        }
    }

    private void BoxChanging(Transform Location)
    {
        Quaternion BoxSqawnRotate = Quaternion.Euler(-90, 180, 0);
        Instantiate(ChangeBox, Location.position, BoxSqawnRotate);
    }
    // -121.914 168.596 189.716
}
