using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Filament_Manager2 : MonoBehaviour
{

    [SerializeField] GameObject Filament; // Filament ������
    void Start()
    {
        // 10�� �Ŀ� �ʱ�ȭ ����
        StartCoroutine(InitializeAfterDelay(20.0f));
        Debug.Log("����");
    }


    IEnumerator InitializeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // �ʱ� Scale ����
        Quaternion rotation = Quaternion.Euler(0, 90, 90);
        GameObject FilamentSp = Instantiate(Filament, transform.position, rotation);

        Debug.Log("����");
    }
}
