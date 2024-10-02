using UnityEngine;

public class PlasticSpawn : MonoBehaviour
{
    [SerializeField] GameObject[] prefabs;

    public void OnSpawnObjectBtnClkEvent()
    {
        int rand = Random.Range(0, prefabs.Length);
        GameObject newObj = Instantiate(prefabs[rand]);

        newObj.transform.SetParent(transform);
        newObj.transform.position = transform.position;

    }
}
