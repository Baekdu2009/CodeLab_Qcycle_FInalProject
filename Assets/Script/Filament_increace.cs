using UnityEngine;

public class Filament_increace : MonoBehaviour
{
    public float initialScale = 0.2f; // �ʱ� ������
    public float scaleIncreaseSpeed; // ���� ���� �ӵ�
    private bool isScaling = true;  // ������ ���� ����
    private float maxScale; // �ִ� ������
    public float maxScaleMultiplier; // �ִ� �����Ϲ��

    void Start()
    {
        // �ʱ� ������ ����
        transform.localScale = new Vector3(initialScale, initialScale, initialScale);
        maxScale = initialScale * maxScaleMultiplier; // �ִ� ������ ����
    }

    void Update()
    {
        // �Ǹ����� Y�� �������� �������� ���� �ø���
        Vector3 currentScale = transform.localScale;
        currentScale.y += scaleIncreaseSpeed * Time.deltaTime; // Y�� ������ ����

        if(currentScale.y > 9)
        {
            currentScale.y = 9; // �ִ� �����Ϸ� ����
            isScaling = false;    // ������ ���� ����
        }

        transform.localScale = currentScale; // ���ο� ������ ����
    }
}