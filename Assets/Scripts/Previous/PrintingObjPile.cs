using UnityEditor.Analytics;
using UnityEngine;

public class PrintingObjPile : MonoBehaviour
{
    public Transform nozzle;
    public Transform printingPos;
    public GameObject objTrigger;
    public GameObject printingPrefab;
    public GameObject powder;
    int powderCnt;
    

    void Start()
    {
        
    }

    
    void Update()
    {
        TransformPosition(printingPos);
        TransformPosition(objTrigger);
    }

    Vector3 TransformPosition(Transform trans)
    {
        Vector3 pos = trans.position;
        pos.x = nozzle.position.x;
        pos.y = nozzle.position.y - 0.3f;
        pos.z = nozzle.position.z;
        trans.position = pos;
        return pos;
    }
    Vector3 TransformPosition(GameObject obj)
    {
        Vector3 pos = obj.transform.position;
        pos.x = printingPos.position.x;
        pos.y = 0.3f;
        pos.z = printingPos.position.z;
        obj.transform.position = pos;
        return pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Powder")
        {
            if(powderCnt <= 19)
            {
                powderCnt++;
                print(powderCnt);
            }
            else if(powderCnt > 19)
            {
                GameObject obj = Instantiate(printingPrefab);
                obj.transform.position = printingPos.position;
                powderCnt = 0;
            }
        }
    }
}
