using UnityEngine;

public class Filament_increace : MonoBehaviour
{
    public float initialScale = 0.001f; // �ʱ� Scale
    public float scaleIncreaseSpeed; // ���� ���� �ӵ�
    private bool isScaling = true;  // Scale ���� ����
    public float maxScale = 1.65f; // �ִ� Scale
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
            transform.position += new Vector3(scaleIncreaseSpeed * Time.deltaTime, 0, 0); // ��ġ ���� (X������ �̵�)

            // Y�� Scale�� 1.65 �ʰ� �� ����
            if (currentScale.y > maxScale)
            {
                currentScale.y = maxScale; // �ִ� Scale�� ����
                isScaling = false;    // Scale ���� ����
                GameManager.instance.SpawnPrefab1();
            }

            transform.localScale = currentScale; // ���ο� Scale ���� 

            // X������ �̵� (������Ʈ�� ���������� �̵�)
            transform.position += new Vector3(xMoveSpeed * Time.deltaTime, 0, 0);
        }
        /* else
          {
              // Scale�� �ִ뿡 �������� �� X�� �̵� ����
              // �� �κ��� �ƹ��͵� ���� ����
          }*/
    }
}