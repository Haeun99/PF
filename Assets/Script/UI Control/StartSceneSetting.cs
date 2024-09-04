using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneSetting : MonoBehaviour
{
    public Button logInButton;
    public RectTransform logInPanel;
    public Button logInConfirmButton;
    public GameObject logInError;
    public Button signUpButton;
    public RectTransform signUpPanel;
    public Button signUpConfirmButton;
    public GameObject idOverlap;
    public GameObject nicknameOverlap;
    public GameObject bothOverlap;

    [Space(20)]
    public Button startButton;
    public Button settingButton;
    public Button backButton;
    public RectTransform background;

    private void Start()
    {
        startButton.onClick.AddListener(() => LoadScene("CutScene"));
        settingButton.onClick.AddListener(() => ToggleMainMenu(false));
        backButton.onClick.AddListener(() => ToggleMainMenu(true));
        logInButton.onClick.AddListener(() => OpenPanel(logInPanel));
        logInConfirmButton.onClick.AddListener(ConfirmLogIn);
        signUpButton.onClick.AddListener(() => OpenPanel(signUpPanel));
        signUpConfirmButton.onClick.AddListener(ConfirmSignUp);
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void ToggleMainMenu(bool isActive)
    {
        startButton.gameObject.SetActive(isActive);
        settingButton.gameObject.SetActive(isActive);
        background.gameObject.SetActive(!isActive);
    }

    private void OpenPanel(RectTransform panel)
    {
        logInButton.gameObject.SetActive(false);
        signUpButton.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
    }

    private void ClosePanel(RectTransform panel)
    {
        panel.gameObject.SetActive(false);
        ToggleMainMenu(true);
    }

    private void ConfirmLogIn()
    {
        // 로그인 기능 추가
        ClosePanel(logInPanel);
    }

    private void ConfirmSignUp()
    {
        // 회원가입 기능 추가
        ClosePanel(signUpPanel);
    }
}
