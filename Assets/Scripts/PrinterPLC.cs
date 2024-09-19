using System;
using UnityEngine;
using ActUtlType64Lib;
using System.Linq; // ActUtlType ���̺귯�� �߰�

public class PrinterPLC : MonoBehaviour
{
    public enum Status
    {
        CONNECTED,
        DISCONNECTED
    }

    ActUtlType64 mxComponent;
    [SerializeField] Status status = Status.DISCONNECTED;
    float scanTime = 1;
    [SerializeField] Transform nozzle;
    [SerializeField] Transform rod;
    [SerializeField] Transform plate;

    [SerializeField] public int blockNum = 4;
    public int[][] pointY;

    private void Start()
    {
        mxComponent = new ActUtlType64();

        mxComponent.ActLogicalStationNumber = 0;

        InvokeRepeating("ScanPLC", 1, scanTime);
    }

    void ScanPLC()
    {
        if (status == Status.DISCONNECTED) return;

        pointY = ReadDeviceBlock("Y0");

        
    }

    private int[][] ReadDeviceBlock(string deviceName)
    {
        int[] values = new int[blockNum];
        int[][] informantion = new int[values.Length][];

        values = ReadDeviceBlock(deviceName,values.Length);

        int i = 0;
        foreach (int value in values)
        {
            string binary = Convert.ToString(value, 2);
            informantion[i] = ConvertStringToIntArray(binary);

            i++;
        }

        return informantion;
    }

    int[] devices;
    public int[] ReadDeviceBlock(string deviceName, int blockSize)
    {
        devices = new int[blockSize];
        int ret = mxComponent.ReadDeviceBlock(deviceName, blockSize, out devices[0]);

        if (ret == 0)
        {
            return devices;
        }
        else
        {
            print("ERROR" + ret);

            return null;
        }
    }

    public void WriteDeviceBlock(string devicename, int[][] nowPoint)
    {
        
    }

    private static int[] ConvertStringToIntArray(string binary)
    {
        int strlength = binary.Length;
        int zeroNum = 16 - strlength;

        string reversedBinary = new string(binary.Reverse().ToArray());

        for (int i = 0; i < zeroNum; i++)
        {
            reversedBinary += "0";
        }

        char[] strings = reversedBinary.ToCharArray();

        int[] devicePoints = Array.ConvertAll(strings, c => (int)Char.GetNumericValue(c));

        return devicePoints;
    }

    public void OnConnectBtnClkEvent()
    {

        if (status == Status.CONNECTED)
        {
            print("�̹� ����Ǿ� �ֽ��ϴ�.");
            return;
        }

        int ret = mxComponent.Open();

        if (ret == 0)
        {
            print("PLC���ῡ �����߽��ϴ�.");
            status = Status.CONNECTED;
        }
        else
        {
            print("PLC���ῡ �����߽��ϴ�." + Convert.ToString(ret, 16));
        }
    }

    public void OnDisconnectBtnClkEvent()
    {

        if (status == Status.DISCONNECTED)
        {
            print("�̹� ����Ǿ� ���� �ʽ��ϴ�.");
            return;
        }

        int ret = mxComponent.Close();

        if (ret == 0)
        {
            print("PLC������ �����߽��ϴ�.");
            status = Status.DISCONNECTED;
        }
        else
        {
            print("PLC������ �����߽��ϴ�." + Convert.ToString(ret, 16));
        }
    }

    private void OnDestroy()
    {
        OnDisconnectBtnClkEvent();
    }
}
