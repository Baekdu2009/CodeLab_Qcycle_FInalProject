using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoveAGV : MonoBehaviour
{
    public GameObject AGV;        // ������ AGV
    public Transform target1;      // ��ǥ ��ġ (Warehouse Object)
    public Transform target2;      // ��ǥ ��ġ (3D Print Object) 
    public Transform target3;      // 
    public Button moveButton1;     // Ŭ���� ��ư
    public Button moveButton2;     // Ŭ���� ��ư
    public Button moveButton3;     // Ŭ���� ��ư
    public float speed = 2.0f;    // �̵� �ӵ�

    private bool isMoving = false; // ������ ����
    private Transform currentTarget; // ���� ��ǥ ��ġ(Filament)

    void Start()
    {
        // ��ư Ŭ�� �� ���� �ٸ� ��ġ�� �̵�
        moveButton1.onClick.AddListener(() => MoveAGVToTarget(target1));     
        moveButton3.onClick.AddListener(() => MoveAGVToTarget(target2));      
        moveButton2.onClick.AddListener(() => MoveAGVToTarget(target3));      // �ʱ� ��ġ(Filament)
    }
   
        

    void Update()
    {
        if (isMoving && currentTarget != null)
        {
            // AGV�� ��ǥ �������� �̵�
            AGV.transform.position = Vector3.MoveTowards(AGV.transform.position, currentTarget.position, speed * Time.deltaTime);

            // AGV�� ��ǥ�� �����ߴ��� Ȯ��
            if (Vector3.Distance(AGV.transform.position, currentTarget.position) < 0.1f)
            {
                AGV.transform.position = currentTarget.position; // ��Ȯ�� ��ǥ ��ġ���� ����
                isMoving = false; // �̵� ����
               // Debug.Log("AGV ����");
            }
        }
    }

    void MoveAGVToTarget(Transform target)
    {
        currentTarget = target; // ��ǥ ��ġ ����
        isMoving = true; // ��ư Ŭ�� �� �̵� ����
    }
}