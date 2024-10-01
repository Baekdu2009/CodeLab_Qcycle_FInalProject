using System.Collections;
using UnityEngine;

public class Filament_Manager2 : MonoBehaviour
{
    public static Filament_Manager2 instance;
    public GameObject Filament; // Filament ������
    public GameObject Filamentdia; // �밢�� Filament ������
    public GameObject Filament2;  // Filament2 ������
    public GameObject Filament3;  // Filament2 ������

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
        // (0, 0, 0) ��ġ�� y, Z������ 90�� ȸ���Ͽ� Filament ����
        Quaternion rotation = Quaternion.Euler(0, 90, 90); // Z������ 90�� ȸ��

        // GameManager�� ��ġ���� Filament ����
        GameObject FilamentIn = Instantiate(Filament, transform.position, rotation);

        // �ʿ�� �ּ� ����
        // CylinderController ��ũ��Ʈ�� �߰��Ͽ� �ʱ�ȭ
        /*Filament_increace controller = FilamentIn.AddComponent<Filament_increace>();
        controller.scaleIncreaseSpeed = scaleIncreaseSpeed;     // ���� ���� �ӵ� ����*/


        // 30.437 2.102 -2.463 Scale 0.425 Z 15 Y 90
        // 30.437 2.504 -1.845 Scale 0.52 Z 90 Y 90
        // 30.437 2.209 -0.98 Scale 0.46 Z 130 Y 90

    }

    public void SpawnPrefab1()
    {
        if (Filamentdia != null)
        {
            // ������ ��ġ �� ȸ��
            Vector3 SpawnPosition = new Vector3(30.437f, 2.102f, -2.463f);
            Quaternion SpawnRotation = Quaternion.Euler(0, 90, 15);

            // Filamentdia ����
            GameObject FilamentdiaIn = Instantiate(Filamentdia, SpawnPosition, SpawnRotation);
            Debug.Log("Filamentdia2 ����");

            StartCoroutine(SpawnWithDelay());
        }
    }


    public IEnumerator SpawnWithDelay()
    {
        if (Filament2 != null)
        {
            yield return new WaitForSeconds(delayTime);

            // ������ ��ġ �� ȸ��
            Vector3 SpawnPosition2 = new Vector3(30.437f, 2.504f, -1.845f);
            Quaternion SpawnRotation2 = Quaternion.Euler(0, 90, 90);

            // Filament2 ����
            GameObject Filament2In = Instantiate(Filament2, SpawnPosition2, SpawnRotation2);
            Debug.Log("Filament3 ����");

            SpawnWithDelay2();


        }
    }

    public void SpawnWithDelay2()
    {
        if (Filament3 != null)
        {
            Vector3 SpawnPosition3 = new Vector3(30.437f, 2.209f, -0.98f);
            Quaternion SpawnRotation3 = Quaternion.Euler(0, 90, 130);

            GameObject Filament3In = Instantiate(Filament3, SpawnPosition3, SpawnRotation3);
            Debug.Log("Filament4 ����");
        }
// yield return new WaitForSeconds(delayTime);
        Filament_Rotate.instance.Cpf();
        Debug.Log("����");
    }
}