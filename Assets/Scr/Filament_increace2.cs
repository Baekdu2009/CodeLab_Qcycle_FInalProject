using UnityEngine;

public class Filament_increace2 : MonoBehaviour
{
    public float initialScale = 0.01f; // �ʱ� Scale
    public float scaleIncreaseSpeed; // ���� ���� �ӵ�
    private bool isScaling = true;  // Scale ���� ����
    public float maxScale = 1.4f; // �ִ� Scale
    public float xMoveSpeed; // X�� �̵� �ӵ�

    void Start()
    {
        // �ʱ� Scale ����
        transform.localScale = new Vector3(initialScale, initialScale, initialScale);
    }

    void Update()
    {
        if (isScaling)
        {
            // �Ǹ����� Y�� Scale�� �������� ���� �ø���
            Vector3 currentScale = transform.localScale;
            currentScale.y += scaleIncreaseSpeed * Time.deltaTime; // Y�� Scale ���� �������� ����
            transform.position += new Vector3(0, 0, scaleIncreaseSpeed * Time.deltaTime); // ��ġ ���� (z������ �̵�)
            

            // Y�� Scale�� 1.65 �ʰ� �� ����
            if (currentScale.y > maxScale)
            {
                currentScale.y = maxScale; // �ִ� Scale�� ����
                isScaling = false;    // Scale ���� ����
                GameManager2.instance.SpawnPrefab1();
                
            }

            // ���ο� Scale ����(�� �����Ӹ��� ��ȭ�ϴ� ������ ����)
            transform.localScale = currentScale;
        }
    }
}