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
        startButton.onClick.AddListener(startButtonClick);
        settingButton.onClick.AddListener(settingButtonClick);
        backButton.onClick.AddListener(backButtonClick);
        logInButton.onClick.AddListener(logInButtonClick);
        logInConfirmButton.onClick.AddListener(logInConfirmButtonClick);
        signUpButton.onClick.AddListener(signUpButtonClick);
        signUpConfirmButton.onClick.AddListener(signUpConfirmButtonClick);
    }

    public void startButtonClick()
    {
        SceneManager.LoadScene("Game_Scene");
    }

    public void settingButtonClick()
    {
        startButton.gameObject.SetActive(false);
        settingButton.gameObject.SetActive(false);
        background.gameObject.SetActive(true);
    }

    public void backButtonClick()
    {
        background.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
        settingButton.gameObject.SetActive(true);
    }

    public void logInButtonClick()
    {
        logInButton.gameObject.SetActive(false);
        signUpButton.gameObject.SetActive(false);
        logInPanel.gameObject.SetActive(true);
    }

    public void logInConfirmButtonClick()
    {
        // 로그인 기능 넣어야지ㅣㅣㅣㅣㅣㅣㅣㅣㅣ
        logInPanel.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
        settingButton.gameObject.SetActive(true);
    }

    public void signUpButtonClick()
    {
        logInButton.gameObject.SetActive(false);
        signUpButton.gameObject.SetActive(false);
        signUpPanel.gameObject.SetActive(true);
    }

    public void signUpConfirmButtonClick()
    {
        // 회원가입 기능 넣기ㅣㅣㅣㅣㅣ
        signUpPanel.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
        settingButton.gameObject.SetActive(true);
    }
}
