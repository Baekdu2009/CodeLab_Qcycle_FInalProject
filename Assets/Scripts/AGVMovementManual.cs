using UnityEngine;

public class AGVMovementManual : MonoBehaviour
{
    public float moveSpeed = 5f; // �̵� �ӵ�
    public float rotationSpeed = 200f; // ȸ�� �ӵ�

    void Update()
    {
        AGVMoveByKey();
    }

    void AGVMoveByKey()
    {
        // W Ű�� ������ �� ������ �̵�
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }

        // A Ű�� ������ �� �������� ȸ��
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }

        // D Ű�� ������ �� �������� ȸ��
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}
