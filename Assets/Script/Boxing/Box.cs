using UnityEngine;
using UnityEngine.UIElements;

public class Box : MonoBehaviour
{
    GameObject UpRight;
    GameObject UpLeft;
    GameObject UpFront;
    GameObject UpBack;
    GameObject DownRight;
    GameObject DownLeft;
    GameObject DownFront;
    GameObject DownBack;

  
    private bool hasTriggered1 = false;
    private bool hasTriggered2 = false;
    private bool hasTriggered3 = false;
    

    private void Start()
    {
        UpRight = FindChildObject(gameObject, "UpRight");
        UpLeft = FindChildObject(gameObject, "UpLeft");
        UpFront = FindChildObject(gameObject, "UpFront");
        UpBack = FindChildObject(gameObject, "UpBack");
        DownRight = FindChildObject(gameObject, "DownRight");
        DownLeft = FindChildObject(gameObject, "DownLeft");
        DownFront = FindChildObject(gameObject, "DownFront");
        DownBack = FindChildObject(gameObject, "DownBack"); 
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!hasTriggered1 && other.CompareTag("BoxSensor1"))
        {
            hasTriggered1 = true; // 충돌 발생 시 true로 설정
            RotateDownLRColliding();
        }

        if (!hasTriggered2 && other.CompareTag("BoxSensor2"))
        {
            hasTriggered2 = true; // 충돌 발생 시 true로 설정
            RotateDownFBColliding();
        }

        if(!hasTriggered3 && other.CompareTag("BoxSensor3"))
        {
            hasTriggered3 = true;
            RotateUpRFBCilliding();
        }
    }

    private void RotateDownLRColliding()
    {
        if (DownRight != null)
            DownRight.transform.Rotate(0, -90, 0);

        if (DownLeft != null)
            DownLeft.transform.Rotate(0, -90, 0);

        // Debug.Log($"회전중{DownRight}");
    }
    private void RotateDownFBColliding()
    {
        if (DownFront != null)
            DownFront.transform.Rotate(0, 90, 0);

        if (DownBack != null)
            DownBack.transform.Rotate(0, -90, 0);
    }

    private void RotateUpRFBCilliding()
    {
        if (UpRight != null)
            UpRight.transform.Rotate(0, 90, 0);

        if (UpFront != null)
            UpFront.transform.Rotate(0, -90, 0);

        if (UpBack != null)
            UpBack.transform.Rotate(0, 90, 0);

        if (UpLeft != null)
            UpLeft.transform.Rotate(0, 90, 0);
    }


    private GameObject FindChildObject(GameObject parent, string childName)
    {
        Transform childTransform = parent.transform.Find(childName);
        if (childTransform != null)
        {
            return childTransform.gameObject;
        }
        else
        {
            return null;
        }
    } 
}
