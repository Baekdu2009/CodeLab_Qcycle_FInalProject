using UnityEngine;
using System.Collections;

public class Road : MonoBehaviour
{
    public Transform roadStart; // ���� ��ġ
    public Transform roadEnd; // �� ��ġ
    public float speed = 2.0f; // �̵� �ӵ�
    private GameObject filament; // �ν��Ͻ�ȭ�� filament
    FilamentMachine filamentMachine;

    void Start()
    {
        filamentMachine = FindAnyObjectByType<FilamentMachine>();
    }

    public void OnRoadMoveBtn()
    {
        if (filamentMachine != null)
        {
            filament = filamentMachine.GetCurrentFilament(); // filament ��������

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
            StartCoroutine(RoadMove(roadStart, roadEnd));
        }
        else
        {
            Debug.LogError("�̵��� filament�� �����ϴ�.");
        }
    }

    IEnumerator RoadMove(Transform start, Transform end)
    {
        filament.transform.position = start.position;
        filament.transform.rotation = Quaternion.Euler(0, 0, 0);

        while (Vector3.Distance(filament.transform.position, end.position) > 0.1f)
        {
            // ���� ��ġ�� ��ǥ ��ġ ������ ���� ���� ���
            Vector3 direction = (end.position - filament.transform.position).normalized;

            // �ӵ��� ���� �̵�
            filament.transform.position += direction * speed * Time.deltaTime;

            yield return null; // ���� �����ӱ��� ���
        }

        // ���� ��ġ ����
        filament.transform.position = end.position;
    }
    public GameObject GetCurrentFilament()
    {
        return filament; // ���� filament ��ȯ
    }
}
