using UnityEngine;
using System.Collections;
using static ControlManager;

public class ControlManager : MonoBehaviour
{
    public GameObject filamentRoller;
    public GameObject filamentPrefab;
    public Transform spawnFilament; // Unity 에디터에서 설정 가능
    float rotSpeed = 200;
    private GameObject filament; // 현재 생성된 filament 저장
    MeshRenderer filamentMesh;

    public Transform roadStart1; // 시작 위치1
    public Transform roadEnd1; // 끝 위치1
    public float speed = 2.0f; // 이동 속도

    public Transform filamentLocation; // 필라멘트 위치
    public GameObject itemPrefab;
    GameObject examItem;
    public Transform itemSpawnLocation;
    public Transform dropLocation;
    public float dropSpeed = 2;

    public Transform AGVLocation;
    public Transform AGVstartPos;
    public Transform AGVendPos;

    public Transform roadStart2; // 시작 위치2
    public Transform roadEnd2; // 끝 위치2

    public GameObject boxPrefab;
    GameObject boxObj;
    public Transform boxLocation; // 박스 생성

    public GameObject panel;
    public GameObject text;


    private void Start()
    {
        if (spawnFilament == null)
        {
            Debug.LogError("spawnFilament가 설정되지 않았습니다. Unity 에디터에서 설정해 주세요.");
        }
        else
        {
            spawnFilament.position = filamentRoller.transform.position; // 초기 위치 설정
        }
    }
    private void Update()
    {
        
    }

    void RollerRotate()
    {
        filamentRoller.transform.Rotate(0, 0, -rotSpeed * Time.deltaTime);
    }

    public void OnFilamentSpawnBtn()
    {
        if (spawnFilament != null)
        {
            if (filament == null) // 기존 filament가 없을 경우에만 생성
            {
                filament = Instantiate(filamentPrefab, spawnFilament.position, Quaternion.Euler(90, 0, 0));
                filamentMesh = filament.GetComponent<MeshRenderer>();
                filamentMesh.material.color = Color.red;
            }
            else
            {
                Debug.LogWarning("이미 filament가 생성되어 있습니다.");
            }
        }
        else
        {
            Debug.LogError("spawnFilament가 null입니다. 필라멘트를 생성할 수 없습니다.");
        }
    }

    public void OnFirstRoadMoveBtn()
    {
        if (filament != null)
        {
            filamentMesh = filament.GetComponent<MeshRenderer>();
            filamentMesh.material.color = Color.blue;

            if (filament == null)
            {
                Debug.LogError("FilamentMachine에서 filament를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("FilamentMachine을 찾을 수 없습니다.");
        }

        if (filament != null) // filament가 존재하는 경우에만 이동
        {
            StartCoroutine(RoadMove(roadStart1, roadEnd1, filament));
        }
        else
        {
            Debug.LogError("이동할 filament가 없습니다.");
        }
    }

    IEnumerator RoadMove(Transform start, Transform end, GameObject obj)
    {
        obj.transform.position = start.position;
        obj.transform.rotation = Quaternion.Euler(0, 0, 0);

        while (Vector3.Distance(obj.transform.position, end.position) > 0.1f)
        {
            // 현재 위치와 목표 위치 사이의 방향 벡터 계산
            Vector3 direction = (end.position - obj.transform.position).normalized;

            // 속도에 따라 이동
            obj.transform.position += direction * speed * Time.deltaTime;

            yield return new WaitForEndOfFrame(); // 다음 프레임까지 대기
        }
        // 최종 위치 보정
        obj.transform.position = end.position;
        // print(filament);
    }

    public void OnFilamentLocationBtn()
    {
        if (filament != null) // filament가 존재하는지 확인
        {
            filament.transform.position = filamentLocation.position; // 위치 설정
            filament.transform.rotation = Quaternion.Euler(90, 0, 0); // 회전 설정
        }
        else
        {
            Debug.LogError("현재 filament가 없습니다.");
        }
        
    }
    public void OnOperationBtn()
    {
        filament.transform.Rotate(0, 0, rotSpeed * Time.deltaTime);

        // 아이템 생성
        examItem = Instantiate(itemPrefab);
        examItem.transform.position = itemSpawnLocation.position;

        // 드롭 위치로 이동
        StartCoroutine(MoveToDropLocation(examItem, dropLocation));
    }

    // 드롭 위치로 이동하는 코루틴
    IEnumerator MoveToDropLocation(GameObject item, Transform dropLoc)
    {
        while (Vector3.Distance(item.transform.position, dropLoc.position) > 0.1f)
        {
            // 현재 위치와 드롭 위치 사이의 방향 벡터 계산
            Vector3 direction = (dropLoc.position - item.transform.position).normalized;

            // 속도에 따라 이동
            item.transform.position += direction * dropSpeed * Time.deltaTime;

            yield return new WaitForEndOfFrame(); // 다음 프레임까지 대기
        }
        // 최종 위치 보정
        item.transform.position = dropLoc.position;
    }

    public void OnAGVMoving()
    {
        StartCoroutine(AGVMoving(AGVLocation, AGVendPos));
        StartCoroutine(AGVMoving(examItem.transform, AGVendPos));
    }
    IEnumerator AGVMoving(Transform obj, Transform to)
    {
        while (Vector3.Distance(obj.position, to.position) > 0.1f)
        {
            Vector3 direction = (to.position - obj.position).normalized;
            obj.position += direction * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
        obj.position = to.position;
    }
    public void OnSecondRoadMoveBtn()
    {
        if (examItem != null) // examItem이 존재하는 경우에만 이동
        {
            StartCoroutine(RoadMove(roadStart2, roadEnd2, examItem));
        }
        else
        {
            Debug.LogError("이동할 examItem이 없습니다.");
        }
    }

    public void OnBoxingBtn()
    {
        boxObj = Instantiate(boxPrefab);
        boxObj.transform.position = boxLocation.position;
        examItem.transform.parent = boxObj.transform;
        examItem.transform.position = boxObj.transform.position;

        panel.SetActive(false);
        text.SetActive(true);
    }
}
