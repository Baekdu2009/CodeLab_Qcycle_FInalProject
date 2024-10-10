using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;

    private float rotationX = 0f;

    void Update()
    {
        // Ű���� �Է� ó��
        float moveHorizontal = Input.GetAxis("Horizontal"); // A, D Ű
        float moveVertical = Input.GetAxis("Vertical"); // W, S Ű

        Vector3 moveDirection = new Vector3(moveHorizontal, 0f, moveVertical).normalized;
        Vector3 move = transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime;
        transform.position += move;

        // ���콺 �Է� ó��
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // ���� ȸ�� ����

        transform.localEulerAngles = new Vector3(rotationX, transform.localEulerAngles.y + mouseX, 0f);
    }
}
