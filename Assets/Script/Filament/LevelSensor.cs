using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSensor : MonoBehaviour
{
    [SerializeField] private int collisionCount = 0; // 충돌 수
    public bool isDetected = false; // 감지 상태
    [SerializeField] public string plasticTag = "Plastic1"; // 설정할 태그

    private HashSet<Collider> collidedPlastics = new HashSet<Collider>();
    private LevelSensorExtruder extruder;
    private Coroutine reduceCoroutine;
    private int sensingChangeCount = 0;
    private bool lastIsSensingState = false;

    private void Start()
    {
        extruder = FindObjectOfType<LevelSensorExtruder>();
        if (extruder == null)
        {
            Debug.LogError("LevelSensorExtruder를 찾을 수 없습니다.");
        }
    }

    private void Update()
    {
        if (collisionCount >= 3000 && !isDetected)
        {
            isDetected = true;
        }

        if (!extruder.isSensing && !isDetected)
        {
            if (reduceCoroutine == null)
            {
                reduceCoroutine = StartCoroutine(ReducePlasticCount(0.2f));
            }
        }

        if (extruder.isSensing)
        {
            if (!lastIsSensingState)
            {
                lastIsSensingState = true;
            }
        }
        else
        {
            if (lastIsSensingState)
            {
                sensingChangeCount++;
                lastIsSensingState = false;
            }
        }

        if (sensingChangeCount >= 4)
        {
            ResetDetection();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(plasticTag))
        {
            if (!isDetected)
            {
                if (!collidedPlastics.Contains(other))
                {
                    collidedPlastics.Add(other);
                    collisionCount++;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(plasticTag))
        {
            collidedPlastics.Remove(other);
        }
    }

    private IEnumerator ReducePlasticCount(float percentage)
    {
        while (!extruder.isSensing)
        {
            GameObject[] allPlastics = GameObject.FindGameObjectsWithTag(plasticTag);
            int totalPlastics = allPlastics.Length;

            if (totalPlastics > 0)
            {
                int countToRemove = Mathf.CeilToInt(totalPlastics * percentage);

                for (int i = 0; i < countToRemove && totalPlastics > 0; i++)
                {
                    int randomIndex = Random.Range(0, totalPlastics);
                    GameObject plasticToRemove = allPlastics[randomIndex];

                    if (plasticToRemove != null)
                    {
                        Destroy(plasticToRemove);
                        allPlastics = GameObject.FindGameObjectsWithTag(plasticTag);
                        totalPlastics = allPlastics.Length;
                    }
                    yield return new WaitForSeconds(0.1f);
                }

                yield return new WaitForSeconds(3f);
            }
            else
            {
                break;
            }
        }

        reduceCoroutine = null;
    }

    private void ResetDetection()
    {
        isDetected = false;
        collisionCount = 0;
        sensingChangeCount = 0;
        collidedPlastics.Clear();
    }
}