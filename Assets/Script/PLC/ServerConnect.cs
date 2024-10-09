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
        // Singleton 패턴 구현
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 변경되어도 파괴되지 않도록
        }
        else
        {
            Destroy(gameObject); // 다른 인스턴스가 있을 경우 파괴
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
            print("TCPServer가 실행되었습니다.");
        }
        catch (Exception ex)
        {
            print("서버 실행 중 오류 발생: " + ex.Message);
        }
    }

    public void StopTCPServer()
    {
        if (tcpServerProcess != null && !tcpServerProcess.HasExited)
        {
            tcpServerProcess.Kill();
            tcpServerProcess = null;
            UnityEngine.Debug.Log("TCPServer가 종료되었습니다.");
        }
    }

    private string GetSceneFolderPath(string fileName)
    {
        string scenePath = SceneManager.GetActiveScene().path;
        string sceneFolder = Path.GetDirectoryName(scenePath);
        return Path.Combine(sceneFolder, fileName);
    }
}
