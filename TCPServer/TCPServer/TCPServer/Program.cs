using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ActUtlType64Lib;

public class TCPServer
{
    // PLC - TCP Server - Unity
    // PLC로 데이터 송수신
    // 클라이언트(Unity)로 데이터를 송수신

    // TCP Server: TCP Listener 객체를 사용
    // TCP Client: TCP Client 객체를 사용


    static MxCom mxComponent;

    public static void Main()
    {
        //1.Mx Component 객체 생성
        mxComponent = new MxCom(0, 8);

        // 종료시 이벤트
        AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

        //TCP/IP Port 7000번 설정
        TcpListener listener = new TcpListener(IPAddress.Any, 7000);
        listener.Start();
        WriteLog("TCP 서버를 시작합니다.");

        TcpClient client;
        NetworkStream stream;
        byte[] buffer = new byte[1024];
        
       
        while(true) 
        {
            //1.TcpClient의 요청 받아들이기
            client = listener.AcceptTcpClient();

            //2. TcpClient 객체에서 NetworkStream 받아오기
            stream = client.GetStream();

            int nByte;
            string msg = "";

            try
            {

                //1. 데이터 수신  //수신받을 때는
                //stream에 Byte[]형식으로 된것을 UTF8, string 형식으로 바꿔야
                //사람이 읽을 수 있으니
                while ((nByte = stream.Read(buffer,0,buffer.Length))>0)
                {
                    //데이터 인코딩 (Byte[] -> UTF8)
                    msg = Encoding.UTF8.GetString(buffer);
                    string retMsg = "";
                    WriteLog(msg);

                    if(msg.Contains("Connect"))
                    {
                        retMsg = mxComponent.Connect();
                        WriteLog(retMsg);
                    }
                    else if (msg.Contains("Disconnect"))
                    {
                        retMsg = mxComponent.Disconnect();
                        WriteLog(retMsg);
                    }


                    //메시지를 매개로 보내고 쓰고 retMsg
                    //GetBytes 바이트 형식으로 다시 스트림으로 내보는거
                    //그러니까 내가 retMsg를 보내니까
                    //retMsg에는 Server에서 PLC로 보낼 신호가 들어있어야겠지?

                 
                    else if (msg.Contains("GET") && msg.Contains("SET"))
                    {
                        //msg : GET,Y0,4,SET,Y0,0,170
                        //Rad Device -> Write Device

                        string[] dataFromUnity = msg.Split(',');
                        string devicePoint = dataFromUnity[1]; // Y0, X0
                        int blockNum = int.Parse(dataFromUnity[2]); //나는 8
                        string sensorData = dataFromUnity[5] + "," + dataFromUnity[6]; // Sensor Data(0) + Limit Switch Data(170)

                        mxComponent.ReadDeviceBlock(devicePoint, blockNum, out retMsg);
                        WriteLog(retMsg);

                        retMsg = mxComponent.WriteDeviceBlock(devicePoint, sensorData);

                        //sensorData = sensor + , + limitSwitch

                    }
                    else
                    {
                        WriteLog("잘못 입력하셨습니다.");
                        mxComponent.Disconnect();
                        break;
                    }

                    buffer = new byte[1024];
                    buffer = Encoding.UTF8.GetBytes(retMsg);


                    //데이터 송신
                    stream.Write(buffer, 0, buffer.Length);

                    if (msg.Contains("quit"))
                    {
                        Console.WriteLine("서버를 종료합니다.");
                        break;
                    }

                    buffer = new byte[1024];
                }

                if(msg.Contains("quit"))
                {
                    mxComponent.Disconnect();
                    break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

                mxComponent.Disconnect();

                break;
            }
        }
        
        stream.Close(); 
        client.Close();

    }

    public static void WriteLog(string msg)
    {
        Console.WriteLine($"{DateTime.Now}: {msg}");
    }


    public class MxCom 
    {
        
        
        public enum Status
        {
            CONNECTED,
            DISCONNECTED
        };

        ActUtlType64 mxComponent;
      
        Status status = Status.DISCONNECTED;
        float scanTime = 1;
        int blockNum = 8;
        public int[][] pointY;
        public int[][] pointX;
        int[] devices;
        public MxCom(int logicalStationNumber, int blockSizeToRead)
        {

            mxComponent = new ActUtlType64();

            mxComponent.ActLogicalStationNumber = logicalStationNumber;
            WriteLog($"ActLogicalStationNumber가 {mxComponent.ActLogicalStationNumber}로 초기화 되었습니다.");

            blockNum = blockSizeToRead;
            devices = new int[blockNum];


        }

       





        public string Connect()
        {
            if (status == Status.CONNECTED)
            {
               
                return "이미 연결되었습니다.";
            }

            int ret = mxComponent.Open();

            if (ret == 0)
            {
                status = Status.CONNECTED;

                return "PLC에 연결되었습니다.";
            }
            else
            {
                return "PLC연결에 실패했습니다. " + Convert.ToString(ret, 16);
            }
        }

        public string Disconnect()
        {
            if (status == Status.DISCONNECTED)
            {
                return "이미 연결이 해제되었습니다.";
            }

            int ret = mxComponent.Close();

            if (ret == 0)
            {
                return "PLC 연결을 해제하였습니다.";
            }
            else
            {
                return "PLC연결 해제에 실패했습니다. " + Convert.ToString(ret, 16);
            }
        }

      
        public int[] ReadDeviceBlock(string deviceName, int blockNum, out string retMsg)
        {
            retMsg = "";

            devices = new int[blockNum];
            int ret = mxComponent.ReadDeviceBlock(deviceName, blockNum, out devices[0]);

            if (ret == 0)
            {
                foreach (int device in devices)
                {
                    retMsg += device.ToString() + ",";
                }

                return devices;
            }
            else
            {
                retMsg = "ERROR " + Convert.ToString(ret, 16);
                return null;
            }

        }


        //ReadDeviceBlock 했을 때 

        private int[][] ReadDeviceBlock(string deviceName, out string retMsg)
        {
            int[] values = new int[blockNum];
            int[][] information = new int[values.Length][];


            retMsg = "";
            values = ReadDeviceBlock(deviceName, values.Length,out retMsg);

            int i = 0;
            foreach (var value in values)
            {
                string binary = Convert.ToString(value, 2);  //2진수 변환
                retMsg = binary;

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

        public string WriteDeviceBlock(string deviceName, string dataFromClient)
        {
            string[] dataSplited = dataFromClient.Split(",");

            
            int[] data = new int[blockNum];
            data[0] = devices[0];
            data[1] = devices[1];
            data[2] = int.Parse(dataSplited[0]);
            data[5] = int.Parse(dataSplited[1]);

            int ret = mxComponent.WriteDeviceBlock(deviceName, blockNum, ref data[0]);

            if (ret == 0)
            {
                return $"{data[0]},{data[1]},{data[2]},{data[3]}";
            }
            else
            {
                return "ERROR " + Convert.ToString(ret, 16);
            }
        }

    }


    static void CurrentDomain_ProcessExit(object sender, EventArgs e)
    {
        mxComponent.Disconnect();

        Console.WriteLine("exit");
    }


}


