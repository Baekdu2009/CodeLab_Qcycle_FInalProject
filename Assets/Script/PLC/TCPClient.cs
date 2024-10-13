using UnityEngine;
using System.Net.Sockets;
using System.Collections;
using System.Text;
using System;
using TMPro;
using System.Threading.Tasks;
using UnityEditor.Rendering;
using static UnityEngine.InputSystem.Controls.AxisControl;
using UnityEngine.InputSystem;
using System.Linq;
using System.IO;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using Unity.Collections.LowLevel.Unsafe;
public class TCPClient : MonoBehaviour
{

    [Header("연결과 데이터 전송에 대한 부분입니다.")]
    
    [SerializeField] bool isConnected = false;
    [SerializeField] string dataToServerY;
    [SerializeField] string dataFromServerY;
    [SerializeField] int[][] pointY;
    [SerializeField] string dataToServerX;
    [SerializeField] string dataFromServerX;
    [SerializeField] int[][] pointX;
    [SerializeField] float scanTime = 0.1f;
    [SerializeField] string startPoint1 = "Y0";
    [SerializeField] string startPoint2 = "X0";
    [SerializeField] int blockNum = 8;
    TcpClient client;
    NetworkStream stream;


    [Header("설비들을 연결합니다.")]
    [SerializeField] EachFilamentFactory filamentFactory;
    Conveyor conveyor;
    Shredder shredder;
    [SerializeField] LevelSensor[] tankSensor;
    LevelSensorExtruder[] extruderSensor;
    PressureSensor[] pressureSensor;
    [SerializeField] FilamentLine[] linemanagers;
    WireCutting wireCutting;
    ScrewBelt screwBelt;
    PlasticSpawn[] plasticSpawn;
   
    public bool cooling1;
    
    private void Awake()
    {
        //ServerConnect serverConnect = GetComponent<ServerConnect>();

        //if (serverConnect != null)
        //{
        //    serverConnect.RunTCPServer();
        //}
        //else
        //{
        //    UnityEngine.Debug.LogError("ServerConnect 인스턴스를 찾을 수 없습니다.");
        //}
    }
    private void Start()
    {
        
        // 로컬호스트: 로컬 컴퓨터의 디폴트 IP
        FactoryMachineStart();

        try
        {
            client = new TcpClient("127.0.0.1", 7000);

            stream = client.GetStream();
        }
        catch (Exception e)
        {
            print(e.ToString());
            print("서버를 먼저 켜주세요.");
        }
    }

    private void FactoryMachineStart()
    {
        conveyor = filamentFactory.conveyor;
        shredder = filamentFactory.shredder;
        wireCutting = filamentFactory.wireCutting;
        screwBelt = filamentFactory.screwBelt;

        linemanagers = filamentFactory.linemanagers; // 배열을 직접 할당
        tankSensor = filamentFactory.levelSensors; // 배열을 직접 할당
        plasticSpawn = filamentFactory.plasticSpawn; // 배열을 직접 할당
        extruderSensor = filamentFactory.extruder; // 배열을 직접 할당
        pressureSensor = filamentFactory.pressureSensor; // 배열을 직접 할당
    }

    private int[][] ReadDeviceBlock(string dataFromServer)
    {
        //print("1. ReadDeviceBlock dataFromServer: " + dataFromServer);

        // 문자열을 분리
        string[] strSplited = dataFromServer.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        //print("2. ReadDeviceBlock newData: " + string.Join(",", strSplited));

        // 배열의 크기를 strSplited의 길이에 맞추기
        int[] values = new int[strSplited.Length];

        for (int i = 0; i < strSplited.Length; i++)
        {
            if (int.TryParse(strSplited[i], out values[i]))
            {
                // 변환 성공
            }
            else
            {
                UnityEngine.Debug.LogError($"변환 실패: '{strSplited[i]}'는 정수로 변환할 수 없습니다.");
                values[i] = 0; // 기본값 설정
            }
        }

        // 2차원 배열로 변환
        int[][] information = new int[values.Length][];

        for (int i = 0; i < values.Length; i++)
        {
            string binary = Convert.ToString(values[i], 2).PadLeft(16, '0'); // 16자리로 패딩
            information[i] = ConvertStringToIntArray(binary);
        }

        return information;
    }

    //2진수로 바뀐 수에 16개 비트에서 빼서 16비트 숫자처럼 만들기
    private static int[] ConvertStringToIntArray(string binary)
    {

        int strLength = binary.Length;
        int zeroNum = 16- strLength;

        string newBinary = new string(binary.Reverse().ToArray());

        for(int i = 0; i<zeroNum; i++)
        {
            newBinary += "0";
        }

        char[] strings = newBinary.ToCharArray();

        int[] devicePoints = Array.ConvertAll(strings, c => (int)Char.GetNumericValue(c));

        return devicePoints;
    }

    public string WriteDeivceBlock()
    {
        int tank1SensorValue = (tankSensor[0].isDetected == true) ? 1 : 0;
        int tank2SensorValue = (tankSensor[1].isDetected == true) ? 1 : 0;
        int extruder1SensorValue = (extruderSensor[0].isSensing == true) ? 1 : 0;
        int extruder2SensorValue = (extruderSensor[1].isSensing == true) ? 1 : 0;
        int pressure1SensorValue = (pressureSensor[0].isPressing == true) ? 1 : 0;
        int pressure2SensorValue = (pressureSensor[1].isPressing == true) ? 1 : 0;
        int ls1value = (tankSensor[0].isDetected == true) ? 1 : 0;

        int sensorNum = pressure2SensorValue * 32 + pressure1SensorValue*16 +extruder2SensorValue*8 +extruder1SensorValue * 4 + tank2SensorValue*2 + tank1SensorValue * 1;
        int lsNum = ls1value * 1;

        string sensorData = sensorNum.ToString() +"," + lsNum.ToString();

        return sensorData;
    }
    
    private void ScanPLC()
    {

        try
        {

            string sensorData = WriteDeivceBlock();
            dataToServerY = $"GET,{startPoint1},{blockNum},SET,{startPoint1},{sensorData}";
            dataToServerX = $"GET,{startPoint2},{blockNum},SET,{startPoint2},{sensorData}";


            pointY = ReadDeviceBlock(dataFromServerY);
            pointX = ReadDeviceBlock(dataFromServerX);
            
          
            //장비
            int runConveyor = pointY[0][1];
            int runShreder = pointY[2][0];
            int runExtruder1 = pointY[2][1];
            int runCooler1 = pointY[2][2];
            int runCuttingMachine = pointY[2][3];
            int runScrewBelt = pointY[2][4];
            int runExtruder2 = pointY[2][5];
            int runCooler2 = pointY[2][6];
            int runPullyMachine = pointY[2][7];

            for (int i = 0; i < pointY.Length; i++)
            {
                for (int j = 0; j < pointY[i].Length; j++)
                {
                    print($"PointY[{i}][{j}]값 : {pointY[i][j]}");
                }
            }

            //전원
            int processStart = pointX[0][2];    
            
            //센서

            int tank1Level = pointX[5][0];
            int tank2Level = pointX[5][1];
            int Extruder1Level = pointX[5][2];
            int Extruder2Level = pointX[5][3];
            int Extruder1Pressure = pointX[5][4];
            int Extruder2Pressure = pointX[5][5];
       
            //리미트
            int limitSwitch1 = pointX[6][0];

            if (runConveyor == 1)
            {
                conveyor.conveyorRunning = true;
            }

            else if (runConveyor != 1)
            {
                conveyor.conveyorRunning = false;
            }
            if(runShreder == 1)
            {
                shredder.isRunning = true;
            }
            else if (runShreder != 1)
            {
                shredder.isRunning = false;
            }
            if (runExtruder1 == 1)
            {
                linemanagers[0].isWorking = true;
                linemanagers[0].isOn = true;
            }
            else if (runExtruder1 != 1)
            {
                linemanagers[0].isWorking = false;
                linemanagers[0].isOn = false;
            }
            if (runCooler1 == 1)
            {
                cooling1 = true;
            }
            else if (runCooler1 != 1)
            {
                cooling1 = false;
            }
            if (runCuttingMachine == 1)
            {
                wireCutting.isWorking = true;
                plasticSpawn[1].isOn = true;
                
            }
            else if (runCuttingMachine != 1)
            {
                wireCutting.isWorking = false;
                plasticSpawn[1].isOn = false;

            }
            if (runScrewBelt == 1)
            {
                screwBelt.isWorking = true;
            }
            else if (runScrewBelt != 1)
            {
                screwBelt.isWorking = false;
            }

            //X제어

            if (tankSensor[0].isDetected == true)
            {
                tank1Level = 1;
            }
            else if (tankSensor[0].isDetected == false)
            {
                tank1Level = 0;
            }
            if (tankSensor[1].isDetected == true)
            {
                tank2Level = 1;
            }
            else if (tankSensor[1].isDetected == false)
            {
                tank2Level = 0;
            }
           if( extruderSensor[0].isSensing == true)
            {
                Extruder1Level = 1;
            }
            else if( extruderSensor[0].isSensing == false)
            {
                Extruder1Level = 0;
            }
            if (extruderSensor[1].isSensing == true)
            {
                Extruder2Level = 1;
            }
            else if (extruderSensor[1].isSensing == false)
            {
                Extruder2Level = 0;
            }

            if (pressureSensor[0].isPressing == true)
            {
                Extruder1Pressure = 1;
            }
            else if (pressureSensor[0].isPressing == false)
            {
                Extruder1Pressure = 0;
            }
            if (pressureSensor[1].isPressing == true)
            {
                Extruder2Pressure = 1;
            }
            else if (pressureSensor[1].isPressing == false)
            {
                Extruder2Pressure = 0;
            }

        }

        catch (Exception ex)
        {
            print(ex.ToString());
        }
    }

    private void Request(string order)
    {
        try
        {
            byte[] buffer = Encoding.UTF8.GetBytes(order);

            //Steam에 데이터 쓰기
            stream.Write(buffer, 0, buffer.Length);

            //데이터 수신
            byte[] buffer2 = new byte[1024];
            int nBytes = stream.Read(buffer2, 0, buffer2.Length);
            string msg = Encoding.UTF8.GetString(buffer2, 0, nBytes);
            print(msg);

            if (msg.Contains("PLC에 연결되었습니다."))
                isConnected = true;
        }
        catch (Exception e)
        {
            print(e.ToString());
        }
    }

    private async Task RequestAsync()
    {
        while (true)
        {
            try
            {
                // Y 데이터 요청
                await stream.WriteAsync(Encoding.UTF8.GetBytes(dataToServerY), 0, dataToServerY.Length);
                // 데이터 수신
                byte[] bufferY = new byte[1024];
                int nBytesY = await stream.ReadAsync(bufferY, 0, bufferY.Length);
                string msgY = Encoding.UTF8.GetString(bufferY, 0, nBytesY);

                if (!string.IsNullOrEmpty(msgY))
                {
                    dataFromServerY = msgY; // Y 데이터
                    pointY = ReadDeviceBlock(dataFromServerY);
                }

                // X 데이터 요청
                await stream.WriteAsync(Encoding.UTF8.GetBytes(dataToServerX), 0, dataToServerX.Length);
                // 데이터 수신
                byte[] bufferX = new byte[1024];
                int nBytesX = await stream.ReadAsync(bufferX, 0, bufferX.Length);
                string msgX = Encoding.UTF8.GetString(bufferX, 0, nBytesX);

                if (!string.IsNullOrEmpty(msgX))
                {
                    dataFromServerX = msgX; // X 데이터
                    pointX = ReadDeviceBlock(dataFromServerX);
                }

                if (!isConnected) break;
            }
            catch (Exception e)
            {
                print(e.ToString());
            }
        }
    }

    public void OnAsyncBtnClkEvent()
    {
        if (isConnected)
            Task.Run(() => RequestAsync());
        else
            print("연동해제 상태입니다. Connect 버튼을 클릭해 주세요.");
    }

    private void OnDestroy()
    {
        // PLC 연결 종료
        if (client != null)
        {
            stream.Close();
            client.Close();
            client = null;
            UnityEngine.Debug.Log("PLC와의 연결이 종료되었습니다.");
        }

        // ServerConnect 인스턴스에서 TCP 서버 종료
        ServerConnect serverConnect = FindAnyObjectByType<ServerConnect>();
        if (serverConnect != null)
        {
            serverConnect.StopTCPServer();
        }

        isConnected = false;
    }

    IEnumerator UpdateScan()
    {
        yield return new WaitUntil(() => isConnected);

        print("UpdateScan");

        // 1. Server에 데이터를 요청하고 받는역할
        OnAsyncBtnClkEvent();

        while (isConnected)
        {
            // 2. 요청해서 받은 데이터를 Unity의 설비에 적용하는 역할
            ScanPLC();
            
            yield return new WaitForSeconds(scanTime);
        }
    }

    public void OnConnectBtnClkEvent()
    {
        if (!isConnected)
        {
            Request("Connect");

            StartCoroutine(UpdateScan());
        }
        else
        {
            print("이미 연결된 상태입니다.");
        }
    }


    public void OnDisconnectBtnClkEvent()
    {
        if (isConnected)
        {
            isConnected = false;

            Request("Disconnect");
        }
        else
        {
            print("연결 해지 상태입니다.");
        }
    }


}