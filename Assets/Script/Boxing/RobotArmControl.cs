using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class RobotArmControl : MonoBehaviour
{
    [SerializeField] protected Transform[] motors; // ���� �迭
    [SerializeField] protected RotationAxis[] rotationAxes; // �� ������ ȸ�� �� (Enum)

    // �� ������ ȸ�� ������ ����
    [SerializeField] protected int[] minAngles; // �� ������ �ּ� ����
    [SerializeField] protected int[] maxAngles; // �� ������ �ִ� ����

    [SerializeField] protected TMP_InputField angleInputField;
    [SerializeField] protected TMP_InputField speedInputField;
    [SerializeField] protected TMP_InputField delayInputField;
    [SerializeField] protected TMP_Text currentMotorText;
    [SerializeField] protected TMP_InputField fileNameInputField;

    protected int currentMotorIndex = 0;
    protected List<Step> steps = new List<Step>();
    protected bool isRunning = false;
    protected bool isIncreasing;
    protected bool isDecreasing;

    public enum RotationAxis
    {
        X,
        Y,
        Z
    }

    [System.Serializable]
    public class Step
    {
        public int[] angles;
        public float speed;
        public float delay;

        public Step(int[] angles, float speed, float delay)
        {
            this.angles = angles;
            this.speed = speed;
            this.delay = delay;
        }
    }
    protected const float totalDuration = 1f;

    protected virtual void OnValidate()
    {
        // motors �迭�� ������ ��� minAngles�� maxAngles�� ũ�� ����
        if (motors != null && motors.Length > 0)
        {
            if (minAngles == null || minAngles.Length != motors.Length)
            {
                minAngles = new int[motors.Length];
            }

            if (maxAngles == null || maxAngles.Length != motors.Length)
            {
                maxAngles = new int[motors.Length];
            }
        }
    }

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        speedInputField.text = "5";
        delayInputField.text = "1";
        UpdateAngleInputField();
        UpdateCurrentMotorText();
    }

    public void OnMotorSelectButton()
    {
        currentMotorIndex = (currentMotorIndex + 1) % motors.Length;
        UpdateAngleInputField();
        UpdateCurrentMotorText();
    }

    protected void UpdateCurrentMotorText()
    {
        currentMotorText.text = "Motor: " + currentMotorIndex;
    }

    protected void UpdateAngleInputField()
    {
        angleInputField.text = GetCurrentMotorAngle().ToString();
    }

    protected int GetCurrentMotorAngle()
    {
        switch (rotationAxes[currentMotorIndex])
        {
            case RotationAxis.X:
                return Mathf.RoundToInt(motors[currentMotorIndex].localEulerAngles.x);
            case RotationAxis.Y:
                return Mathf.RoundToInt(motors[currentMotorIndex].localEulerAngles.y);
            case RotationAxis.Z:
                return Mathf.RoundToInt(motors[currentMotorIndex].localEulerAngles.z);
            default:
                return 0;
        }
    }

    public void OnClearSteps()
    {
        steps.Clear();

        for (int i = 0; i < motors.Length; i++)
        {
            motors[i].localRotation = Quaternion.Euler(0, 0, 0);
        }
        currentMotorIndex = 0;
        UpdateAngleInputField();
        Debug.Log("Steps cleared and motors reset to origin.");
    }

    public void OnSaveStep()
    {
        float speed = float.Parse(speedInputField.text);
        float delay = float.Parse(delayInputField.text);
        int[] angles = new int[motors.Length];

        // ������ ���� ����
        for (int i = 0; i < motors.Length; i++)
        {
            currentMotorIndex = i;
            angles[i] = GetCurrentMotorAngle();
        }

        Step step = new Step(angles, speed, delay);
        steps.Add(step);
        Debug.Log("Step saved: " + string.Join(", ", angles));
    }

    public void OnLoadStepsFromCSV()
    {
        string fileName = fileNameInputField.text;
        string filePath = GetSceneFolderPath(fileName + ".csv");

        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found: " + filePath);
            return;
        }

        steps.Clear();
        using (StreamReader reader = new StreamReader(filePath))
        {
            string headerLine = reader.ReadLine(); // ��� �� �б�

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(',');

                // ���� �迭�� ���̸� motors.Length�� ����
                if (values.Length == motors.Length + 2)
                {
                    try
                    {
                        int[] angles = new int[motors.Length];
                        for (int i = 0; i < motors.Length; i++)
                        {
                            angles[i] = int.Parse(values[i + 2]); // ������ 3������ ����
                        }
                        float speed = float.Parse(values[0]); // �ӵ��� 1��
                        float delay = float.Parse(values[1]); // ���� �ð��� 2��

                        steps.Add(new Step(angles, speed, delay));
                    }
                    catch (FormatException e)
                    {
                        Debug.LogError($"Failed to parse line: {line}. Error: {e.Message}");
                    }
                }
                else
                {
                    Debug.LogWarning($"Line skipped due to incorrect format: {line}");
                }
            }
        }

        Debug.Log("Loaded steps from CSV: " + steps.Count + " steps.");
    }
    
    public void OnSaveStepsToCSV()
    {
        string fileName = fileNameInputField.text;
        string filePath = GetSceneFolderPath(fileName + ".csv");

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // CSV ��� �ۼ�
            string[] header = new string[motors.Length + 2];
            header[0] = "speed"; // 1��
            header[1] = "delay"; // 2��
            for (int i = 0; i < motors.Length; i++)
            {
                header[i + 2] = "angle" + (i + 1); // ������ 3������ ����
            }

            writer.WriteLine(string.Join(",", header));

            // �� ���� �����͸� CSV�� �ۼ�
            if (steps.Count > 0)
            {
                // ù ��° ���� ������ 0���� ����
                steps[0].angles = new int[motors.Length];
                for (int i = 0; i < motors.Length; i++)
                {
                    steps[0].angles[i] = 0;
                }
            }

            if (steps.Count > 1)
            {
                // ������ ���� ������ 0���� ����
                steps[steps.Count - 1].angles = new int[motors.Length];
                for (int i = 0; i < motors.Length; i++)
                {
                    steps[steps.Count - 1].angles[i] = 0;
                }
            }

            // ��� ���� �����͸� CSV�� �ۼ�
            foreach (var step in steps)
            {
                string stepData = $"{step.speed},{step.delay},{string.Join(",", step.angles)}"; // ���� ����
                writer.WriteLine(stepData);
            }
        }

        Debug.Log("Saved steps to CSV: " + steps.Count + " steps.");
    }

    private string GetSceneFolderPath(string fileName)
    {
        string scenePath = SceneManager.GetActiveScene().path;
        string sceneFolder = Path.GetDirectoryName(scenePath);
        return Path.Combine(sceneFolder, fileName);
    }

    public void OnStartButton()
    {
        if (!isRunning)
        {
            isRunning = true;
            StartCoroutine(RunSteps());
        }
    }

    public void OnStopButton()
    {
        isRunning = false;
        StopAllCoroutines();
    }

    private IEnumerator RunSteps()
    {
        while (isRunning)
        {
            if (steps.Count == 0)
            {
                Debug.Log("No steps to run.");
                yield break;
            }

            for (int i = 0; i < steps.Count; i++)
            {
                Step prevStep = (i == 0)
                    ? new Step(new int[motors.Length], 0, 0)
                    : steps[i - 1];

                yield return RunStep(prevStep, steps[i]);
            }
        }
    }

    private IEnumerator RunStep(Step prevStep, Step nowStep)
    {
        float elapsedTime = 0f;

        Quaternion[] targetRotations = new Quaternion[motors.Length];
        for (int i = 0; i < motors.Length; i++)
        {
            Vector3 targetAngle = Vector3.zero;
            switch (rotationAxes[i])
            {
                case RotationAxis.X:
                    targetAngle = new Vector3(nowStep.angles[i], 0, 0);
                    break;
                case RotationAxis.Y:
                    targetAngle = new Vector3(0, nowStep.angles[i], 0);
                    break;
                case RotationAxis.Z:
                    targetAngle = new Vector3(0, 0, nowStep.angles[i]);
                    break;
            }
            targetRotations[i] = Quaternion.Euler(targetAngle);
        }

        Quaternion[] prevRotations = new Quaternion[motors.Length];
        for (int i = 0; i < motors.Length; i++)
        {
            Vector3 prevAngle = Vector3.zero;
            switch (rotationAxes[i])
            {
                case RotationAxis.X:
                    prevAngle = new Vector3(prevStep.angles[i], 0, 0);
                    break;
                case RotationAxis.Y:
                    prevAngle = new Vector3(0, prevStep.angles[i], 0);
                    break;
                case RotationAxis.Z:
                    prevAngle = new Vector3(0, 0, prevStep.angles[i]);
                    break;
            }
            prevRotations[i] = Quaternion.Euler(prevAngle);
        }

        while (elapsedTime < totalDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / totalDuration;

            for (int i = 0; i < motors.Length; i++)
            {
                motors[i].localRotation = Quaternion.Slerp(prevRotations[i], targetRotations[i], t);
            }

            yield return null; // ���� �����ӱ��� ���
        }

        // �� ���� ������ ���� �߰�
        yield return new WaitForSeconds(nowStep.delay);
    }

    public Quaternion RotateAngle(Vector3 from, Vector3 to, float t)
    {
        return Quaternion.Slerp(Quaternion.Euler(from), Quaternion.Euler(to), t);
    }

    public void OnIncreaseAngleButtonDown()
    {
        isIncreasing = true;
        StartCoroutine(ChangeMotorAngle(1));
    }

    public void OnDecreaseAngleButtonDown()
    {
        isDecreasing = true;
        StartCoroutine(ChangeMotorAngle(-1));
    }

    private void AdjustMotorAngle(float direction)
    {
        float newAngle = GetCurrentMotorAngle() + direction;

        // ���� ����: ������ �ּ� �� �ִ� ���� ������ ����
        newAngle = Mathf.Clamp(newAngle, minAngles[currentMotorIndex], maxAngles[currentMotorIndex]);

        // ȸ�� �࿡ ���� ȸ�� ó��
        switch (rotationAxes[currentMotorIndex])
        {
            case RotationAxis.X:
                motors[currentMotorIndex].localRotation = Quaternion.Euler(newAngle, 0, 0);
                break;
            case RotationAxis.Y:
                motors[currentMotorIndex].localRotation = Quaternion.Euler(0, newAngle, 0);
                break;
            case RotationAxis.Z:
                motors[currentMotorIndex].localRotation = Quaternion.Euler(0, 0, newAngle);
                break;
        }

        UpdateAngleInputField();
    }

    public void OnButtonUp()
    {
        isIncreasing = false;
        isDecreasing = false;
    }

    private IEnumerator ChangeMotorAngle(int direction)
    {
        while (isIncreasing || isDecreasing)
        {
            if (isIncreasing)
            {
                AdjustMotorAngle(1);
            }
            if (isDecreasing)
            {
                AdjustMotorAngle(-1);
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
