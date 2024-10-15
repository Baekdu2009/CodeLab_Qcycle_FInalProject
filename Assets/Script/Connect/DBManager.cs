using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using Firebase;

public class DBManager : MonoBehaviour
{
    public static DBManager instance;
    FirebaseDatabase database;
    public DatabaseReference dbRef;
    [SerializeField] string dbURL = "";

    public class RobotarmData
    {
        public string name;
        public Work working;
        public enum Work
        {
            AGV,
            Boxing
        }

    }
    [SerializeField] List<BoxingRobot> BoxingRobots = new List<BoxingRobot>();
    [SerializeField] List<RobotArmOnAGV> robotArmOnAGVs = new List<RobotArmOnAGV>();

    public void Awake()
    {
        if (instance != null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new Uri(dbURL);
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    void BoxingRobotDataUpload()
    {
        if (BoxingRobots.Count != 0)
        {
            dbRef = FirebaseDatabase.DefaultInstance.GetReference("BoxingRobot");

            List<System.Threading.Tasks.Task> uploadTasks = new List<System.Threading.Tasks.Task>();

            foreach (BoxingRobot robot in BoxingRobots)
            {
                var robotData = new Dictionary<string, object>
                {
                    { "RobotWork", robot.robotWork },
                };

                string key = $"RobotWork";

                var uploadTask = dbRef.Child(key).SetValueAsync(robotData);
                uploadTasks.Add(uploadTask);
            }

            System.Threading.Tasks.Task.WhenAll(uploadTasks).ContinueWith(t =>
            {
                if (t.IsCompleted)
                {
                    print("박스로봇 데이터 전송 완료");
                }
                else
                {
                    print($"박스로봇 데이터 전송 오류: {t.Exception}");
                }
            });
        }
    }
    void AGVRobotDataUpload()
    {
        if (BoxingRobots.Count != 0)
        {
            dbRef = FirebaseDatabase.DefaultInstance.GetReference("AGVRobot");

            List<System.Threading.Tasks.Task> uploadTasks = new List<System.Threading.Tasks.Task>();

            foreach (RobotArmOnAGV robot in robotArmOnAGVs)
            {
                var robotData = new Dictionary<string, object>
                {
                    { "RobotWork", robot.robotWork },
                };

                string key = $"RobotWork";

                var uploadTask = dbRef.Child(key).SetValueAsync(robotData);
                uploadTasks.Add(uploadTask);
            }

            System.Threading.Tasks.Task.WhenAll(uploadTasks).ContinueWith(t =>
            {
                if (t.IsCompleted)
                {
                    print("AGV로봇팔 데이터 전송 완료");
                }
                else
                {
                    print($"AGV로봇팔 데이터 전송 오류: {t.Exception}");
                }
            });
        }
    }
}
