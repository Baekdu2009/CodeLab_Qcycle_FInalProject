using UnityEngine;

public class Taping : MonoBehaviour
{
    [SerializeField] GameObject tapePrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            if (gameObject.name == "Taping")
            {

                BackTapeSqwan(other.gameObject);
            }

            if (gameObject.name == "Taping2")
            {
                FrontTapeSqwan(other.gameObject);
            }
        }
    }
    private void FrontTapeSqwan(GameObject obj)
    {
        // Tape prefab ����
        GameObject tape = Instantiate(tapePrefab);

        // Tape�� ũ�� ����
        tape.transform.localScale = new Vector3(0.82f, 2f, 2f);

        // �ڽ��� �θ�� ����
        tape.transform.SetParent(obj.transform);

        // Tape�� ��� ��ġ�� ����
        tape.transform.localPosition = new Vector3(0, -0.111f, -0.0018f);
        tape.transform.localRotation = Quaternion.Euler(180, 180, -90);
    }

    private void BackTapeSqwan(GameObject obj)
    {
        // Tape prefab ����
        GameObject tape = Instantiate(tapePrefab);

        // Tape�� ũ�� ����
        tape.transform.localScale = new Vector3(0.82f, 2f, 2f);

        // �ڽ��� �θ�� ����
        tape.transform.SetParent(obj.transform);

        // Tape�� ��� ��ġ�� ����
        tape.transform.localPosition = new Vector3(0, -0.111f, 0.094f);
        tape.transform.localRotation = Quaternion.Euler(180, 0, -90);
    }
}