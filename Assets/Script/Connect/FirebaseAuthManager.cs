using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase;
using System;
using UnityEngine.LightTransport;
using System.Security.Cryptography;
using Firebase.Database;
using Newtonsoft.Json;

/// <summary>
/// 1. �α���: �̸���, �н����� �Է½� ȸ������ ���ο� ���� �α���
/// 2. ȸ������: �̸���, �н����� �Է� �� �̸��� ������ �ȴٸ� ȸ������ �Ϸ�
/// 3. �ҷ�����: ���ѿ� ���� DB�� Ư�� ������ �ҷ�����
/// 
/// class diagram
/// (-) Initialization(void)
/// (+) SignIn(string, string)
/// (+) SignUP(string, string)
/// (-) SendVerificationEmail(string)
/// </summary>
public class FirebaseAuthManager : MonoBehaviour
{
    [Serializable]
    public class UserInfo
    {
        public string email;
        public string name;
        public string role;
    }

    [Header("�α��� ���� UI")]
    [SerializeField] GameObject signInPanel;
    [SerializeField] TMP_InputField signInEmailInput;
    [SerializeField] TMP_InputField signInPWInput;
    [SerializeField] Button signInBtn;
    [SerializeField] Button signUpBtn;
    [SerializeField] Button exitBtn;
    [SerializeField] TMP_Text userInfoTxt;
    [SerializeField] UserInfo userSignedIn;

    [Header("ȸ������ ���� UI")]
    [SerializeField] GameObject signUpPanel;
    [SerializeField] TMP_InputField signUpNameInput;
    [SerializeField] TMP_InputField signUpEmailInput;
    [SerializeField] TMP_InputField signUpPWInput;
    [SerializeField] TMP_InputField signUpPWCheckInput;
    [SerializeField] Button registerBtn;
    [SerializeField] Button cancleBtn;
    [SerializeField] GameObject verificationPanel;
    [SerializeField] TMP_Text verificationMsg;
    int pwWrongCnt;

    FirebaseAuth auth;
    FirebaseUser user;

    void Start()
    {
        Initialization();
    }

    void Initialization()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;

        AuthStateChanged(this, null);
        auth.SignOut();
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool isSignedIn = (user != auth.CurrentUser) && (auth.CurrentUser != null);

            if (isSignedIn == false && user != null)
            {
                print("Signed Out " + user.UserId);
            }

            user = auth.CurrentUser;

            if (isSignedIn == true)
            {
                print("Signed In " + user.UserId);
            }
        }
    }
    public void OnSignInBtnClkEvent()
    {
        signInPanel.SetActive(true);
        signUpPanel.SetActive(false);

        StartCoroutine(SignIn(signInEmailInput.text, signInPWInput.text));
    }

    public IEnumerator SignIn(string email, string password)
    {
        Task t = auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => t.IsCompleted);

        if (user != null)
        {
            try
            {
                if (user.IsEmailVerified)
                {
                    StartCoroutine(TurnMessagePanel("�α����� ���������� �Ϸ�Ǿ����ϴ�."));
                    print("�α����� �Ǿ����ϴ�.");
                    signInEmailInput.text = "";
                    signInPWInput.text = "";
                }
                else if (!user.IsEmailVerified)
                {
                    StartCoroutine(TurnMessagePanel($"���� ���� ������ �Ͻñ� �ٶ��ϴ�."));
                    print($"���� ���� ������ �Ͻñ� �ٶ��ϴ�.");
                    signInEmailInput.text = "";
                    signInPWInput.text = "";
                }
            }
            catch (Exception e)
            {
                print(e.ToString());
                // ��й�ȣ�� Ʋ�� ���
                pwWrongCnt++;
                if (pwWrongCnt >= 5)
                {
                    StartCoroutine(TurnMessagePanel("��й�ȣ�� 5ȸ �̻� Ʋ�Ƚ��ϴ�. ��й�ȣ �ʱ�ȭ ������ �߼��մϴ�."));
                    StartCoroutine(PasswordReset(email));
                }
                else
                {
                    StartCoroutine(TurnMessagePanel($"��й�ȣ�� Ʋ�Ƚ��ϴ�. {5 - pwWrongCnt}ȸ ���ҽ��ϴ�."));
                }
            }
        }

        else if (user == null)
        {
            StartCoroutine(TurnMessagePanel("��ϵ� ������ �����ϴ�."));
            print("��ϵ� ������ �����ϴ�.");
        }

        yield return new WaitForSeconds(3);

        if (verificationPanel.activeSelf)
        {
            verificationPanel.SetActive(false);
        }
    }
    public void OnShowInfoBtnClkEvent()
    {
        if (user != null && user.IsEmailVerified == true)
        {
            ReadUserData();
            print("���� ������ �ҷ��Խ��ϴ�");
        }
        else
        {
            StartCoroutine(TurnMessagePanel("�α����� �ϰų� ���� ������ ���ּ���."));
            print("�α����� �ϰų� ���� ������ ���ּ���.");
        }
    }

    public void OnSignUpBtnclkEvent()
    {
        signInPanel.SetActive(false);
        signUpPanel.SetActive(true);
    }
    public void OnNewRegisterBtnClkEvent()
    {
        StartCoroutine(SignUp(signUpEmailInput.text, signUpPWInput.text, signUpPWCheckInput.text));
    }
    public IEnumerator SignUp(string email, string password, string passwordCheck)
    {
        if (email == "" || password == "" || passwordCheck == "")
        {
            StartCoroutine(TurnMessagePanel("�̸��� �Ǵ� �н����带 �Է����ּ���."));
            print("�̸��� �Ǵ� �н����带 �Է����ּ���.");
            yield break;
        }

        if (password != passwordCheck)
        {
            StartCoroutine(TurnMessagePanel("��й�ȣ�� ���� �ʽ��ϴ�."));
            print("��й�ȣ�� ���� �ʽ��ϴ�.");
            yield break;
        }

        Task task = auth.CreateUserWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => task.IsCompleted == true);

        if (task.Exception != null)
        {
            FirebaseException e = task.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)e.ErrorCode;

            switch (authError)
            {
                case AuthError.InvalidEmail:
                    StartCoroutine(TurnMessagePanel("��ȿ���� ���� �̸��� �����Դϴ�."));
                    print("��ȿ���� ���� �̸��� �����Դϴ�");
                    break;
                case AuthError.WeakPassword:
                    StartCoroutine(TurnMessagePanel("��й�ȣ�� ����մϴ�."));
                    print("��й�ȣ�� ����մϴ�.");
                    break;
                case AuthError.EmailAlreadyInUse:
                    StartCoroutine(TurnMessagePanel("�̹� ������ �����Դϴ�."));
                    print("�̹� ������ �����Դϴ�.");
                    break;
                case AuthError.WrongPassword:
                    StartCoroutine(TurnMessagePanel("�߸��� ��й�ȣ�Դϴ�."));
                    print("�߸��� ��й�ȣ�Դϴ�.");
                    break;
            }
        }
        else
        {
            StartCoroutine(SendVerificationEmail(email));
            WriteUserData();
        }

        yield return new WaitForSeconds(1);

        signUpNameInput.text = "";
        signUpEmailInput.text = "";
        signUpPWInput.text = "";
        signUpPWCheckInput.text = "";
    }

    private void WriteUserData()
    {
        if (DBManager.instance != null)
        {
            DatabaseReference dbref = DBManager.instance.dbRef;

            string json =
            $@"{{""role"":""user""
                ""name"":""{signUpNameInput.text}""
                ""email"":""{signUpEmailInput.text}""
                }}";

            dbref.Child("users").Child(user.UserId).SetRawJsonValueAsync(json).ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    print("���� ����� �Ϸ�Ǿ����ϴ�");
                }

                if (task.Exception != null)
                    print(task.Exception.ToString());
            });
        }
    }
    private async void ReadUserData()
    {
        if (user != null)
        {
            string userInfo = string.Empty;

            if (DBManager.instance != null)
            {
                DatabaseReference dbref = DBManager.instance.dbRef;

                var userSnapshot = await dbref.Child("users").Child(user.UserId).GetValueAsync();
                string json = userSnapshot.GetRawJsonValue();
                userSignedIn = JsonConvert.DeserializeObject<UserInfo>(json);

                if (userSignedIn.role == "user")
                {
                    foreach (var data in userSnapshot.Children)
                    {
                        userInfo += $"{data.Key}: {data.Value}\n";
                    }
                }
                else if (userSignedIn.role == "admin")
                {
                    var adminSnapshot = await dbref.Child("users").GetValueAsync(); // "user" ��� "users"�� ����
                    foreach (var data in adminSnapshot.Children)
                    {
                        IDictionary value = (IDictionary)data.Value;
                        userInfo += $"{data.Key}: {value["email"]}, {value["name"]}, {value["role"]}\n";
                    }
                }
                userInfoTxt.text = userInfo; // �񵿱� �۾��� �Ϸ�� �Ŀ� ����
            }
        }
    }

    IEnumerator TurnMessagePanel(string message)
    {
        verificationPanel.SetActive(true);
        verificationMsg.text = message;

        yield return new WaitForSeconds(3);

        verificationPanel.SetActive(false);
    }

    private IEnumerator SendVerificationEmail(string email)
    {
        FirebaseUser user = auth.CurrentUser;

        Task task = null;


        if (user != null)
        {
            task = user.SendEmailVerificationAsync();
            
            yield return new WaitUntil(() => task.IsCompleted == true);

            if (task.Exception != null)
            {
                FirebaseException e = task.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)e.ErrorCode;
                print(authError);
            }
        }

        yield return new WaitForSeconds(3);
        
        signInPanel.SetActive(true);
        signUpPanel.SetActive(false);
        verificationPanel.SetActive(false);
        
        StartCoroutine(TurnMessagePanel($"���������� {email}�� ���½��ϴ�. \n�̸����� Ȯ�����ּ���."));
    }
    public IEnumerator PasswordReset(string email)
    {
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            auth.SendPasswordResetEmailAsync(email).ContinueWith(task =>
            {
                if (task.Exception != null)
                    print(task.Exception.Message);
            });
        }
        yield return null;
    }

    public void OnCancleBtnClkEvent()
    {
        signUpPanel.SetActive(false);
        signInPanel.SetActive(true);
    }
    public void OnSignOutBtnClkEvent()
    {
        signUpPanel.SetActive(false);
        signInPanel.SetActive(true);
        auth.SignOut();
        
        StartCoroutine(TurnMessagePanel("������ �����մϴ�."));
        print("�α׾ƿ� �Ǿ����ϴ�.");

        signInEmailInput.text = "";
        signInPWInput.text = "";
        userInfoTxt.text = "User Contents";
    }
    public void OnExitBtnClkEvent()
    {
        Application.Quit();
    }
}
