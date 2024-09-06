using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class StartSceneSetting : MonoBehaviour
{
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PanelOpen(gameEndPanel);
        }
    }

    private void Start()
    {
        startButton.onClick.AddListener(() => LoadScene("CutScene"));
        settingButton.onClick.AddListener(() => ToggleMainMenu(false));
        backButton.onClick.AddListener(() => ToggleMainMenu(true));
        logInButton.onClick.AddListener(() => OpenPanel(logInPanel));
        signUpButton.onClick.AddListener(() => OpenPanel(signUpPanel));
        logInCancelButton.onClick.AddListener(() => CancelPanel(logInPanel));
        signUpCancelButton.onClick.AddListener(() => CancelPanel(signUpPanel));
        gameEndButton.onClick.AddListener(EndGame);
        stayButton.onClick.AddListener(() => PanelClose(gameEndPanel));
        exitButton.onClick.AddListener(() => PanelOpen(gameEndPanel));
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

    private void CancelPanel(RectTransform panel)
    {
        logInButton.gameObject.SetActive(true);
        signUpButton.gameObject.SetActive(true);
        panel.gameObject.SetActive(false);
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