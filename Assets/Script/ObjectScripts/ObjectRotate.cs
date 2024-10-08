using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
    public enum Axis { X, Y, Z };

    public GameObject[] rotateObjects; // 회전할 객체들
    public Axis[] axis; // 각 객체의 회전 축
    public bool[] isCCW; // 각 객체의 회전 방향
    public float rotSpeed = 200f; // 회전 속도

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
                float direction = isCCW[i] ? 1 : -1; // CCW이면 1, CW이면 -1
                float rotationAmount = direction * rotSpeed * Time.deltaTime;

                // 축에 따라 회전
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
