using System.Collections;
using UnityEngine;

public class Filament_manager1 : MonoBehaviour
{
    public static Filament_manager1 instance;
    public GameObject Filament; // Filament ������
    public GameObject Filamentdia; // �밢�� Filament ������
    public GameObject Filament2;  // Filament2 ������

    public float scaleIncreaseSpeed; // ���� ���� �ӵ�
    float delayTime = 2.0f;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    void Start()
    {
        // (0, 0, 0) ��ġ�� Z������ 90�� ȸ���Ͽ� Filament ����
        Quaternion rotation = Quaternion.Euler(0, 0, 90); // Z������ 90�� ȸ��

        // GameManager�� ��ġ���� Filament ����
        GameObject FilamentIn = Instantiate(Filament, transform.position, rotation);

        // �ʿ�� �ּ� ����
        // CylinderController ��ũ��Ʈ�� �߰��Ͽ� �ʱ�ȭ
         /*Filament_increace controller = FilamentIn.AddComponent<Filament_increace>();
         controller.scaleIncreaseSpeed = scaleIncreaseSpeed;     // ���� ���� �ӵ� ����*/
       
        
        // 26.5 1.7 -6.39 rotate z -50
        // 27.0601 2.0876 -6.39 rotate z 90 Scale 0.2067529
    }

    public void SpawnPrefab1()
    {
        if (Filamentdia != null)
        {
            // ������ ��ġ �� ȸ��
            Vector3 SpawnPosition = new Vector3(26.7042f, 1.90857f, -6.39f);
            Quaternion SpawnRotation = Quaternion.Euler(0, 0, -50);

            // Filamentdia ����
            GameObject FilamentdiaIn = Instantiate(Filamentdia, SpawnPosition, SpawnRotation);
            Debug.Log("Filamentdia ����");

            StartCoroutine(SpawnWithDelay());
        }
    }

    public IEnumerator SpawnWithDelay()
    {
        if (Filament2 != null)
        {
            // ������ ��ġ �� ȸ��
            Vector3 SpawnPosition2 = new Vector3(27.135f, 2.0876f, -6.39f);
            Quaternion SpawnRotation2 = Quaternion.Euler(0, 0, 90);

            // Filament2 ����
            GameObject Filament2In = Instantiate(Filament2, SpawnPosition2, SpawnRotation2);
            Debug.Log("Filament2 ����");

            yield return new WaitForSeconds(delayTime);

        }
    }
}