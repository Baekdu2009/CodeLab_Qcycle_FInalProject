using UnityEngine;

public class BoxConveyor : MonoBehaviour
{
    public float speed = 3.0f;
    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 pos = rb.position;
        rb.position += ((1 * transform.forward) * speed * Time.fixedDeltaTime);

        rb.MovePosition(pos);
    }
}
