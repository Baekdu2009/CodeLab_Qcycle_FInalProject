using System.Collections;
using UnityEngine;

public class PlasticSpawn : MonoBehaviour
{
    [SerializeField] GameObject[] prefabs;

    [HideInInspector]
    private bool isOn;
    private Coroutine spawnCoroutine; // �ڷ�ƾ �ν��Ͻ��� ������ ����

    public void OnBtnSpawn()
    {
        isOn = !isOn;

        if (isOn)
        {
            // �ڷ�ƾ�� ���� ������ ���� ���� ���� ����
            if (spawnCoroutine == null)
            {
                spawnCoroutine = StartCoroutine(SpawnObject());
            }
        }
        else
        {
            // �ڷ�ƾ�� ���� ���̸� �����ϰ� ���� �ʱ�ȭ
            if (spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine);
                spawnCoroutine = null; // �ڷ�ƾ �ν��Ͻ� �ʱ�ȭ
            }
        }
    }

    private IEnumerator SpawnObject()
    {
        while (isOn) // isOn�� true�� ���� ����ؼ� ����
        {
            int rand = Random.Range(0, prefabs.Length);
            GameObject newObj = Instantiate(prefabs[rand]);

            newObj.transform.SetParent(transform);
            newObj.transform.position = transform.position;

            yield return new WaitForSeconds(2f); // 2�� ���
        }

        spawnCoroutine = null; // �ڷ�ƾ ���� �� �ν��Ͻ� �ʱ�ȭ
    }
}
