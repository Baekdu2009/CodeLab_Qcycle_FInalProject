/*using UnityEngine;

public class BoxSqawn : MonoBehaviour
{
    [Header("Boxing")]
    [SerializeField] GameObject DownLeft;
    [SerializeField] GameObject DownRight;
    [SerializeField] GameObject DownFront;
    [SerializeField] GameObject DownBack;


    [Header("BoxSqwan")]
    [SerializeField] GameObject BoxSqawnprefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DownRight = transform.GetChild(4).gameObject;
        DownLeft = transform.GetChild(5).gameObject;
        DownBack = transform.GetChild(6).gameObject;
        DownFront = transform.GetChild(7).gameObject;


    }


    private void BoxSqawn()
    {
        Quaternion BoxSqawnRotate = Quaternion.Euler(-90, 180, 0);

        GameObject Box = Instantiate(BoxSqawnprefab, transform.position, BoxSqawnRotate);

    }

}*/

using UnityEngine;

public class BoxSqawn : MonoBehaviour
{

    [Header("BoxSqawn")]
    [SerializeField] GameObject BoxSqawnprefab;

    void Start()
    {
        BoxSqawnPrefab();
    }

    private void BoxSqawnPrefab()
    {
        Quaternion BoxSqawnRotate = Quaternion.Euler(-90, 180, 0);
        Instantiate(BoxSqawnprefab, transform.position, BoxSqawnRotate);

        // 자식 오브젝트를 찾아서 할당
      
    }

    
}


