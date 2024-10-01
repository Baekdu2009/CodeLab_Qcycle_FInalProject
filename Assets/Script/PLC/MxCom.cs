using UnityEngine;
using ActUtlType64Lib;
using System.Linq;
using System;


public class MxCom : FilamentFactory
{
    [SerializeField] private FilamentFactory filamentFactory;
    public enum Status
    {
        CONNECTED,
        DISCONNECTED
    };

    ActUtlType64 mxComponent;
    [Header("연결과 데이터 전송")]
    [SerializeField] Status status = Status.DISCONNECTED;
    [SerializeField] float scanTime = 1;
    [SerializeField] int blockNum = 1;

    [Header("설비 연결")]
    [SerializeField] ConveyBelt conveyorA;
    

    [SerializeField] LevelSensor sensorA;
    /*[SerializeField] LevelSensor sensorB;
    [SerializeField] LevelSensor sensorC;*/

    public int[][] pointY;
    public int[][] pointX;

    private void Start()
    {
        mxComponent = new ActUtlType64();

        mxComponent.ActLogicalStationNumber = 0;

        if (filamentFactory == null)
        {
            filamentFactory = FindObjectOfType<FilamentFactory>(); // 씬에서 찾아서 할당
        }


        InvokeRepeating("ScanPLC", 1, scanTime);

    }

    private void ScanPLC()
    {

        if (status == Status.DISCONNECTED) return;

        pointY = ReadDeviceBlock("Y0");
        WriteDeviceBlock("Y0", pointY);
        pointX = ReadDeviceBlock("X0");
        WriteDeviceBlock("X0", pointX);


        //컨베이어
        int runConveyor = pointY[0][1];

        //탱크 제어
        int Tank1 = pointY[0][5];
        int Tank2 = pointY[0][6];


        //모터릴레이
        int runShrreder = pointY[2][0];
        int runExtruder1 = pointY[2][1];
        int runWasher1 = pointY[2][2];
        int runCuttingMachine = pointY[2][3];
        int runHooper = pointY[2][4];
        int runExtruder2 = pointY[2][5];
        int runWasher2 = pointY[2][6];
        int runPullyMachine = pointY[2][7];

        //센서

        int Level1 = pointX[5][0];
        int Level2 = pointX[5][1];
        int LimitSwitch = pointX[5][2];

        if (runConveyor == 1)
        {
            conveyorA.OnStartConveyorBtnClkEvent();
            filamentFactory.StatusCheck(conveyorStatus, runConveyor, 0);
        }

    }





    public void OnConnectBtnClkEvent()
    {
        if (status == Status.CONNECTED)
        {
            print("이미 연결되었습니다.");
            return;
        }

        int ret = mxComponent.Open();

        if (ret == 0)
        {
            print("PLC에 연결되었습니다.");
            status = Status.CONNECTED;
        }
        else
        {
            print("PLC연결에 실패했습니다. " + Convert.ToString(ret, 16));
        }
    }

    public void OnDisconnectBtnClkEvent()
    {
        if (status == Status.DISCONNECTED)
        {
            print("이미 연결이 해제되었습니다.");
            return;
        }

        int ret = mxComponent.Close();

        if (ret == 0)
        {
            print("PLC 연결을 해제하였습니다.");
            status = Status.DISCONNECTED;
        }
        else
        {
            print("PLC연결 해제에 실패했습니다. " + Convert.ToString(ret, 16));
        }
    }

    int[] devices;
    public int[] ReadDeviceBlock(string deviceName, int blockNum)
    {
        devices = new int[blockNum];
        int ret = mxComponent.ReadDeviceBlock(deviceName, blockNum, out devices[0]);

        if (ret == 0)
        {
            foreach (int device in devices)
            {
                print(device);
            }

            return devices;
        }
        else
        {
            print("ERROR" + ret);

            return null;
        }

    }


    //ReadDeviceBlock 했을 때 

    private int[][] ReadDeviceBlock(string deviceName)
    {
        int[] values = new int[blockNum];
        int[][] information = new int[values.Length][];

        values = ReadDeviceBlock(deviceName, values.Length);

        int i = 0;
        foreach (var value in values)
        {
            string binary = Convert.ToString(value, 2);  //2진수 변환
            print(binary);

            information[i] = ConvertStringToIntArray(binary);

            i++;
        }

        return information;
    }

    private static int[] ConvertStringToIntArray(string binary)
    {
        // 1. 문자열의 길이 파악
        int strLength = binary.Length; // 5
        int zeroNum = 16 - strLength; // 11개 추가필요

        // 2. 16비트 형태로 문자열 추가
        // 00001 -> 0000100000000000
        string newBinary = new string(binary.Reverse().ToArray());
        // binary.Reverse();
        for (int i = 0; i < zeroNum; i++)
        {
            newBinary += "0";
        }

        // 3. 문자열 -> 문자열 배열
        char[] strings = newBinary.ToCharArray();

        // 4. 문자열 배열 -> 정수형 배열
        int[] devicePoints = Array.ConvertAll(strings, c => (int)Char.GetNumericValue(c));

        return devicePoints;
    }

    public void WriteDeviceBlock(string deviceName, int[][] Point)
    {

        int sensorAValue = (sensorA.isDetected == true) ? 1 : 0;
        /*int sensorBValue = (sensorB.isDetected == true) ? 1 : 0;
        int sensorCValue = (sensorC.isDetected == true) ? 1 : 0;*/

        int sensorNum = /*sensorCValue * 4 + sensorBValue * 2*/  sensorAValue * 1;

        int[] data = new int[blockNum];
        data[0] = devices[0];
        data[1] = devices[1];
        data[5] = sensorNum;

        mxComponent.WriteDeviceBlock(deviceName, blockNum, ref data[0]);
    }



}
