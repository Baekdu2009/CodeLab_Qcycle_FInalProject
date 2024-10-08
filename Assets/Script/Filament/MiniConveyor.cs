using UnityEngine;

public class MiniConveyor : MonoBehaviour
{
    public float speed = 2.0f; // 이동 속도
    Rigidbody rb;
    BoxCollider BoxCollider;

    Vector3 colliderCenter;
    Vector3 colliderSize;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        BoxCollider = rb.GetComponent<BoxCollider>();
        rb.isKinematic = true;
        rb.useGravity = false;

        colliderCenter = new Vector3(-1.1f, 0.35f, 0.67f);
        colliderSize = new Vector3(14.7f, 0.05f, 0.3f);

        BoxCollider.center = colliderCenter;
        BoxCollider.size = colliderSize;

    }
    void FixedUpdate()
    {
        Vector3 pos = rb.position;

        rb.position += (-1 * transform.right) * speed * Time.fixedDeltaTime;

        rb.MovePosition(pos);
    }
}