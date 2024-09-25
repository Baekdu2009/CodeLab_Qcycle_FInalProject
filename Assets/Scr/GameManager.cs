

using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public GameObject cylinderPrefab; // �Ǹ��� ������
    public float scaleIncreaseSpeed; // ���� ���� �ӵ�


    void Start()
    {
        // (0, 0, 0) ��ġ�� Z������ 90�� ȸ���Ͽ� �Ǹ��� ����
        Quaternion rotation = Quaternion.Euler(0, 0, 90); // Z������ 90�� ȸ��
        
        // GameManager�� ��ġ���� �Ǹ��� ����
        GameObject cylinder = Instantiate(cylinderPrefab, transform.position, rotation);

        // CylinderController ��ũ��Ʈ�� �߰��Ͽ� �ʱ�ȭ
        Filament_increace controller = cylinder.AddComponent<Filament_increace>();
        controller.initialScale = 0.01f; // �ʱ� ������ ����
        controller.scaleIncreaseSpeed = scaleIncreaseSpeed; // ���� ���� �ӵ� ����
    }
}