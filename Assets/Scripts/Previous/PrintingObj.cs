using UnityEngine;

public class PrintingObj : MonoBehaviour
{
    public GameObject printingObject;
    private int collisionCnt = 0;
    private float changeValue = 0.1f;

    private void Start()
    {
        printingObject.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        printingObject.transform.position = new Vector3(0.5f, 0.3f, 0f);
    }

    private void Update()
    {
        PrintingObjectScaleUpdate();
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Powder"))
    //    {
    //        collisionCnt++;
    //    }
    //}

    void PrintingObjectScaleUpdate()
    {
        Vector3 changeScale = printingObject.transform.localScale;
        Vector3 changePosition = printingObject.transform.localPosition;

        if (changeScale.x < 1f)
        {
            if (collisionCnt >= 20)
            {
                collisionCnt = 0;
                changeScale.x += changeValue;
                changePosition.x -= changeValue / 2;
            }
            printingObject.transform.localPosition = changePosition;
            printingObject.transform.localScale = changeScale;
        }
    }
}
