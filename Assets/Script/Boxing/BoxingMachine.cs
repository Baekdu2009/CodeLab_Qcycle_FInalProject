using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BoxingMachine : MonoBehaviour
{
    public GameObject[] flatBox;
    public Transform initialBoxPlate;

    int boxCnt;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (boxCnt < 10)
        {
            StartCoroutine(FlatBoxCreate());
        }
    }

    private IEnumerator FlatBoxCreate()
    {
        Instantiate(flatBox[0]);
        Instantiate(flatBox[1]);
        flatBox[0].transform.position = initialBoxPlate.transform.position;
        flatBox[1].transform.position = initialBoxPlate.transform.position;

        boxCnt += 2;

        yield return new WaitForSeconds(1f);
    }
}
