using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class RobotArmControl : MonoBehaviour
{
    // 모터 각각의 Transform
    [SerializeField] Transform motorA;
    [SerializeField] Transform motorB;
    [SerializeField] Transform motorC;
    [SerializeField] Transform motorD;
    [SerializeField] Transform motorE;
    [SerializeField] Transform motorF;

    // UI 요소
    [SerializeField] TMP_InputField angleInputField;
    [SerializeField] GameObject leftButton;
    [SerializeField] GameObject rightButton;
    [SerializeField] TMP_InputField speedInputField;
    [SerializeField] TMP_InputField delayInputField;
    [SerializeField] TMP_Text currentMotorText; // 현재 선택된 모터 이름을 표시할 텍스트
    [SerializeField] TMP_InputField fileNameInputField; // CSV 파일 이름 입력 필드

    private int currentMotorIndex = 0; // 현재 선택된 모터 인덱스
    private List<Step> steps = new List<Step>();
    private bool isRunning = false; // 동작 중인지 여부
    private bool isIncreasing;
    private bool isDecreasing;

    // 모터 배열 추가
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
        // 모터 배열 초기화
        motors = new Transform[] { motorA, motorB, motorC, motorD, motorE, motorF };

        speedInputField.text = "5";
        delayInputField.text = "1";

        UpdateAngleInputField();
        UpdateCurrentMotorText(); // 초기 모터 텍스트 업데이트
    }

    public void OnMotorSelectButton()
    {
        // 현재 모터 인덱스를 증가시키고, 배열의 길이에 맞춰서 순환
        currentMotorIndex = (currentMotorIndex + 1) % motors.Length;
        UpdateAngleInputField();
        UpdateCurrentMotorText(); // 선택된 모터 텍스트 업데이트
    }

    private void UpdateCurrentMotorText()
    {
        // 현재 선택된 모터 이름을 표시
        string[] motorNames = { "A", "B", "C", "D", "E", "F" };
        currentMotorText.text = "Motor: " + motorNames[currentMotorIndex];
    }

    private void UpdateAngleInputField()
    {
        angleInputField.text = GetCurrentMotorAngle().ToString();
    }

    private float GetCurrentMotorAngle()
    {
        // A, D, F는 Z축, B, C, E는 X축 기준으로 각도를 반환
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
        steps.Clear(); // 스텝 리스트 초기화
        Debug.Log("Steps cleared."); // 초기화 완료 로그
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
            // 첫 번째 줄을 읽어 헤더를 건너뜁니다.
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
            // CSV 헤더 작성
            writer.WriteLine("angleA,angleB,angleC,angleD,angleE,angleF,speed,delay");

            // 첫 번째 스텝이 있을 경우, 각도를 0으로 설정
            if (steps.Count > 0)
            {
                // 첫 번째 스텝 각도 설정
                var firstStep = steps[0];
                firstStep.angleA = 0;
                firstStep.angleB = 0;
                firstStep.angleC = 0;
                firstStep.angleD = 0;
                firstStep.angleE = 0;
                firstStep.angleF = 0;

                // 수정된 첫 번째 스텝 저장
                writer.WriteLine($"{firstStep.angleA},{firstStep.angleB},{firstStep.angleC},{firstStep.angleD},{firstStep.angleE},{firstStep.angleF},{firstStep.speed},{firstStep.delay}");
            }

            // 중간 스텝 저장
            for (int i = 1; i < steps.Count - 1; i++)
            {
                var step = steps[i];
                writer.WriteLine($"{step.angleA},{step.angleB},{step.angleC},{step.angleD},{step.angleE},{step.angleF},{step.speed},{step.delay}");
            }

            // 마지막 스텝이 있을 경우, 각도를 0으로 설정
            if (steps.Count > 1)
            {
                var lastStep = steps[steps.Count - 1];
                lastStep.angleA = 0;
                lastStep.angleB = 0;
                lastStep.angleC = 0;
                lastStep.angleD = 0;
                lastStep.angleE = 0;
                lastStep.angleF = 0;

                // 수정된 마지막 스텝 저장
                writer.WriteLine($"{lastStep.angleA},{lastStep.angleB},{lastStep.angleC},{lastStep.angleD},{lastStep.angleE},{lastStep.angleF},{lastStep.speed},{lastStep.delay}");
            }
        }

        Debug.Log("Saved steps to CSV: " + steps.Count + " steps.");
    }


    public void OnStartButton()
    {
        if (!isRunning)
        {
            isRunning = true; // 실행 중임을 표시
            StartCoroutine(RunSteps());
        }
    }

    public void OnStopButton()
    {
        isRunning = false; // 실행 중지
        StopAllCoroutines(); // 모든 코루틴 중지
    }

    private IEnumerator RunSteps()
    {
        while (isRunning)
        {
            if (steps.Count == 0)
            {
                Debug.Log("No steps to run."); // 스텝이 없을 경우 로그
                yield break; // 코루틴 종료
            }

            for (int i = 0; i < steps.Count; i++)
            {
                Step prevStep = (i == 0)
                    ? new Step(0, 0, 0, 0, 0, 0, 0, 0) // 첫 번째 스텝의 경우 초기값 사용
                    : steps[i - 1];

                yield return RunStep(prevStep, steps[i]);
            }
        }
    }

    private IEnumerator RunStep(Step prevStep, Step nowStep)
    {
        float elapsedTime = 0f;
        float totalDuration = 1f; // 고정된 시간 (예: 1초)
        Vector3 targetA = new Vector3(0, 0, nowStep.angleA);
        Vector3 targetB = new Vector3(nowStep.angleB, 0, 0);
        Vector3 targetC = new Vector3(nowStep.angleC, 0, 0);
        Vector3 targetD = new Vector3(0, 0, nowStep.angleD);
        Vector3 targetE = new Vector3(nowStep.angleE, 0, 0);
        Vector3 targetF = new Vector3(0, 0, nowStep.angleF);

        // 이전 각도를 설정
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

            yield return null; // 다음 프레임까지 대기
        }

        // 각 스텝 사이의 지연 추가
        yield return new WaitForSeconds(nowStep.delay);
    }


    // 회전 메서드 추가
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

        newAngle = Mathf.Clamp(newAngle, -359, 359); // 각도 제한

        // A, D, F는 Z축, B, C, E는 X축 기준으로 회전
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

