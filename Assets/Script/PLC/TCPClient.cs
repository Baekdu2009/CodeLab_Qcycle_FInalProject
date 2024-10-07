using UnityEngine;
using System.Net.Sockets;
using System.Collections;
using System.Text;
using System;
using TMPro;
using System.Threading.Tasks;
using UnityEditor.Rendering;
public class TCPClient : MonoBehaviour
{

    public TMP_InputField input;
    TcpClient client;
    NetworkStream stream;
    bool isConnected = false;


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

    public void OnConnectBtnClkEvent()
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
   
    public void OnAsyncBtnClkEvent()
    {
        if (isConnected)
            Task.Run(() => RequestAsync());
        else
            print("�������� �����Դϴ�. Connect ��ư�� Ŭ���� �ּ���.");
    }

    private void Request()
    {
        try
        {
            byte[] buffer = Encoding.UTF8.GetBytes(input.text);

            // NetworkStream�� ������ ����
            stream.Write(buffer, 0, buffer.Length);

            // ������ ����(i.g GET,Y0,5)
            byte[] buffer2 = new byte[1024];
            int nBytes = stream.Read(buffer2, 0, buffer2.Length);
            string msg = Encoding.UTF8.GetString(buffer2, 0, nBytes);
            print(msg);
        }
        catch (Exception e)
        {
            print(e.ToString());
        }
    }

    private void Request(string order)
    {
        try
        {
            byte[] buffer = Encoding.UTF8.GetBytes(order);

            // NetworkStream�� ������ ����
            stream.Write(buffer, 0, buffer.Length);

            // ������ ����(i.g GET,Y0,5)
            byte[] buffer2 = new byte[1024];
            int nBytes = stream.Read(buffer2, 0, buffer2.Length);
            string msg = Encoding.UTF8.GetString(buffer2, 0, nBytes);
            print(msg);
        }
        catch (Exception e)
        {
            print(e.ToString());
        }
    }


    private async Task RequestAsync()
    {
        if(input.text == "")
        {
            print("�Է�â�� ��ɾ �Է����ּ���.\ni.g) GET,Y0,4 or SET,Y0,7,128");
            return;
        }

        while (true) 
        {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(input.text);

                await stream.WriteAsync(buffer, 0, buffer.Length);

                //������ ���� (i.g GET,Y0,5)

                byte[] buffer2 = new byte[1024];
                int nBytes = await stream.ReadAsync(buffer2, 0, buffer2.Length);
                string msg = Encoding.UTF8.GetString(buffer2, 0, nBytes);
                print(msg);

                if (!isConnected) break;
            }
            catch (Exception e)
            {
                print(e.ToString());
            }

        }

    }

}










