using UnityEngine;
using System.Collections;

public class AutoPrinter : MonoBehaviour
{
    // 노즐 관련 변수
    public GameObject nozzle;
    public GameObject movingAxis;
    public GameObject movingPlate;
    public Transform nozzleTip;
    Vector3 nozzleMoving;
    public float nozzleSpeed;
    public float axisSpeed;
    float xRange = 0.5f;
    float zMin = 0.6f;
    float zMax = 1.2f;
    float yRange = 0.002f;
    int powderCnt;

    public GameObject powderPrefab;
    GameObject powderItem;
    public GameObject printingPrefab;
    public Transform printingPos;
    public GameObject printingPosObj;
    bool isPowderOn = false;
    Coroutine powderCoroutine;
    bool isMovingToX = false; // X축 이동 상태

    void Start()
    {
        // nozzle.transform.position = new Vector3 (-xRange, yMin, zRange);
        nozzleMoving = nozzle.transform.localPosition;
    }

    
    void Update()
    {
        NozzleMoving();
        ZAxisMoving(movingAxis);
        TransformPosition(printingPosObj);
    }

    public void TogglePowderCreation()
    {
        if (isPowderOn)
        {
            // 코루틴이 실행 중일 때 멈추기
            if (powderCoroutine != null)
            {
                StopCoroutine(powderCoroutine);
                powderCoroutine = null; // 코루틴 참조를 null로 설정
            }
        }
        else
        {
            // 코루틴을 시작하기
            powderCoroutine = StartCoroutine(ItemCreation());
        }

        isPowderOn = !isPowderOn; // 상태 토글
    }

    IEnumerator ItemCreation()
    {
        while (true) // 무한 루프를 통해 계속 생성
        {
            powderItem = Instantiate(powderPrefab, nozzleTip);
            powderItem.transform.localPosition = nozzleTip.localPosition;
            powderItem.transform.parent = null;
            powderItem.transform.localScale = new Vector3(.05f, .05f, .05f);
            yield return new WaitForSeconds(0.1f); // 0.1초 대기
        }
    }
    void NozzleMoving()
    {
        nozzleMoving.y = yRange * Mathf.Sin(Time.time * nozzleSpeed);

        //if (Mathf.Abs(nozzleMoving.z) < 0.01f && !isMovingToX) // Z축 왕복 운동이 완료되면
        //{
        //    isMovingToX = true; // X축 이동 상태로 변경
        //    StartCoroutine(MoveToX());
        //}

        nozzle.transform.localPosition = nozzleMoving;
    }

    //IEnumerator MoveToX()
    //{
    //    // 현재 X 위치와 목표 X 위치 설정
    //    float currentX = nozzle.transform.position.x;
    //    float targetX = Mathf.Clamp(currentX + 0.1f, -xRange, xRange); // xRange 내에서 목표 X 위치 설정

    //    // X축으로 0.1만큼 이동
    //    while (Mathf.Abs(nozzle.transform.position.x - targetX) > 0.01f)
    //    {
    //        nozzleMoving.x = Mathf.MoveTowards(nozzle.transform.position.x, targetX, nozzleSpeed * Time.deltaTime);
    //        nozzle.transform.position = nozzleMoving;
    //        yield return null;
    //    }

    //    // 이동이 완료되면 Z축 왕복 운동 재개
    //    isMovingToX = false;
    //}

    void NozzleXAxisMoving()
    {
        nozzleMoving.x = xRange * Mathf.Sin(Time.time * nozzleSpeed);
        nozzle.transform.position = nozzleMoving;
    }

    void ZAxisMoving(GameObject obj)
    {
        Vector3 zMoving = obj.transform.localPosition;
        
        if (Input.GetKey(KeyCode.UpArrow))
        {
            zMoving.z += axisSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            zMoving.z -= axisSpeed * Time.deltaTime;
        }

        obj.transform.localPosition = zMoving;
    }
    Vector3 TransformPosition(GameObject obj)
    {
        Vector3 pos = obj.transform.position;
        pos.x = printingPos.position.x;
        pos.y = 0;
        pos.z = printingPos.position.z;
        obj.transform.position = pos;
        return pos;
    }

    void PlateMoving()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Powder")
        {
            if (powderCnt <= 19)
            {
                powderCnt++;
                print(powderCnt);
            }
            else if (powderCnt > 19)
            {
                GameObject obj = Instantiate(printingPrefab);
                obj.transform.position = printingPos.position;
                powderCnt = 0;
            }
        }
    }
}
