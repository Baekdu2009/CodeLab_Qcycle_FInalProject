using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine.SceneManagement;

public class ServerConnect : MonoBehaviour
{
    private static ServerConnect instance;
    private Process tcpServerProcess;

    private void Awake()
    {
        // Singleton ���� ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ���� ����Ǿ �ı����� �ʵ���
        }
        else
        {
            Destroy(gameObject); // �ٸ� �ν��Ͻ��� ���� ��� �ı�
        }
    }

    public void RunTCPServer()
    {
        string path = GetSceneFolderPath("TCPServer.lnk");

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = path,
            UseShellExecute = true
        };

        try
        {
            Process.Start(startInfo);
            print("TCPServer�� ����Ǿ����ϴ�.");
        }
        catch (Exception ex)
        {
            print("���� ���� �� ���� �߻�: " + ex.Message);
        }
    }

    public void StopTCPServer()
    {
        if (tcpServerProcess != null && !tcpServerProcess.HasExited)
        {
            tcpServerProcess.Kill();
            tcpServerProcess = null;
            UnityEngine.Debug.Log("TCPServer�� ����Ǿ����ϴ�.");
        }
    }

    private string GetSceneFolderPath(string fileName)
    {
        string scenePath = SceneManager.GetActiveScene().path;
        string sceneFolder = Path.GetDirectoryName(scenePath);
        return Path.Combine(sceneFolder, fileName);
    }
}
