using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class BoxSqawn : MonoBehaviour
{
    public GameObject SqawnBox; // ������ �ڽ� ������
    private float delayTime = 5f; // �ڽ� ���� ����
    private bool isSpawning = false; // ���� ������ ����

    private void Start()
    {
        StartCoroutine(BoxSpawnCoroutine()); // �ڷ�ƾ ����
    }

    private IEnumerator BoxSpawnCoroutine()
    {
        while (true) // ���� ����
        {
            Quaternion BoxSqawnRotate = Quaternion.Euler(-90, 0, 180);
            // �ڽ��� ����
            Instantiate(SqawnBox, transform.position, BoxSqawnRotate);
            // ��� �ð�
            yield return new WaitForSeconds(delayTime);
        }
    }
}
