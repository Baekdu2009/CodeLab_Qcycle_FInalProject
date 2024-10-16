using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class BoxSqawn : MonoBehaviour
{
    public GameObject SqawnBox; // 생성할 박스 프리팹
    private float delayTime = 5f; // 박스 생성 간격
    private bool isSpawning = false; // 생성 중인지 여부

    private void Start()
    {
        StartCoroutine(BoxSpawnCoroutine()); // 코루틴 시작
    }

    private IEnumerator BoxSpawnCoroutine()
    {
        while (true) // 무한 루프
        {
            Quaternion BoxSqawnRotate = Quaternion.Euler(-90, 0, 180);
            // 박스를 생성
            Instantiate(SqawnBox, transform.position, BoxSqawnRotate);
            // 대기 시간
            yield return new WaitForSeconds(delayTime);
        }
    }
}
