using UnityEngine;

public class ObjectDestroy : MonoBehaviour
{
    public string TagName;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains(TagName))
        {
            Destroy(other.gameObject);
        }
    }
}