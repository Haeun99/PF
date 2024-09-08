using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

    public FirebaseApp App { get; private set; }
    public FirebaseAuth Auth { get; private set; }
    public FirebaseDatabase DB { get; private set; }

    public TMP_InputField loginEmailInput;
    public TMP_InputField loginPWInput;
    public TextMeshProUGUI loginError;

    [Space(20)]
    public TMP_InputField signupEmailInput;
    public TMP_InputField signupNNInput;
    public TMP_InputField signupPWInput;
    public TextMeshProUGUI signupIDError;
    public TextMeshProUGUI signupNNError;
    public TextMeshProUGUI signupSuccess;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        App = FirebaseApp.DefaultInstance;
        Auth = FirebaseAuth.DefaultInstance;
        DB = FirebaseDatabase.DefaultInstance;
    }

    public async void Login()
    {
        string email = loginEmailInput.text;
        string pw = loginPWInput.text;

        try
        {
            var result = await Auth.SignInWithEmailAndPasswordAsync(email, pw);
            FirebaseUser user = result.User;

            ConnectPhoton(user.DisplayName);

            StartSceneSetting.Instance.startButton.gameObject.SetActive(true);
            StartSceneSetting.Instance.settingButton.gameObject.SetActive(true);
            StartSceneSetting.Instance.logInButton.gameObject.SetActive(false);
            StartSceneSetting.Instance.signUpButton.gameObject.SetActive(false);
            StartSceneSetting.Instance.logInPanel.gameObject.SetActive(false);

            Debug.Log($"{user.DisplayName}");
        }

        catch (FirebaseException fe)
        {
            Debug.LogError(fe.Message);
            loginError.gameObject.SetActive(true);
        }
    }

    public async void SignUp()
    {
        string email = signupEmailInput.text;
        string nickname = signupNNInput.text;
        string pw = signupPWInput.text;

        try
        {
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("users");
            DataSnapshot snapshot = await reference.OrderByChild("nickname").EqualTo(nickname).GetValueAsync();

            if (snapshot.Exists)
            {
                signupNNError.gameObject.SetActive(true);
                signupIDError.gameObject.SetActive(false);
                return;
            }

            var result = await Auth.CreateUserWithEmailAndPasswordAsync(email, pw);
            FirebaseUser user = result.User;

            UserProfile profile = new UserProfile { DisplayName = nickname };
            await user.UpdateUserProfileAsync(profile);

            Dictionary<string, object> userData = new Dictionary<string, object>
        {
            { "email", email },
            { "nickname", nickname }
        };
            await reference.Child(user.UserId).SetValueAsync(userData);

            signupSuccess.gameObject.SetActive(true);
        }

        catch (FirebaseException ex)
        {
            if (ex.Message.Contains("The email address is already in use"))
            {
                signupIDError.gameObject.SetActive(true);
                signupNNError.gameObject.SetActive(false);
            }
        }
    }

    private void ConnectPhoton(string nickname)
    {
        PhotonNetwork.NickName = nickname;
        PhotonNetwork.ConnectUsingSettings();
    }
}