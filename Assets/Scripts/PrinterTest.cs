using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class PrinterTest : MonoBehaviour
{
    // 움직임 제한 범위 (x, y, z) = (-0.5 ~ 0.5, 0.6 ~ 1.2, -0.4 ~ 0.4)
    public GameObject nozzle;
    public Transform nozzleTip;
    public GameObject powderPrefab;
    GameObject powderItem;
    public GameObject powderObj { get => powderItem; }
    Vector3 nozzleOriginPos;
    Vector3 pos;
    public float nozzleSpeed;
    float xRange = 0.5f;
    float yMin = 0.6f;
    float yMax = 1.2f;
    float zRange = 0.4f;
    Coroutine itemCreationCoroutine;

    void Start()
    {
        nozzleOriginPos = nozzle.transform.position;
        pos = nozzle.transform.position;
    }

    void Update()
    {
        KeyInputMoving();
    }

    //Vector3 Direction(float x, float y, float z)
    //{
    //    Vector3 to = new Vector3(x, y, z);
    //    return nozzle.transform.position - to;
    //}

    //void Moving(GameObject obj)
    //{
    //    Vector3 v = obj.transform.position;

    //    v.z = nozzleOriginPos.z + zRange * Mathf.Sin(Time.time * nozzleSpeed);

    //    obj.transform.position = v;
    //}

    void KeyInputMoving()
    {
        Vector3 nozzleVector = nozzle.transform.position;

        // A키와 D키로 z축 이동
        if (Input.GetKey(KeyCode.A))
        {
            nozzleVector.z -= nozzleSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            nozzleVector.z += nozzleSpeed * Time.deltaTime;
        }

        // W키와 S키로 x축 이동
        if (Input.GetKey(KeyCode.W))
        {
            nozzleVector.x -= nozzleSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            nozzleVector.x += nozzleSpeed * Time.deltaTime;
        }

        // 위쪽 화살표와 아래쪽 화살표로 y축 이동
        if (Input.GetKey(KeyCode.UpArrow))
        {
            nozzleVector.y += nozzleSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            nozzleVector.y -= nozzleSpeed * Time.deltaTime;
        }

        // Space 키를 눌렀을 때 오브젝트 생성 시작/중지
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (itemCreationCoroutine == null)
            {
                itemCreationCoroutine = StartCoroutine(ItemCreation());
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            if (itemCreationCoroutine != null)
            {
                StopCoroutine(itemCreationCoroutine);
                itemCreationCoroutine = null;
            }
        }

        // 움직임의 범위를 제한합니다.
        nozzleVector.x = Mathf.Clamp(nozzleVector.x, nozzleOriginPos.x - xRange, nozzleOriginPos.x + xRange);
        nozzleVector.y = Mathf.Clamp(nozzleVector.y, yMin, yMax);
        nozzleVector.z = Mathf.Clamp(nozzleVector.z, nozzleOriginPos.z - zRange, nozzleOriginPos.z + zRange);

        // 최종 위치를 업데이트합니다.
        nozzle.transform.position = nozzleVector;
    }

    IEnumerator ItemCreation()
    {
        while (true) // 무한 루프를 통해 계속 생성
        {
            powderItem = Instantiate(powderPrefab, nozzleTip);
            powderItem.transform.position = nozzleTip.position;
            powderItem.transform.parent = null;

            yield return new WaitForSeconds(0.1f); // 1초 대기
        }
    }


}