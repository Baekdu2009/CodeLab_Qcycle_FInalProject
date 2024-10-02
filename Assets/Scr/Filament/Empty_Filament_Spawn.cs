using System.Collections;
using UnityEngine;

public class Empty_Filament_Spawn : MonoBehaviour
{
    [SerializeField] GameObject Filamentdia; // �밢�� Filament ������
    [SerializeField] GameObject Filament2; // Filament2 ������
    [SerializeField] float initialScale = 0.01f; // �ʱ� Scale
    [SerializeField] float maxScale = 1.65f; // �ִ� Scale
    [SerializeField] float scaleIncreaseSpeed = 1f; // ���� ���� �ӵ�
    [SerializeField] float xMoveSpeed = 1f; // X�� �̵� �ӵ�
    [SerializeField] float delayTime = 2.0f;

    private Transform spawnTransform; // CreateEmpty���� ���޹��� Transform
    bool isScaling = true; // Scale ���� ����

    public void Start()
    {
        // �ʱ� Scale ����
        transform.localScale = new Vector3(initialScale, initialScale, initialScale);
    }

    // CreateEmpty���� Transform�� �����ϴ� �޼���
    public void SetSpawnTransform(Transform transform)
    {
        spawnTransform = transform;
    }

    void Update()
    {
        if (isScaling)
        {
            UpdateScaleAndPosition();

            // Y�� Scale�� �ִ�ġ�� �ʰ� �� ����
            if (transform.localScale.x >= maxScale)
            {
                FinalizeScaling();
            }
        }
    }

    void UpdateScaleAndPosition()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x += scaleIncreaseSpeed * Time.deltaTime;
        transform.localScale = currentScale; // ���ο� Scale ����
        transform.Translate(new Vector3(xMoveSpeed * Time.deltaTime, 0, 0), Space.World); // X������ �̵�
    }

    void FinalizeScaling()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x = maxScale; // �ִ� Scale�� ����
        transform.localScale = currentScale;
        isScaling = false;

        SpawnPrefab1();
    }

    void SpawnPrefab1()
    {
        if (Filamentdia != null)
        {
            // CreateEmpty�� ��ġ�� ����Ͽ� ����
            Vector3 spawnPosition = spawnTransform.position;
            Quaternion spawnRotation = Quaternion.Euler(0, 0, -50);

            Instantiate(Filamentdia, spawnPosition, spawnRotation);
            StartCoroutine(SpawnWithDelay());
        }
    }

    IEnumerator SpawnWithDelay()
    {
        yield return new WaitForSeconds(delayTime);

        if (Filament2 != null)
        {
            // CreateEmpty�� ��ġ�� ����Ͽ� ����
            Vector3 spawnPosition2 = spawnTransform.position;
            Quaternion spawnRotation2 = Quaternion.Euler(0, 0, 90);

            Instantiate(Filament2, spawnPosition2, spawnRotation2);
        }
    }
}
