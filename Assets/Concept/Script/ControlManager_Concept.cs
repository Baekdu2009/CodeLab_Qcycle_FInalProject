using UnityEngine;
using System.Collections;
using static ControlManager;

public class ControlManager : MonoBehaviour
{
    public GameObject filamentRoller;
    public GameObject filamentPrefab;
    public Transform spawnFilament; // Unity �����Ϳ��� ���� ����
    float rotSpeed = 200;
    private GameObject filament; // ���� ������ filament ����
    MeshRenderer filamentMesh;

    public Transform roadStart1; // ���� ��ġ1
    public Transform roadEnd1; // �� ��ġ1
    public float speed = 2.0f; // �̵� �ӵ�

    public Transform filamentLocation; // �ʶ��Ʈ ��ġ
    public GameObject itemPrefab;
    GameObject examItem;
    public Transform itemSpawnLocation;
    public Transform dropLocation;
    public float dropSpeed = 2;

    public Transform AGVLocation;
    public Transform AGVstartPos;
    public Transform AGVendPos;

    public Transform roadStart2; // ���� ��ġ2
    public Transform roadEnd2; // �� ��ġ2

    public GameObject boxPrefab;
    GameObject boxObj;
    public Transform boxLocation; // �ڽ� ����

    public GameObject panel;
    public GameObject text;


    private void Start()
    {
        if (spawnFilament == null)
        {
            Debug.LogError("spawnFilament�� �������� �ʾҽ��ϴ�. Unity �����Ϳ��� ������ �ּ���.");
        }
        else
        {
            spawnFilament.position = filamentRoller.transform.position; // �ʱ� ��ġ ����
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
            if (filament == null) // ���� filament�� ���� ��쿡�� ����
            {
                filament = Instantiate(filamentPrefab, spawnFilament.position, Quaternion.Euler(90, 0, 0));
                filamentMesh = filament.GetComponent<MeshRenderer>();
                filamentMesh.material.color = Color.red;
            }
            else
            {
                Debug.LogWarning("�̹� filament�� �����Ǿ� �ֽ��ϴ�.");
            }
        }
        else
        {
            Debug.LogError("spawnFilament�� null�Դϴ�. �ʶ��Ʈ�� ������ �� �����ϴ�.");
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
                Debug.LogError("FilamentMachine���� filament�� ã�� �� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("FilamentMachine�� ã�� �� �����ϴ�.");
        }

        if (filament != null) // filament�� �����ϴ� ��쿡�� �̵�
        {
            StartCoroutine(RoadMove(roadStart1, roadEnd1, filament));
        }
        else
        {
            Debug.LogError("�̵��� filament�� �����ϴ�.");
        }
    }

    IEnumerator RoadMove(Transform start, Transform end, GameObject obj)
    {
        obj.transform.position = start.position;
        obj.transform.rotation = Quaternion.Euler(0, 0, 0);

        while (Vector3.Distance(obj.transform.position, end.position) > 0.1f)
        {
            // ���� ��ġ�� ��ǥ ��ġ ������ ���� ���� ���
            Vector3 direction = (end.position - obj.transform.position).normalized;

            // �ӵ��� ���� �̵�
            obj.transform.position += direction * speed * Time.deltaTime;

            yield return new WaitForEndOfFrame(); // ���� �����ӱ��� ���
        }
        // ���� ��ġ ����
        obj.transform.position = end.position;
        // print(filament);
    }

    public void OnFilamentLocationBtn()
    {
        if (filament != null) // filament�� �����ϴ��� Ȯ��
        {
            filament.transform.position = filamentLocation.position; // ��ġ ����
            filament.transform.rotation = Quaternion.Euler(90, 0, 0); // ȸ�� ����
        }
        else
        {
            Debug.LogError("���� filament�� �����ϴ�.");
        }
        
    }
    public void OnOperationBtn()
    {
        filament.transform.Rotate(0, 0, rotSpeed * Time.deltaTime);

        // ������ ����
        examItem = Instantiate(itemPrefab);
        examItem.transform.position = itemSpawnLocation.position;

        // ��� ��ġ�� �̵�
        StartCoroutine(MoveToDropLocation(examItem, dropLocation));
    }

    // ��� ��ġ�� �̵��ϴ� �ڷ�ƾ
    IEnumerator MoveToDropLocation(GameObject item, Transform dropLoc)
    {
        while (Vector3.Distance(item.transform.position, dropLoc.position) > 0.1f)
        {
            // ���� ��ġ�� ��� ��ġ ������ ���� ���� ���
            Vector3 direction = (dropLoc.position - item.transform.position).normalized;

            // �ӵ��� ���� �̵�
            item.transform.position += direction * dropSpeed * Time.deltaTime;

            yield return new WaitForEndOfFrame(); // ���� �����ӱ��� ���
        }
        // ���� ��ġ ����
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
        if (examItem != null) // examItem�� �����ϴ� ��쿡�� �̵�
        {
            StartCoroutine(RoadMove(roadStart2, roadEnd2, examItem));
        }
        else
        {
            Debug.LogError("�̵��� examItem�� �����ϴ�.");
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
