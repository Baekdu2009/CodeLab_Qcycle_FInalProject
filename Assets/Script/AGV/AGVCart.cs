using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class AGVCart : MonoBehaviour
{
    public GameObject PlateCollider;
    public Image FullCheck;
    public TMP_Text BoxFullTxt;
    public GameObject callAGVBtn;
    bool plateIsFull;
    public int boxFullNum;
    public bool isAGVCallOn;
    public int colliderCount = 0;

    private void Start()
    {
        Collider plateCollider = PlateCollider.GetComponent<Collider>();
        plateCollider.isTrigger = true;
        callAGVBtn.SetActive(false);
    }

    private void Update()
    {
        CallBtnOn();
    }

    public void IncrementColliderCount()
    {
        colliderCount++;
    }

    private void CallBtnOn()
    {
        if (colliderCount > boxFullNum)
        {
            FullCheck.color = Color.red;
            callAGVBtn.SetActive(true);
            BoxFullTxt.text = "Box Full";
        }
        else
        {
            FullCheck.color = Color.green;
            callAGVBtn.SetActive(false);
            BoxFullTxt.text = "";
        }
    }

    public void OnAGVCall()
    {
        isAGVCallOn = true;
    }
}