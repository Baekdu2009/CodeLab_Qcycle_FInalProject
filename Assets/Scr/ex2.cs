using UnityEngine;

public class ex2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       /* Transform parentTransform = transform;

        foreach(Transform child in parentTransform)
        {
            Debug.Log("오브젝트 이름 : " + child.name);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        /*transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime, Space.World);*/
        Vector3 localDirection = new Vector3(1, 0, 0);                              // local x 축으로 이동
        Vector3 worldDirection = transform.TransformDirection(localDirection);     // world z 축으로 이동

        transform.position += worldDirection * Time.deltaTime;
    }
}
