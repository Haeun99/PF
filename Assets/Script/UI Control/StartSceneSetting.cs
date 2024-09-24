using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class StartSceneSetting : MonoBehaviour
{
    public static StartSceneSetting Instance { get; private set; }

    public Button logInButton;
    public RectTransform logInPanel;
    public GameObject logInError;
    public Button signUpButton;
    public RectTransform signUpPanel;
    public GameObject idOverlap;
    public GameObject nicknameOverlap;
    public GameObject bothOverlap;
    public Button logInCancelButton;
    public Button signUpCancelButton;
    public Button loginConfirmButton;
    public Button signUpConfirmButton;

    [Space(20)]
    public Button startButton;
    public Button settingButton;
    public Button backButton;
    public RectTransform background;

    [Space(20)]
    public RectTransform gameEndPanel;
    public Button gameEndButton;
    public Button stayButton;
    public Button exitButton;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        startButton.onClick.AddListener(() => LoadScene("CutScene"));
        settingButton.onClick.AddListener(() => ToggleMainMenu(false));
        backButton.onClick.AddListener(() => ToggleMainMenu(true));
        logInButton.onClick.AddListener(() => LoginPanel(logInPanel));
        signUpButton.onClick.AddListener(() => SignUpPanel(signUpPanel));
        logInCancelButton.onClick.AddListener(() => CancelPanel(logInPanel));
        signUpCancelButton.onClick.AddListener(() => CancelPanel(signUpPanel));
        gameEndButton.onClick.AddListener(EndGame);
        stayButton.onClick.AddListener(() => PanelClose(gameEndPanel));
        exitButton.onClick.AddListener(() => PanelOpen(gameEndPanel));
        loginConfirmButton.onClick.AddListener(LoginConfirm);
        signUpConfirmButton.onClick.AddListener(SignUpConfirm);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameEndPanel != null)
            {
                PanelOpen(gameEndPanel);
            }
        }
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void PanelOpen(RectTransform panel)
    {
        Time.timeScale = 0;
        panel.gameObject.SetActive(true);
    }

    private void PanelClose(RectTransform panel)
    {
        panel.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    private void ToggleMainMenu(bool isActive)
    {
        startButton.gameObject.SetActive(isActive);
        settingButton.gameObject.SetActive(isActive);
        background.gameObject.SetActive(!isActive);
    }

    private void LoginPanel(RectTransform panel)
    {
        logInButton.gameObject.SetActive(false);
        signUpButton.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);

        DatabaseManager.Instance.loginEmailInput.text = string.Empty;
        DatabaseManager.Instance.loginPWInput.text = string.Empty;
        DatabaseManager.Instance.loginError.gameObject.SetActive(false);
    }

    private void SignUpPanel(RectTransform panel)
    {
        logInButton.gameObject.SetActive(false);
        signUpButton.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);

        DatabaseManager.Instance.signupEmailInput.text = string.Empty;
        DatabaseManager.Instance.signupNNInput.text = string.Empty;
        DatabaseManager.Instance.signupPWInput.text = string.Empty;
        DatabaseManager.Instance.signupIDError.gameObject.SetActive(false);
        DatabaseManager.Instance.signupNNError.gameObject.SetActive(false);
        DatabaseManager.Instance.signupSuccess.gameObject.SetActive(false);
    }

    private void CancelPanel(RectTransform panel)
    {
        logInButton.gameObject.SetActive(true);
        signUpButton.gameObject.SetActive(true);
        panel.gameObject.SetActive(false);
    }

    private void LoginConfirm()
    {
        DatabaseManager.Instance.Login();
    }

    private void SignUpConfirm()
    {
        DatabaseManager.Instance.SignUp();
    }

    private void EndGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}