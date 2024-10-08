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
    [SerializeField] string dataToServer;
    [SerializeField] string dataFromServer;
    [SerializeField] int[][] pointX;
    [SerializeField] int[][] pointY;
    [SerializeField] float scanTime = 0.1f;
    [SerializeField] string startPoint = "Y0";
    [SerializeField] int blockNum = 8;
    TcpClient client;
    NetworkStream stream;

    [Header("설비들을 연결합니다.")]

    [SerializeField] Conveyor conveyorA;
    [SerializeField] LevelSensor sensorA;


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
        // " 0,0,0,170 " 뭐 이런식
        string[] strSplited = dataFromServer.Split(',');
        print("2. ReadDeviceBlock newData: " + strSplited);

        int[] values = Array.ConvertAll(strSplited, int.Parse);
        int[][] information = new int[values.Length][];


        int i = 0;
        foreach(var value in values)
        {
            string binary = Convert.ToString(value, 2); // 2진수변환 10000

            information[i] = ConvertStringToIntArray(binary);

            i++;
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
        int sensorAValue = (sensorA.isDetected == true) ? 1 : 0;

        int sensorNum = /*sensorCValue * 4 + sensorBValue * 2*/  sensorAValue * 1;
        int lsNum = 0;
        
        string sensorData = sensorNum.ToString() +"," + lsNum.ToString();

        return sensorData;
    }

    
    private void ScanPLC()
    {

        try
        {

            string sensorData = WriteDeivceBlock();
            dataToServer = $"GET,{startPoint},{blockNum},SET,{startPoint},{sensorData}";

            pointY = ReadDeviceBlock(dataFromServer);
            pointX = ReadDeviceBlock(dataFromServer);

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
                conveyorA.OnConveyorBtnClkEvent();
                // filamentFactory.StatusCheck(conveyorStatus, runConveyor, 0);
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
                // dataToServer의 형태 GET,Y0,4,SET,Y0,0,170;
                byte[] buffer = Encoding.UTF8.GetBytes(dataToServer);

                // NetworkStream에 데이터 쓰기
                await stream.WriteAsync(buffer, 0, buffer.Length);

                // 데이터 수신(i.g GET,Y0,5)
                byte[] buffer2 = new byte[1024];
                int nBytes = await stream.ReadAsync(buffer2, 0, buffer2.Length);

                // dataFromServer = "0,0,0,170"
                dataFromServer = Encoding.UTF8.GetString(buffer2, 0, nBytes);

                print("RequestAsync: " + dataFromServer);

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

   


