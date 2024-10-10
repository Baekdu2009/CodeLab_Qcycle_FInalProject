using UnityEngine;

public class Taping : MonoBehaviour
{
    [SerializeField] GameObject tapePrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            // Tape prefab 생성
            GameObject tape = Instantiate(tapePrefab);

            // Tape의 크기 설정
            tape.transform.localScale = new Vector3(0.82f, 2f, 2f);

            // 박스를 부모로 설정
            tape.transform.SetParent(other.transform);

            // Tape의 상대 위치로 설정
            tape.transform.localPosition = new Vector3(0, -0.111f, 0.094f);
            tape.transform.localRotation = Quaternion.Euler(180, 0, -90);
        }
    }
}