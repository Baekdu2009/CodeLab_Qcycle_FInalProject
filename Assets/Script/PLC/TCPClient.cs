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

    [Header("����� ������ ���ۿ� ���� �κ��Դϴ�.")]
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

    [Header("������� �����մϴ�.")]

    [SerializeField] Conveyor conveyor;
    [SerializeField] Conveyor shredder;
    [SerializeField] LevelSensor[] sensor;
    [SerializeField] FilamentLine[] linemanagers;
    [SerializeField] WireCutting wireCutting;
    [SerializeField] ScrewBelt screwBelt;

    public bool cooling1;
    private void Start()
    {
        // ����ȣ��Ʈ: ���� ��ǻ���� ����Ʈ IP
        try
        {
            client = new TcpClient("127.0.0.1", 7000);

            stream = client.GetStream();
        }
        catch (Exception e)
        {
            print(e.ToString());
            print("������ ���� ���ּ���.");
        }
    }

    private int[][] ReadDeviceBlock(string dataFromServer)
    {
        print("1. ReadDeviceBlock dataFromServer: " + dataFromServer);

        // ���ڿ��� �и�
        string[] strSplited = dataFromServer.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        print("2. ReadDeviceBlock newData: " + string.Join(",", strSplited));

        // �迭�� ũ�⸦ strSplited�� ���̿� ���߱�
        int[] values = new int[strSplited.Length];

        for (int i = 0; i < strSplited.Length; i++)
        {
            if (int.TryParse(strSplited[i], out values[i]))
            {
                // ��ȯ ����
            }
            else
            {
                Debug.LogError($"��ȯ ����: '{strSplited[i]}'�� ������ ��ȯ�� �� �����ϴ�.");
                values[i] = 0; // �⺻�� ����
            }
        }

        // 2���� �迭�� ��ȯ
        int[][] information = new int[values.Length][];

        for (int i = 0; i < values.Length; i++)
        {
            string binary = Convert.ToString(values[i], 2).PadLeft(16, '0'); // 16�ڸ��� �е�
            information[i] = ConvertStringToIntArray(binary);
        }

        return information;
    }

    //2������ �ٲ� ���� 16�� ��Ʈ���� ���� 16��Ʈ ����ó�� �����
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
            

            //�����̾�
            int runConveyor = pointY[0][1];

            //��ũ ����
           /* int Tank1 = pointY[0][5];
            int Tank2 = pointY[0][6];*/


            //���͸�����
            int runShreder = pointY[2][0];
            int runExtruder1 = pointY[2][1];
            int runCooler1 = pointY[2][2];
            int runCuttingMachine = pointY[2][3];
            int runScrewBelt = pointY[2][4];
            /*int runExtruder2 = pointY[2][5];
            int runCooler2 = pointY[2][6];
            int runPullyMachine = pointY[2][7];*/

            //����

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

            //Steam�� ������ ����
            stream.Write(buffer, 0, buffer.Length);

            //������ ����
            byte[] buffer2 = new byte[1024];
            int nBytes = stream.Read(buffer2, 0, buffer2.Length);
            string msg = Encoding.UTF8.GetString(buffer2, 0, nBytes);
            print(msg);

            if (msg.Contains("PLC�� ����Ǿ����ϴ�."))
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
                // Y ������ ��û
                await stream.WriteAsync(Encoding.UTF8.GetBytes(dataToServerY), 0, dataToServerY.Length);
                // ������ ����
                byte[] bufferY = new byte[1024];
                int nBytesY = await stream.ReadAsync(bufferY, 0, bufferY.Length);
                string msgY = Encoding.UTF8.GetString(bufferY, 0, nBytesY);

                if (!string.IsNullOrEmpty(msgY))
                {
                    dataFromServerY = msgY; // Y ������
                    pointY = ReadDeviceBlock(dataFromServerY);
                }

                // X ������ ��û
                await stream.WriteAsync(Encoding.UTF8.GetBytes(dataToServerX), 0, dataToServerX.Length);
                // ������ ����
                byte[] bufferX = new byte[1024];
                int nBytesX = await stream.ReadAsync(bufferX, 0, bufferX.Length);
                string msgX = Encoding.UTF8.GetString(bufferX, 0, nBytesX);

                if (!string.IsNullOrEmpty(msgX))
                {
                    dataFromServerX = msgX; // X ������
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
            print("�������� �����Դϴ�. Connect ��ư�� Ŭ���� �ּ���.");
    }



    private void OnDestroy()
    {
        isConnected = false;
    }

    IEnumerator UpdateScan()
    {
        yield return new WaitUntil(() => isConnected);

        print("UpdateScan");

        // 1. Server�� �����͸� ��û�ϰ� �޴¿���
        OnAsyncBtnClkEvent();

        while (isConnected)
        {
            // 2. ��û�ؼ� ���� �����͸� Unity�� ���� �����ϴ� ����
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
            print("�̹� ����� �����Դϴ�.");
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
            print("���� ���� �����Դϴ�.");
        }
    }
}

   


