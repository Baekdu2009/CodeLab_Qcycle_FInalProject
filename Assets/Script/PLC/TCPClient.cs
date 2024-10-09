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

    [SerializeField] Conveyor conveyor;
    [SerializeField] Conveyor shredder;
    [SerializeField] LevelSensor[] sensor;
    [SerializeField] FilamentLine[] linemanagers;
    [SerializeField] WireCutting wireCutting;
    [SerializeField] ScrewBelt screwBelt;

    public bool cooling1;
    private void Start()
    {
        // 로컬호스트: 로컬 컴퓨터의 디폴트 IP
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

    private int[][] ReadDeviceBlock(string dataFromServer)
    {
        print("1. ReadDeviceBlock dataFromServer: " + dataFromServer);

        // 문자열을 분리
        string[] strSplited = dataFromServer.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        print("2. ReadDeviceBlock newData: " + string.Join(",", strSplited));

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
                Debug.LogError($"변환 실패: '{strSplited[i]}'는 정수로 변환할 수 없습니다.");
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
        int sensorAValue = (sensor[0].isDetected == true) ? 1 : 0;
        // int sensorBValue = (sensor[1].isDetected == true) ? 1 : 0;

        int sensorNum = /*sensorCValue * 4 + sensorBValue * 2 + */sensorAValue * 1;
        int lsNum = sensorAValue * 1;

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
            

            //컨베이어
            int runConveyor = pointY[0][1];

            //탱크 제어
           /* int Tank1 = pointY[0][5];
            int Tank2 = pointY[0][6];*/


            //모터릴레이
            int runShreder = pointY[2][0];
            int runExtruder1 = pointY[2][1];
            int runCooler1 = pointY[2][2];
            int runCuttingMachine = pointY[2][3];
            int runScrewBelt = pointY[2][4];
            /*int runExtruder2 = pointY[2][5];
            int runCooler2 = pointY[2][6];
            int runPullyMachine = pointY[2][7];*/

            //센서

            int Level1 = pointX[5][0];
           // int Level2 = pointX[5][1];
           //int LimitSwitch = pointX[5][2];

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
                conveyor.shredderRunning = true;
            }
            else if (runShreder != 1)
            {
                conveyor.shredderRunning = false;
            }
            if (runExtruder1 == 1)
            {
                linemanagers[0].isWorking = true;
            }
            else if (runExtruder1 != 1)
            {
                linemanagers[0].isWorking = false;
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
            }
            else if (runCuttingMachine != 1)
            {
                wireCutting.isWorking = false;
            }
            if (runScrewBelt == 1)
            {
                screwBelt.isWorking = true;
            }
            else if (runScrewBelt != 1)
            {
                screwBelt.isWorking = false;
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

   


