using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Filament_Manager2 : MonoBehaviour
{

    [SerializeField] GameObject Filament; // Filament 프리팹
    void Start()
    {
        // 10초 후에 초기화 시작
        StartCoroutine(InitializeAfterDelay(20.0f));
        Debug.Log("지연");
    }


    IEnumerator InitializeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // 초기 Scale 설정
        Quaternion rotation = Quaternion.Euler(0, 90, 90);
        GameObject FilamentSp = Instantiate(Filament, transform.position, rotation);

        Debug.Log("생성");
    }
}
