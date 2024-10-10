using UnityEngine;

public class Taping : MonoBehaviour
{
    [SerializeField] GameObject tapePrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            // Tape prefab ����
            GameObject tape = Instantiate(tapePrefab);

            // Tape�� ũ�� ����
            tape.transform.localScale = new Vector3(0.82f, 2f, 2f);

            // �ڽ��� �θ�� ����
            tape.transform.SetParent(other.transform);

            // Tape�� ��� ��ġ�� ����
            tape.transform.localPosition = new Vector3(0, -0.111f, 0.094f);
            tape.transform.localRotation = Quaternion.Euler(180, 0, -90);
        }
    }
}