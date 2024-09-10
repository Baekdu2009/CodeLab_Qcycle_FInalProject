using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoveAGV : MonoBehaviour
{
    public GameObject AGV;        // 움직일 AGV
    public Transform target1;      // 목표 위치 (Warehouse Object)
    public Transform target2;      // 목표 위치 (3D Print Object) 
    public Transform target3;      // 
    public Button moveButton1;     // 클릭할 버튼
    public Button moveButton2;     // 클릭할 버튼
    public Button moveButton3;     // 클릭할 버튼
    public float speed = 2.0f;    // 이동 속도

    private bool isMoving = false; // 움직임 상태
    private Transform currentTarget; // 현재 목표 위치(Filament)

    void Start()
    {
        // 버튼 클릭 시 각기 다른 위치로 이동
        moveButton1.onClick.AddListener(() => MoveAGVToTarget(target1));     
        moveButton3.onClick.AddListener(() => MoveAGVToTarget(target2));      
        moveButton2.onClick.AddListener(() => MoveAGVToTarget(target3));      // 초기 위치(Filament)
    }
   
        

    void Update()
    {
        if (isMoving && currentTarget != null)
        {
            // AGV를 목표 방향으로 이동
            AGV.transform.position = Vector3.MoveTowards(AGV.transform.position, currentTarget.position, speed * Time.deltaTime);

            // AGV가 목표에 도착했는지 확인
            if (Vector3.Distance(AGV.transform.position, currentTarget.position) < 0.1f)
            {
                AGV.transform.position = currentTarget.position; // 정확히 목표 위치에서 멈춤
                isMoving = false; // 이동 멈춤
               // Debug.Log("AGV 멈춤");
            }
        }
    }

    void MoveAGVToTarget(Transform target)
    {
        currentTarget = target; // 목표 위치 설정
        isMoving = true; // 버튼 클릭 시 이동 시작
    }
}