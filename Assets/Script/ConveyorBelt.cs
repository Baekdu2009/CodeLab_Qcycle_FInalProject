using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float speed = 5.0f; // �����̾� ��Ʈ �ӵ�
    Rigidbody rb;

    private void Start()
    {
       rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        Vector3 pos = rb.position;
        rb.position += ((-1*transform.right) * speed  * Time.fixedDeltaTime);

        rb.MovePosition(pos);
    }
}
