using UnityEngine;
using UnityEngine.UIElements;

public class Filamentshare : MonoBehaviour
{
    private BoxCollider collider;
    private Vector3 colliderCenter;
    private Vector3 colliderSize;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        colliderCenter = new Vector3(-1.192093e-07f, 0.254f, 9.536743e-07f);
        colliderSize = new Vector3(0.9144003f, 0.05080001f, 0.4572001f);

        collider.size = colliderSize;
        collider.center = colliderCenter;
    }
}

