using UnityEngine;

public class CamerMoving : MonoBehaviour
{
    
    public GameObject Target; // ī�޶� ���� �ٴϴ� Ÿ��

    public float offsetX = 0.0f; // ī�޶� X ��ǥ
    public float offsetY = 0.0f;
    public float offsetZ = 0.0f;

    public float CameraSpeed = 10.0f;       // ī�޶��� �ӵ�
    Vector3 TargetPos;
    
   // Start is called once before the first execution of Update after the MonoBehaviour is created
   public void Start()
    {
      
    }
    private void FixedUpdate()
    {
        // Ÿ���� x, y, z ��ǥ�� ī�޶��� ��ǥ�� ���Ͽ� ī�޶��� ��ġ�� ����
        TargetPos = new Vector3(
            Target.transform.position.x + offsetX,
            Target.transform.position.y + offsetY,
            Target.transform.position.z + offsetZ
            );

        // ī�޶��� �������� �ε巴�� �ϴ� �Լ�(Lerp)
        transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);
        // ī�޶� ������ �ε巴�� �ϱ�
        // transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);
    }
}
