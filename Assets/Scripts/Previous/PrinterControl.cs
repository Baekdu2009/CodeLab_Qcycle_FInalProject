using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PrinterControl : MonoBehaviour
{
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

    public GameObject objTrigger;
    public Transform printingPos;
    int powderCnt;
    

    void Start()
    {
        nozzleOriginPos = nozzle.transform.position;
        pos = nozzle.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        KeyInputMoving();
        TransformPosition(printingPos);
        TransformPosition(objTrigger);
    }
    void KeyInputMoving()
    {
        Vector3 nozzleVector = nozzle.transform.position;

        // AŰ�� DŰ�� z�� �̵�
        if (Input.GetKey(KeyCode.A))
        {
            nozzleVector.z -= nozzleSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            nozzleVector.z += nozzleSpeed * Time.deltaTime;
        }

        // WŰ�� SŰ�� x�� �̵�
        if (Input.GetKey(KeyCode.W))
        {
            nozzleVector.x -= nozzleSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            nozzleVector.x += nozzleSpeed * Time.deltaTime;
        }

        // ���� ȭ��ǥ�� �Ʒ��� ȭ��ǥ�� y�� �̵�
        if (Input.GetKey(KeyCode.UpArrow))
        {
            nozzleVector.y += nozzleSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            nozzleVector.y -= nozzleSpeed * Time.deltaTime;
        }

        // Space Ű�� ������ �� ������Ʈ ���� ����/����
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

        // �������� ������ �����մϴ�.
        nozzleVector.x = Mathf.Clamp(nozzleVector.x, nozzleOriginPos.x - xRange, nozzleOriginPos.x + xRange);
        nozzleVector.y = Mathf.Clamp(nozzleVector.y, yMin, yMax);
        nozzleVector.z = Mathf.Clamp(nozzleVector.z, nozzleOriginPos.z - zRange, nozzleOriginPos.z + zRange);

        // ���� ��ġ�� ������Ʈ�մϴ�.
        nozzle.transform.position = nozzleVector;
    }

    IEnumerator ItemCreation()
    {
        while (true) // ���� ������ ���� ��� ����
        {
            powderItem = Instantiate(powderPrefab, nozzleTip);
            powderItem.transform.position = nozzleTip.position;
            powderItem.transform.parent = null;

            yield return new WaitForSeconds(0.1f); // 1�� ���
        }
    }
    Vector3 TransformPosition(Transform trans)
    {
        Vector3 pos = trans.position;
        pos.x = nozzle.transform.position.x;
        pos.y = nozzle.transform.position.y - 0.3f;
        pos.z = nozzle.transform.position.z;
        trans.position = pos;
        return pos;
    }
    Vector3 TransformPosition(GameObject obj)
    {
        Vector3 pos = obj.transform.position;
        pos.x = printingPos.position.x;
        pos.y = 0.3f;
        pos.z = printingPos.position.z;
        obj.transform.position = pos;
        return pos;
    }
}
