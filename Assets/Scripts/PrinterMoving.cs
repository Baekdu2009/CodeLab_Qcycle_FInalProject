using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PrinterMoving : MonoBehaviour
{
    public Transform nozzle;
    public Transform rod;
    public Transform plate;
    public GameObject filament;

    public float Xmin;
    public float Xmax;
    public float Ymin;
    public float Ymax;
    public float Zmin;
    public float Zmax;
    public float nozzleSpeed = 0.1f;
    public float rodSpeed = 0.1f;
    public float plateSpeed = 0.1f;
    public float filamentRotSpeed = 200;

    float nozzleY;
    float rodZ;
    float plateX;

    bool movingX = false;
    bool movingY = false;
    bool movingZ = false;

    Coroutine NozzleRoutine;
    Coroutine RodRoutine;
    Coroutine PlateRoutine;

    bool isNozzleRoutine;
    bool isRodRoutine;
    bool isPlateRoutine;

    private void Start()
    {
        
    }

    private void Update()
    {
        // MovingbyKey();
        RotateFilament();
    }

    //private void MovingbyKey()
    //{
    //    if (printer.activeSelf == true)
    //    {
    //        if (Input.GetKey(KeyCode.A))
    //        {
    //            nozzleY = Mathf.Clamp(nozzleY + Time.deltaTime * speed, Ymin, Ymax);
    //        }
    //        if (Input.GetKey(KeyCode.D))
    //        {
    //            nozzleY = Mathf.Clamp(nozzleY - Time.deltaTime * speed, Ymin, Ymax);
    //        }
    //        if (Input.GetKey(KeyCode.S))
    //        {
    //            rodZ = Mathf.Clamp(rodZ - Time.deltaTime * speed, Zmin, Zmax);
    //        }
    //        if (Input.GetKey(KeyCode.W))
    //        {
    //            rodZ = Mathf.Clamp(rodZ + Time.deltaTime * speed, Zmin, Zmax);
    //        }
    //        if (Input.GetKey(KeyCode.DownArrow))
    //        {
    //            plateX = Mathf.Clamp(plateX + Time.deltaTime * speed, Xmin, Xmax);
    //        }
    //        if (Input.GetKey(KeyCode.UpArrow))
    //        {
    //            plateX = Mathf.Clamp(plateX - Time.deltaTime * speed, Xmin, Xmax);
    //        }

    //        nozzle.localPosition = new Vector3(nozzle.localPosition.x, nozzleY, nozzle.localPosition.z);
    //        rod.localPosition = new Vector3(rod.localPosition.x, rod.localPosition.y, rodZ);
    //        plate.localPosition = new Vector3(plateX, plate.localPosition.y, plate.localPosition.z);
    //    }
    //}

    public void NozzlePosition()
    {
        print($"Nozzle Global Position: {nozzle.position}");
        print($"Rod Global Position: {rod.position}");
    }
    public void NozzleLocalPosition()
    {
        print($"Nozzle Local Position: {nozzle.localPosition}");
        print($"Rod Local Position: {rod.localPosition}");
    }

    IEnumerator NozzleMovingAuto()
    {
        float ydir;

        while (true)
        {
            if (movingY)
            {
                ydir = 1;
                if (nozzle.localPosition.y >= Ymax)
                    movingY = false;
            }
            else
            {
                ydir = -1;
                if (nozzle.localPosition.y <= Ymin)
                    movingY = true;
            }

            nozzleY = Mathf.Clamp(nozzleY + Time.deltaTime * nozzleSpeed * ydir, Ymin, Ymax);

            nozzle.localPosition = new Vector3(nozzle.localPosition.x, nozzleY, nozzle.localPosition.z);

            yield return null;
        }
    }
    IEnumerator RodMovingAuto()
    {
        float zdir;

        while (true)
        {
            if (movingZ)
            {
                zdir = 1;
                if (rod.localPosition.z >= Zmax)
                    movingZ = false;
            }
            else
            {
                zdir = -1;
                if (rod.localPosition.z <= Zmin)
                    movingZ = true;
            }

            rodZ = Mathf.Clamp(rodZ + Time.deltaTime * rodSpeed * zdir, Zmin, Zmax);

            rod.localPosition = new Vector3(rod.localPosition.x, rod.localPosition.y, rodZ);

            yield return null;
        }
    }
    IEnumerator PlateMovingAuto()
    {
        float xdir;

        while (true)
        {
            if (movingX)
            {
                xdir = 1;
                if (plate.localPosition.x >= Xmax)
                    movingX = false;
            }
            else
            {
                xdir = -1;
                if (plate.localPosition.x <= Xmin)
                    movingX = true;
            }

            plateX = Mathf.Clamp(plateX + Time.deltaTime * plateSpeed * xdir, Xmin, Xmax);

            plate.localPosition = new Vector3(plateX, plate.localPosition.y, plate.localPosition.z);

            yield return null;
        }
    }
    private void RotateFilament()
    {
        if (filament != null)
        {
            // 현재 회전 상태를 가져옴
            Quaternion currentRotation = filament.transform.localRotation;

            // Y축을 기준으로 회전할 각도 계산
            Quaternion deltaRotation = Quaternion.Euler(0, filamentRotSpeed * Time.deltaTime, 0);

            // 새로운 회전 상태 계산
            filament.transform.localRotation = currentRotation * deltaRotation;
        }
    }
    public void OnBtnNozzlePlay()
    {
        if (isNozzleRoutine)
        {
            if (NozzleRoutine != null)
            {
                StopCoroutine(NozzleRoutine);
                NozzleRoutine = null;
            }
        }
        else
        {
            NozzleRoutine = StartCoroutine(NozzleMovingAuto());
        }
        isNozzleRoutine = !isNozzleRoutine;
    }
    public void OnBtnRodPlay()
    {
        if (isRodRoutine)
        {
            if (RodRoutine != null)
            {
                StopCoroutine(RodRoutine);
                RodRoutine = null;
            }
        }
        else
        {
            RodRoutine = StartCoroutine(RodMovingAuto());
        }
        isRodRoutine = !isRodRoutine;
    }
    public void OnBtnPlatePlay()
    {
        if (isPlateRoutine)
        {
            if (PlateRoutine != null)
            {
                StopCoroutine(PlateRoutine);
                PlateRoutine = null;
            }
        }
        else
        {
            PlateRoutine = StartCoroutine(PlateMovingAuto());
        }
        isPlateRoutine = !isPlateRoutine;
    }
}