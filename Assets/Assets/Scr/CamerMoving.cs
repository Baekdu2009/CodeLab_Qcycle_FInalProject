using UnityEngine;

public class CamerMoving : MonoBehaviour
{
    
    public GameObject Target; // 카메라가 따라 다니는 타겟

    public float offsetX = 0.0f; // 카메라 X 좌표
    public float offsetY = 0.0f;
    public float offsetZ = 0.0f;

    public float CameraSpeed = 10.0f;       // 카메라의 속도
    Vector3 TargetPos;
    
   // Start is called once before the first execution of Update after the MonoBehaviour is created
   public void Start()
    {
      
    }
    private void FixedUpdate()
    {
        // 타겟의 x, y, z 좌표에 카메라의 좌표를 더하여 카메라의 위치를 결정
        TargetPos = new Vector3(
            Target.transform.position.x + offsetX,
            Target.transform.position.y + offsetY,
            Target.transform.position.z + offsetZ
            );

        // 카메라의 움직임을 부드럽게 하는 함수(Lerp)
        transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);
        // 카메라 움직임 부드럽게 하기
        // transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);
    }
}
