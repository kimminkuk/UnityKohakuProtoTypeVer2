using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
//using Firebase.Database;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Extensions;

public class FirebaseAuthManager : MonoBehaviour
{
    [SerializeField]
    private PlayerData _playerData;
    private FirebaseAuth auth;
    private FirebaseUser user; 
    
    public InputField emailInputField;
    public InputField passwordInputField;

    const int ERR_PASS = 0;
    const int ERR_EMAIL_SPLIT = 1;
    const int ERR_EMAIL_LENGTH = 2;
    const int ERR_EMAIL_GMAIL_FORM = 3;
    
    const bool IS_TEST = false;

    public GameObject loginOkPanel;
    public GameObject loginFailPanel;

    void Start() {
        auth = FirebaseAuth.DefaultInstance;
    }  
    //1. Login 이후 -> SampleScene으로 이동
    //2. 1번 Login이 가지고 있는 DB정보들 가지고 옵니다.
    //3. 2번의 정보를 가지고 SampleScene에 적용합니다.
    //4. gameLevel, coin
    //5. 가볍게 이렇게 2개만 테이블에 저장해서 테스트합시다.
    public void GetDataOfUser(string userId) {
        // 1. userId에서 @gmail을 제거합니다.
        string _userId = userId.Split('@')[0];
        // 2. 1번 Login이 가지고 있는 DB정보들 가지고 옵니다.
        // 3. 2번의 정보를 가지고 SampleScene에 적용합니다.

        // FirebaseDatabase.DefaultInstance.GetReference("Users").GetValueAsync().ContinueWith(task => {
        //     if (task.IsFaulted) {
        //         Debug.Log("Error");
        //     }
        //     else if (task.IsCompleted) {
        //         DataSnapshot snapshot = task.Result;
        //         Debug.Log(snapshot.Child("gameLevel").Value.ToString());
        //         Debug.Log(snapshot.Child("coin").Value.ToString());
        //         GameManager.instance.gameLevel = int.Parse(snapshot.Child("gameLevel").Value.ToString());
        //         GameManager.instance.coin = int.Parse(snapshot.Child("coin").Value.ToString());
        //     }
        // });
    }

    // public void SetDataOfUser() {
    //     FirebaseDatabase.DefaultInstance.GetReference("Users").Child("gameLevel").SetValueAsync(1);
    //     FirebaseDatabase.DefaultInstance.GetReference("Users").Child("coin").SetValueAsync(100);
    // }
    



    //1. emailValidCheck 함수를 생성합니다.
    //2. emailInputField.text를 parameter로 받아옵니다.
    //2-1. emailInputField.text를 @를 기준으로 split합니다.
    //2-2. 2-1 실패 시, return false 합니다.
    //3. emailInputField.text는 string + @gmail.com 으로 고정됩니다.
    //3-1. string은 20글자 이내입니다.
    //4. 3번의 항목에서 @ 혹은 @gmail.com이 아닐 시, fail return 합니다.
    //5. 4번의 항목에서 @ 혹은 @gmail이 맞을 시, true return 합니다.
    private int emailValidCheck(string email) {

        //2-1. emailInputField.text를 @를 기준으로 split합니다.
        string[] emailSplit = email.Split('@');

        //3. emailInputField.text는 string + @gmail.com 으로 고정됩니다.
        if (emailSplit.Length != 2) {
            return ERR_EMAIL_SPLIT;
        }
        //3-1. emailSplit의 첫 번째 index는 20글자 이내입니다.
        if (emailSplit[0].Length > 20) {
            return ERR_EMAIL_LENGTH;
        }

        //4. 3번의 항목에서 @ 혹은 @gmail이 아닐 시, false return 합니다.
        if (emailSplit[1] != "gmail.com") {
            return ERR_EMAIL_GMAIL_FORM;
        }

        return ERR_PASS;
    }
    
    private void myErrorMessage(int errorNumber) {
        string _init = "Error: ";
        string _message = "";
        switch (errorNumber) {
            case ERR_EMAIL_SPLIT:
                _message = "이메일 형식이 올바르지 않습니다.";
                break;
            case ERR_EMAIL_LENGTH:
                _message = "이메일은 20글자 이내로 작성해주세요.";
                break;
            case ERR_EMAIL_GMAIL_FORM:
                _message = "gamil.com만 가능합니다.";
                break;
        }
        Debug.Log(_init + _message);
    }

    public void Join() {
        Debug.Log("emailInputField.text: " + emailInputField.text + " passwordInputField.text: " + passwordInputField.text);
        int errorNumber = emailValidCheck(emailInputField.text);
        if (errorNumber != ERR_PASS) {
            myErrorMessage(errorNumber);
            return;
        }
        auth.CreateUserWithEmailAndPasswordAsync(emailInputField.text, passwordInputField.text).ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.Log("회원가입 취소");
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled");
                return;
            }
            if (task.IsFaulted) {
                Debug.Log("회원가입 실패");
                //task.Exception.Flatten().InnerExceptions[0].Message.ToString();
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
            
            //Firebase New User Created
            //FirebaseUser newUser = task.Result;
            

            //user = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0}", emailInputField.text);
        });
    }

    // FireBase에 가입한 아이디의 비밀번호는 내가 못보나?? 내가 관리자아닌가??
    // 회원가입 구현, Login하면 Scence으로 이동
    // Login 한거는 Debug로 나오는데.. Scene으로 이동을 안함;;;
    // Login 버튼 누르고, 한 2초 정도 기다린다음에 user 정보를 체크해볼까?
    // Wait도 동작안함.. 그냥 Login 버튼 누르면, Login -> Login Check 두개가 Call하는것을 생각하자
    // Login Check는 Wait한 3초 기다리게..
    public void Login() {
        auth.SignInWithEmailAndPasswordAsync(emailInputField.text, passwordInputField.text).ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
            Debug.LogFormat("task.Result: {0}", task.Result);
            AuthResult authResult = task.Result;
            FirebaseUser user = authResult.User;
            Debug.LogFormat("[1] User signed in successfully: {0}", user.Email);
            Debug.LogFormat("[2] User signed in successfully: {0}", user.Email);
        });             
    }

    // 여기서, Login 시 정보 가져오는거 해야합니다.
    public void LoginVer2() {
        auth.SignInWithEmailAndPasswordAsync(emailInputField.text, passwordInputField.text).ContinueWithOnMainThread(task => {
            if (task.IsCanceled) {
                loginFailPanel.SetActive(true);
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                loginFailPanel.SetActive(true);
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
            if (task.IsCompleted) {
                Debug.Log("Login Complete");
                AuthResult authResult = task.Result;
                FirebaseUser user = authResult.User;
                Debug.LogFormat("task.Result: {0}", task.Result);
                Debug.LogFormat("[1] User signed in successfully: {0}", user.Email);
                Debug.LogFormat("[2] User signed in successfully: {0}", user.Email);            
                LoginPageManager.LoginInstance.IS_LOGIN = true;
                LoginPageManager.LoginInstance.SetUserId(auth.CurrentUser.UserId);
                _playerData.Name = auth.CurrentUser.UserId;
                Debug.Log("[3] auth.CurrentUser.UserId: " + auth.CurrentUser.UserId);
                
                //GameManager.instance.UpdatePlayer(_playerData);
                loginOkPanel.SetActive(true);
            }
        });
    }

    private IEnumerator LoadSampleScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("SampleScene");
        while (!asyncLoad.isDone)
        {
            Debug.Log("Loading...");
            yield return null;
        }
    }

    public void LogOut() {
        auth.SignOut();
        LoginPageManager.LoginInstance.IS_LOGIN = false;
        Debug.Log("LogOut Call");
    }
}
