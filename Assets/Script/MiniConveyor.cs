using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public float speed = 2.0f; // 이동 속도
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        Vector3 pos = rb.position;

        rb.position += (-1 * transform.right) * speed * Time.fixedDeltaTime;

        rb.MovePosition(pos);
    }
}