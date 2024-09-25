using UnityEngine;

public class CamerMoving : MonoBehaviour
{

    public Transform target;
    public float distance = 5.0f;
    public float height = 2.0f;
    public float damping = 2.0f;
    private Vector3 offset;

    void Start()
    {
        // 초기 오프셋 계산
        offset = new Vector3(0, height, -distance);
    }

    private void LateUpdate()
    {
        if(target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);
            transform.LookAt(target);
        }
    }
}
