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
/// 1. 로그인: 이메일, 패스워드 입력시 회원가입 여부에 따라 로그인
/// 2. 회원가입: 이메일, 패스워드 입력 후 이메일 인증이 된다면 회원가입 완료
/// 3. 불러오기: 권한에 따라 DB의 특정 정보를 불러오기
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

    [Header("로그인 관련 UI")]
    [SerializeField] GameObject signInPanel;
    [SerializeField] TMP_InputField signInEmailInput;
    [SerializeField] TMP_InputField signInPWInput;
    [SerializeField] Button signInBtn;
    [SerializeField] Button signUpBtn;
    [SerializeField] Button exitBtn;
    [SerializeField] TMP_Text userInfoTxt;
    [SerializeField] UserInfo userSignedIn;

    [Header("회원가입 관련 UI")]
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
                    StartCoroutine(TurnMessagePanel("로그인이 성공적으로 완료되었습니다."));
                    print("로그인이 되었습니다.");
                    signInEmailInput.text = "";
                    signInPWInput.text = "";
                }
                else if (!user.IsEmailVerified)
                {
                    StartCoroutine(TurnMessagePanel($"먼저 메일 인증을 하시기 바랍니다."));
                    print($"먼저 메일 인증을 하시기 바랍니다.");
                    signInEmailInput.text = "";
                    signInPWInput.text = "";
                }
            }
            catch (Exception e)
            {
                print(e.ToString());
                // 비밀번호가 틀린 경우
                pwWrongCnt++;
                if (pwWrongCnt >= 5)
                {
                    StartCoroutine(TurnMessagePanel("비밀번호가 5회 이상 틀렸습니다. 비밀번호 초기화 메일을 발송합니다."));
                    StartCoroutine(PasswordReset(email));
                }
                else
                {
                    StartCoroutine(TurnMessagePanel($"비밀번호가 틀렸습니다. {5 - pwWrongCnt}회 남았습니다."));
                }
            }
        }

        else if (user == null)
        {
            StartCoroutine(TurnMessagePanel("등록된 정보가 없습니다."));
            print("등록된 정보가 없습니다.");
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
            print("유저 정보를 불러왔습니다");
        }
        else
        {
            StartCoroutine(TurnMessagePanel("로그인을 하거나 메일 인증을 해주세요."));
            print("로그인을 하거나 메일 인증을 해주세요.");
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
            StartCoroutine(TurnMessagePanel("이메일 또는 패스워드를 입력해주세요."));
            print("이메일 또는 패스워드를 입력해주세요.");
            yield break;
        }

        if (password != passwordCheck)
        {
            StartCoroutine(TurnMessagePanel("비밀번호가 같지 않습니다."));
            print("비밀번호가 같지 않습니다.");
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
                    StartCoroutine(TurnMessagePanel("유효하지 않은 이메일 포멧입니다."));
                    print("유효하지 않은 이메일 포멧입니다");
                    break;
                case AuthError.WeakPassword:
                    StartCoroutine(TurnMessagePanel("비밀번호가 취약합니다."));
                    print("비밀번호가 취약합니다.");
                    break;
                case AuthError.EmailAlreadyInUse:
                    StartCoroutine(TurnMessagePanel("이미 가입한 계정입니다."));
                    print("이미 가입한 계정입니다.");
                    break;
                case AuthError.WrongPassword:
                    StartCoroutine(TurnMessagePanel("잘못된 비밀번호입니다."));
                    print("잘못된 비밀번호입니다.");
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
                    print("유저 등록이 완료되었습니다");
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
                    var adminSnapshot = await dbref.Child("users").GetValueAsync(); // "user" 대신 "users"로 변경
                    foreach (var data in adminSnapshot.Children)
                    {
                        IDictionary value = (IDictionary)data.Value;
                        userInfo += $"{data.Key}: {value["email"]}, {value["name"]}, {value["role"]}\n";
                    }
                }
                userInfoTxt.text = userInfo; // 비동기 작업이 완료된 후에 설정
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
        
        StartCoroutine(TurnMessagePanel($"인증메일을 {email}로 보냈습니다. \n이메일을 확인해주세요."));
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
        
        StartCoroutine(TurnMessagePanel("접속을 종료합니다."));
        print("로그아웃 되었습니다.");

        signInEmailInput.text = "";
        signInPWInput.text = "";
        userInfoTxt.text = "User Contents";
    }
    public void OnExitBtnClkEvent()
    {
        Application.Quit();
    }
}
