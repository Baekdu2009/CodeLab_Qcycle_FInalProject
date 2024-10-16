using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;

public class AGVCart : MonoBehaviour
{
    public GameObject PlateCollider;
    public Image FullCheck;
    public TMP_Text BoxFullTxt;
    public GameObject callAGVBtn;
    public GameObject Canvas;

    bool plateIsFull;
    int boxFullNum = 19;
    int colliderCount = 0;
   //private Quaternion initialRotationValue;

    [HideInInspector]
    public bool isAGVCallOn;
    // Vector3 initialSpawnposition;
    // Quaternion initialSpawnrotation;

    private void Start()
    {
        Collider plateCollider = PlateCollider.GetComponent<Collider>();
        plateCollider.isTrigger = true;
        callAGVBtn.SetActive(false);

        //initialRotationValue = transform.rotation;
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

    public void SetAGVCallState(bool state)
    {
        isAGVCallOn = state;
    }
    /*public void ResetRotatior()
    {
        transform.rotation = initialRotationValue;
    }
*/
}