using UnityEngine;

public class BoxSensor2 : MonoBehaviour
{
    [Header("Boxing")]
    [SerializeField] GameObject DownLeft;
    [SerializeField] GameObject DownRight;
    [SerializeField] GameObject DownFront;
    [SerializeField] GameObject DownBack;

    private bool hasTriggered = false;
    private void OnTriggerEnter(Collider other)
    {

        if (!hasTriggered && other.CompareTag("Box"))
        {
            hasTriggered = true; // 충돌 발생 시 true로 설정

            if (gameObject.name == "BoxSensor_Down1")
            {
                RotateDownLRColliding(other.gameObject);
            }
            if (gameObject.name == "BoxSensor_Down2")
            {
                RotateDownFBColliding(other.gameObject);
            }
        }
    }


    private void RotateDownLRColliding(GameObject obj)
    {

        DownRight.transform.Rotate(0, -90, 0);
        DownLeft.transform.Rotate(0, -90, 0);
        // Debug.Log($"회전중{DownRight}");
    }
    private void RotateDownFBColliding(GameObject obj)
    {

        DownFront.transform.Rotate(0, 90, 0);
        DownBack.transform.Rotate(0, -90, 0);
    }
}
