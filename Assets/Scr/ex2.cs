using UnityEngine;

public class ex2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform parentTransform = transform;

        foreach(Transform child in parentTransform)
        {
            Debug.Log("오브젝트 이름 : " + child.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
