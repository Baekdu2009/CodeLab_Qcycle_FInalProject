using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
    public GameObject[] rotateObjects;
    public bool[] isCCW;
    public float rotSpeed = 200f;

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
        if (rotateObjects.Length != isCCW.Length)
        {
            int index = rotateObjects.Length;
            isCCW = new bool[index];
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
                rotateObjects[i].transform.Rotate(Vector3.up, rotationAmount);
            }
        }
    }
}
