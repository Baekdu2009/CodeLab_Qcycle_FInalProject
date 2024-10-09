using UnityEngine;

public class AGVLarge : AGVControl
{
    public AGVCart[] movingCarts; // AGV�� �̵��� īƮ �迭
    public bool fullSignalInput;   // ��ȣ ���¸� ��Ÿ���� ����

    private void Start()
    {
        movingPositions.Add(transform);
        
    }

    private void Update()
    {
        CartSignalCheck(); // īƮ ��ȣ üũ
        
        if (isMoving)
        {
            MoveAlongPath();   // ��� ���� �̵�
        }
    }

    private void CartSignalCheck()
    {
        if (!isMoving)
        {
            fullSignalInput = false; // ��ȣ �ʱ�ȭ

            for (int i = 0; i < movingCarts.Length; i++)
            {
                if (movingCarts[i].isAGVCallOn) // īƮ�� ��ȣ�� ���� �ִ��� Ȯ��
                {
                    fullSignalInput = true; // ��ȣ�� �ϳ��� ������ true�� ����
                    movingPositions.Add(movingCarts[i].transform); // ���� ��ǥ ��ġ ����
                    isMoving = true; // AGV �̵� ���� ����
                    break; // ù ��° īƮ�� ó���ϰ� ���� ����
                }
            }
        }
    }
}
