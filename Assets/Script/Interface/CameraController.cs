using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;

    private float rotationX = 0f;
    private bool isOn;

    void Update()
    {
        CameraConrolbyUser();
        ControlOn();
    }

    private void CameraConrolbyUser()
    {
        if (isOn)
        {
            // 키보드 입력 처리
            float moveHorizontal = Input.GetAxis("Horizontal"); // A, D 키
            float moveVertical = Input.GetAxis("Vertical"); // W, S 키

            Vector3 moveDirection = new Vector3(moveHorizontal, 0f, moveVertical).normalized;
            Vector3 move = transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime;
            transform.position += move;

            // 마우스 입력 처리
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -90f, 90f); // 상하 회전 제한

            transform.localEulerAngles = new Vector3(rotationX, transform.localEulerAngles.y + mouseX, 0f);
        }
    }

    private void ControlOn()
    {
        if (!isOn && Input.GetKeyDown(KeyCode.Space))
        {
            isOn = true;
        }
        else if (isOn && Input.GetKeyDown(KeyCode.Space))
        {
            isOn = false;
        }
    }
}
