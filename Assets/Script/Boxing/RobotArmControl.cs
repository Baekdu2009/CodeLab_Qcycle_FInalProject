using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class RobotArmControl : MonoBehaviour
{
    // ���� ������ Transform
    [SerializeField] Transform motorA;
    [SerializeField] Transform motorB;
    [SerializeField] Transform motorC;
    [SerializeField] Transform motorD;
    [SerializeField] Transform motorE;
    [SerializeField] Transform motorF;

    // UI ���
    [SerializeField] TMP_InputField angleInputField;
    [SerializeField] GameObject leftButton;
    [SerializeField] GameObject rightButton;
    [SerializeField] TMP_InputField speedInputField;
    [SerializeField] TMP_InputField delayInputField;
    [SerializeField] TMP_Text currentMotorText; // ���� ���õ� ���� �̸��� ǥ���� �ؽ�Ʈ
    [SerializeField] TMP_InputField fileNameInputField; // CSV ���� �̸� �Է� �ʵ�

    private int currentMotorIndex = 0; // ���� ���õ� ���� �ε���
    private List<Step> steps = new List<Step>();
    private bool isRunning = false; // ���� ������ ����
    private bool isIncreasing;
    private bool isDecreasing;

    // ���� �迭 �߰�
    private Transform[] motors;

    [System.Serializable]
    public class Step
    {
        public float angleA;
        public float angleB;
        public float angleC;
        public float angleD;
        public float angleE;
        public float angleF;
        public float speed;
        public float delay;

        public Step(float a, float b, float c, float d, float e, float f, float speed, float delay)
        {
            angleA = a; angleB = b; angleC = c; angleD = d; angleE = e; angleF = f;
            this.speed = speed; this.delay = delay;
        }
    }

    private void Start()
    {
        // ���� �迭 �ʱ�ȭ
        motors = new Transform[] { motorA, motorB, motorC, motorD, motorE, motorF };

        speedInputField.text = "5";
        delayInputField.text = "1";

        UpdateAngleInputField();
        UpdateCurrentMotorText(); // �ʱ� ���� �ؽ�Ʈ ������Ʈ
    }

    public void OnMotorSelectButton()
    {
        // ���� ���� �ε����� ������Ű��, �迭�� ���̿� ���缭 ��ȯ
        currentMotorIndex = (currentMotorIndex + 1) % motors.Length;
        UpdateAngleInputField();
        UpdateCurrentMotorText(); // ���õ� ���� �ؽ�Ʈ ������Ʈ
    }

    private void UpdateCurrentMotorText()
    {
        // ���� ���õ� ���� �̸��� ǥ��
        string[] motorNames = { "A", "B", "C", "D", "E", "F" };
        currentMotorText.text = "Motor: " + motorNames[currentMotorIndex];
    }

    private void UpdateAngleInputField()
    {
        angleInputField.text = GetCurrentMotorAngle().ToString();
    }

    private float GetCurrentMotorAngle()
    {
        // A, D, F�� Z��, B, C, E�� X�� �������� ������ ��ȯ
        if (currentMotorIndex == 0 || currentMotorIndex == 3 || currentMotorIndex == 5) // A, D, F
        {
            return motors[currentMotorIndex].localEulerAngles.z;
        }
        else // B, C, E
        {
            return motors[currentMotorIndex].localEulerAngles.x;
        }
    }

    private void OnSaveStep()
    {
        float speed = float.Parse(speedInputField.text);
        float delay = float.Parse(delayInputField.text);
        Step step = new Step(
            motorA.localEulerAngles.z,
            motorB.localEulerAngles.x,
            motorC.localEulerAngles.x,
            motorD.localEulerAngles.z,
            motorE.localEulerAngles.x,
            motorF.localEulerAngles.z,
            speed,
            delay
        );

        steps.Add(step);
        Debug.Log("Step saved: " + step.angleA + ", " + step.angleB + ", " + step.angleC + ", " + step.angleD + ", " + step.angleE + ", " + step.angleF);
    }

    public void OnClearSteps()
    {
        steps.Clear(); // ���� ����Ʈ �ʱ�ȭ
        Debug.Log("Steps cleared."); // �ʱ�ȭ �Ϸ� �α�
    }

    public void OnLoadStepsFromCSV()
    {
        string fileName = fileNameInputField.text;
        string filePath = "C:\\Unity\\FInalProject\\" + fileName + ".csv";

        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found: " + filePath);
            return;
        }

        steps.Clear();
        using (StreamReader reader = new StreamReader(filePath))
        {
            // ù ��° ���� �о� ����� �ǳʶݴϴ�.
            string headerLine = reader.ReadLine();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(',');

                if (values.Length == 8)
                {
                    try
                    {
                        float angleA = float.Parse(values[0]);
                        float angleB = float.Parse(values[1]);
                        float angleC = float.Parse(values[2]);
                        float angleD = float.Parse(values[3]);
                        float angleE = float.Parse(values[4]);
                        float angleF = float.Parse(values[5]);
                        float speed = float.Parse(values[6]);
                        float delay = float.Parse(values[7]);

                        steps.Add(new Step(angleA, angleB, angleC, angleD, angleE, angleF, speed, delay));
                    }
                    catch (FormatException e)
                    {
                        Debug.LogError($"Failed to parse line: {line}. Error: {e.Message}");
                    }
                    catch (OverflowException e)
                    {
                        Debug.LogError($"Number too large in line: {line}. Error: {e.Message}");
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
        string filePath = "C:\\Unity\\FinalProject\\" + fileName + ".csv";

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // CSV ��� �ۼ�
            writer.WriteLine("angleA,angleB,angleC,angleD,angleE,angleF,speed,delay");

            // ù ��° ������ ���� ���, ������ 0���� ����
            if (steps.Count > 0)
            {
                // ù ��° ���� ���� ����
                var firstStep = steps[0];
                firstStep.angleA = 0;
                firstStep.angleB = 0;
                firstStep.angleC = 0;
                firstStep.angleD = 0;
                firstStep.angleE = 0;
                firstStep.angleF = 0;

                // ������ ù ��° ���� ����
                writer.WriteLine($"{firstStep.angleA},{firstStep.angleB},{firstStep.angleC},{firstStep.angleD},{firstStep.angleE},{firstStep.angleF},{firstStep.speed},{firstStep.delay}");
            }

            // �߰� ���� ����
            for (int i = 1; i < steps.Count - 1; i++)
            {
                var step = steps[i];
                writer.WriteLine($"{step.angleA},{step.angleB},{step.angleC},{step.angleD},{step.angleE},{step.angleF},{step.speed},{step.delay}");
            }

            // ������ ������ ���� ���, ������ 0���� ����
            if (steps.Count > 1)
            {
                var lastStep = steps[steps.Count - 1];
                lastStep.angleA = 0;
                lastStep.angleB = 0;
                lastStep.angleC = 0;
                lastStep.angleD = 0;
                lastStep.angleE = 0;
                lastStep.angleF = 0;

                // ������ ������ ���� ����
                writer.WriteLine($"{lastStep.angleA},{lastStep.angleB},{lastStep.angleC},{lastStep.angleD},{lastStep.angleE},{lastStep.angleF},{lastStep.speed},{lastStep.delay}");
            }
        }

        Debug.Log("Saved steps to CSV: " + steps.Count + " steps.");
    }


    public void OnStartButton()
    {
        if (!isRunning)
        {
            isRunning = true; // ���� ������ ǥ��
            StartCoroutine(RunSteps());
        }
    }

    public void OnStopButton()
    {
        isRunning = false; // ���� ����
        StopAllCoroutines(); // ��� �ڷ�ƾ ����
    }

    private IEnumerator RunSteps()
    {
        while (isRunning)
        {
            if (steps.Count == 0)
            {
                Debug.Log("No steps to run."); // ������ ���� ��� �α�
                yield break; // �ڷ�ƾ ����
            }

            for (int i = 0; i < steps.Count; i++)
            {
                Step prevStep = (i == 0)
                    ? new Step(0, 0, 0, 0, 0, 0, 0, 0) // ù ��° ������ ��� �ʱⰪ ���
                    : steps[i - 1];

                yield return RunStep(prevStep, steps[i]);
            }
        }
    }

    private IEnumerator RunStep(Step prevStep, Step nowStep)
    {
        float elapsedTime = 0f;
        float totalDuration = 1f; // ������ �ð� (��: 1��)
        Vector3 targetA = new Vector3(0, 0, nowStep.angleA);
        Vector3 targetB = new Vector3(nowStep.angleB, 0, 0);
        Vector3 targetC = new Vector3(nowStep.angleC, 0, 0);
        Vector3 targetD = new Vector3(0, 0, nowStep.angleD);
        Vector3 targetE = new Vector3(nowStep.angleE, 0, 0);
        Vector3 targetF = new Vector3(0, 0, nowStep.angleF);

        // ���� ������ ����
        Vector3 prevAngleA = new Vector3(0, 0, prevStep.angleA);
        Vector3 prevAngleB = new Vector3(prevStep.angleB, 0, 0);
        Vector3 prevAngleC = new Vector3(prevStep.angleC, 0, 0);
        Vector3 prevAngleD = new Vector3(0, 0, prevStep.angleD);
        Vector3 prevAngleE = new Vector3(prevStep.angleE, 0, 0);
        Vector3 prevAngleF = new Vector3(0, 0, prevStep.angleF);

        while (elapsedTime < totalDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / totalDuration;

            motorA.localRotation = RotateAngle(prevAngleA, targetA, t);
            motorB.localRotation = RotateAngle(prevAngleB, targetB, t);
            motorC.localRotation = RotateAngle(prevAngleC, targetC, t);
            motorD.localRotation = RotateAngle(prevAngleD, targetD, t);
            motorE.localRotation = RotateAngle(prevAngleE, targetE, t);
            motorF.localRotation = RotateAngle(prevAngleF, targetF, t);

            yield return null; // ���� �����ӱ��� ���
        }

        // �� ���� ������ ���� �߰�
        yield return new WaitForSeconds(nowStep.delay);
    }


    // ȸ�� �޼��� �߰�
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

        newAngle = Mathf.Clamp(newAngle, -359, 359); // ���� ����

        // A, D, F�� Z��, B, C, E�� X�� �������� ȸ��
        if (currentMotorIndex == 0 || currentMotorIndex == 3 || currentMotorIndex == 5) // A, D, F
        {
            motors[currentMotorIndex].localRotation = Quaternion.Euler(0, 0, newAngle);
        }
        else // B, C, E
        {
            motors[currentMotorIndex].localRotation = Quaternion.Euler(newAngle, 0, 0);
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

