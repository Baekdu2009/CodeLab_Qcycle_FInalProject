using UnityEngine;

public class Filament_increace : MonoBehaviour
{
    public float initialScale = 0.2f; // �ʱ� Scale
    public float scaleIncreaseSpeed; // ���� ���� �ӵ�
    private bool isScaling = true;  // Scale ���� ����
    private float maxScale; // �ִ� Scale
    public float maxScaleMultiplier; // �ִ� Scale���
    public float xMoveSpeed; // X�� �̵� �ӵ�



    void Start()
    {
        // �ʱ� Scale ����
        transform.localScale = new Vector3(initialScale, initialScale, initialScale);
        maxScale = initialScale * maxScaleMultiplier; // �ִ� Scale ����
    }

    void Update()
    {
        if (isScaling)
        {
            // �Ǹ����� Y�� Scale�� �������� ���� �ø���
            Vector3 currentScale = transform.localScale;
            currentScale.y += scaleIncreaseSpeed * Time.deltaTime; // Y�� Scale ���� �������� ����
            transform.position += new Vector3(scaleIncreaseSpeed * Time.deltaTime, 0, 0); // ��ġ ���� (X������ �̵�)

            // Y�� Scale�� 9 �ʰ� �� ����
            if (currentScale.y > 9)
            {
                currentScale.y = 9; // �ִ� Scale�� ����
                isScaling = false;    // Scale ���� ����
            }

            transform.localScale = currentScale; // ���ο� Scale ���� 

            // X������ �̵� (������Ʈ�� ���������� �̵�)
            transform.position += new Vector3(xMoveSpeed * Time.deltaTime, 0, 0);
        }
        else
        {
            // Scale�� �ִ뿡 �������� �� X�� �̵� ����
            // �� �κ��� �ƹ��͵� ���� ����
        }

    }
}