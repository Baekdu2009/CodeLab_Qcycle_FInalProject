using UnityEngine;

public class ex2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       /* Transform parentTransform = transform;

        foreach(Transform child in parentTransform)
        {
            Debug.Log("������Ʈ �̸� : " + child.name);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        /*transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime, Space.World);*/
        Vector3 localDirection = new Vector3(1, 0, 0);                              // local x ������ �̵�
        Vector3 worldDirection = transform.TransformDirection(localDirection);     // world z ������ �̵�

        transform.position += worldDirection * Time.deltaTime;
    }
}
