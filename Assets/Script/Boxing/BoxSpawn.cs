using System.Collections;
using UnityEngine;

public class BoxSpawn : MonoBehaviour
{
    public GameObject SqawnBox;
    public ObjectDestroy ObjectDestroy;
    private float delayTime = 5f;
    private Coroutine spawnCoroutine;
    public bool spawnOn = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        spawnOn = ObjectDestroy.objectDestroied;

        if (spawnOn)
        {
            StartSpawning();
        }
        else
        {
            StopSpawning();
        }
    }

    public void StartSpawning()
    {
        if (spawnCoroutine == null) // �̹� �ڷ�ƾ�� ���� ������ Ȯ��
        {
            spawnCoroutine = StartCoroutine(BoxSpawnCoroutine()); // �ڷ�ƾ ����
        }
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null) // ���� ���� �ڷ�ƾ�� ���� ���
        {
            StopCoroutine(spawnCoroutine); // �ڷ�ƾ ����
            spawnCoroutine = null; // ���� �ʱ�ȭ
        }
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
