using UnityEngine;

public class AGVMovementManual : MonoBehaviour
{
    public float moveSpeed = 5f; // 이동 속도
    public float rotationSpeed = 200f; // 회전 속도

    void Update()
    {
        AGVMoveByKey();
    }

    void AGVMoveByKey()
    {
        // W 키를 눌렀을 때 앞으로 이동
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }

        // A 키를 눌렀을 때 좌측으로 회전
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }

        // D 키를 눌렀을 때 우측으로 회전
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}
