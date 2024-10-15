using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BoxingMachine : MonoBehaviour
{
    public GameObject[] flatBox;
    public Transform initialBoxPlate;

    ObjectDestroy ObjectDestroy;
    GameObject newBox;
    int boxCnt;

    private void Start()
    {
        ObjectDestroy = FindAnyObjectByType<ObjectDestroy>();
    }

    private void Update()
    {
        if (newBox == null)
            boxCnt = 0;

        if (boxCnt == 0)
        {
            StartCoroutine(FlatBoxCreate());
        }
    }

    private IEnumerator FlatBoxCreate()
    {
        int rand = UnityEngine.Random.Range(0, flatBox.Length);
        
        newBox = Instantiate(flatBox[rand], initialBoxPlate.position, Quaternion.Euler(0, 0, 90));
        boxCnt++;

        yield return new WaitForSeconds(1f);
    }
}