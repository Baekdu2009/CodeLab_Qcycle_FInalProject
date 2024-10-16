using UnityEngine;
using System.Collections;

public class ObjectDestroy : MonoBehaviour
{
    public string TagName;
    public bool objectDestroied;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagName))
        {
            Destroy(other.gameObject);
            objectDestroied = true;
            StartCoroutine(ResetObjectDestroyed());
        }
    }

    private IEnumerator ResetObjectDestroyed()
    {
        yield return new WaitForSeconds(0.1f);
        objectDestroied = false;
    }
}