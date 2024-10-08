using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
    public enum Axis { X, Y, Z };

    public GameObject[] rotateObjects; // ȸ���� ��ü��
    public Axis[] axis; // �� ��ü�� ȸ�� ��
    public bool[] isCCW; // �� ��ü�� ȸ�� ����
    public float rotSpeed = 200f; // ȸ�� �ӵ�

    void Start()
    {
        ArrayLengthSet();
    }

    void Update()
    {
        RotateObjects();
    }

    private void ArrayLengthSet()
    {
        if (rotateObjects.Length != isCCW.Length || rotateObjects.Length != axis.Length)
        {
            int index = rotateObjects.Length;
            isCCW = new bool[index];
            axis = new Axis[index]; 
        }
    }

    private void RotateObjects()
    {
        for (int i = 0; i < rotateObjects.Length; i++)
        {
            if (rotateObjects[i] != null)
            {
                float direction = isCCW[i] ? 1 : -1; // CCW�̸� 1, CW�̸� -1
                float rotationAmount = direction * rotSpeed * Time.deltaTime;

                // �࿡ ���� ȸ��
                switch (axis[i])
                {
                    case Axis.X:
                        rotateObjects[i].transform.Rotate(Vector3.right, rotationAmount);
                        break;
                    case Axis.Y:
                        rotateObjects[i].transform.Rotate(Vector3.up, rotationAmount);
                        break;
                    case Axis.Z:
                        rotateObjects[i].transform.Rotate(Vector3.forward, rotationAmount);
                        break;
                }
            }
        }
    }
}
