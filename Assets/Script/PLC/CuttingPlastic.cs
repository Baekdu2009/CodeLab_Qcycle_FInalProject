using System.Collections;
using UnityEngine;

public class CuttingPlastic : MonoBehaviour
{
    [SerializeField] GameObject cuttingPrefab;
    [SerializeField] Transform spawnPosition2;
    private void OnCollisionEnter(Collision collision)
    {
        print("¿€µø");
        if (collision.gameObject.tag == "Metal")
        {

            StartCoroutine(shatterAfterDelay(collision.gameObject));
        }
    }

    IEnumerator shatterAfterDelay(GameObject gameObj)
    {
        yield return new WaitForSeconds(1);

        shatter(gameObj);
    }

    private void shatter(GameObject gameObj)
    {

        Vector3 vector3 = spawnPosition2.transform.position;
        Instantiate(cuttingPrefab, vector3, transform.rotation);
        Destroy(gameObj);

    }
}
