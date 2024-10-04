using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class Filament_increase1 : MonoBehaviour
{
    
    [SerializeField] GameObject Filamentdia; // �밢�� Filament ������
    [SerializeField] GameObject Filament2; // Filament2 ������
    [SerializeField] float initialScale = 0.01f; // �ʱ� Scale
    [SerializeField] float maxScale = 1.65f; // �ִ� Scale
    [SerializeField] float scaleIncreaseSpeed = 70f; // ���� ���� �ӵ�
    [SerializeField] float xMoveSpeed = 40f; // X�� �̵� �ӵ�
  
    bool isScaling = true;  // Scale ���� ����
    float delayTime = 2.0f;

    public void Start()
    {
        // 26.5 1.7 -6.39 rotate z -50
        // 27.0601 2.0876 -6.39 rotate z 90 Scale 0.2067529

        // �ʱ� Scale ����
        transform.localScale = new Vector3(initialScale, initialScale, initialScale);
       
    }

    void Update()
    {
        if (isScaling)
        {
            UpdateScaleAndPosition();

            // Y�� Scale�� 1.65 �ʰ� �� ����
            if (transform.localScale.y >= maxScale)
            {
              FinalizeScaling();
            }          
        }
    }

    void UpdateScaleAndPosition()
    {
        Vector3 currentScale = transform.localScale;
        // Y�� Scale ����
        currentScale.y += scaleIncreaseSpeed * Time.deltaTime;
        // ���ο� Scale ����(�� �����Ӹ��� ��ȭ�ϴ� ������ ����)
        transform.localScale = currentScale;
        transform.Translate(new Vector3(xMoveSpeed * Time.deltaTime, 0, 0), Space.World);
        // Debug.Log("�̵� --------, ���� ��ġ: " + transform.position);
       
    }

    void FinalizeScaling()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.y = maxScale;
        transform.localScale = currentScale;
        isScaling = false ;

        SpawnPrefab1 ();
    }
    void SpawnPrefab1()
    {
        if (Filamentdia != null) {

            Vector3 SpawnPosition = new Vector3(26.7042f, 1.90857f, -6.39f);
            Quaternion SpawnRotation = Quaternion.Euler(0, 0, -50);

            // GameObject FilamentSpDia = Instantiate(Filamentdia, SpawnPosition, SpawnRotation);
            Instantiate(Filamentdia, SpawnPosition, SpawnRotation);

            StartCoroutine(SpawnWithDelay());
        }
    }

    IEnumerator SpawnWithDelay()
    {
        yield return new WaitForSeconds(delayTime);

        if (Filament2 != null)
        {
            Vector3 SpawnPosition2 = new Vector3(27.135f, 2.0876f, -6.39f);
            Quaternion SpawnRotation2 = Quaternion.Euler(0, 0, 90);

            // GameObject FilamentSp2 = Instantiate(Filament2, SpawnPosition2, SpawnRotation2);
            Instantiate(Filament2, SpawnPosition2, SpawnRotation2);
        }
    }
}