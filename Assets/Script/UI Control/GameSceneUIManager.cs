using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameSceneUIManager : MonoBehaviour
{
    public Button menuButton;
    public RectTransform menuUI;
    public RectTransform menuList;

    public Button mafiaButton;

    public Button shopButton;
    public Button closeShopButton;
    public RectTransform shopPanel;

    public Button cashButton;
    public Button closeCashButton;
    public RectTransform cashPanel;

    public Button settingButton;
    public Button closeSettingButton;
    public RectTransform settingPanel;

    public Button gameEndButton;
    public Button stayButton;
    public Button endButton;
    public RectTransform gameEndPanel;

    private bool isOpen = false;

    private void Awake()
    {
        Time.timeScale = 1;

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        menuButton.onClick.AddListener(ToggleMenu);
        mafiaButton.onClick.AddListener(() => SceneManager.LoadScene("Mafia_Scene"));
        shopButton.onClick.AddListener(() => OpenPanel(shopPanel));
        cashButton.onClick.AddListener(() => OpenPanel(cashPanel));
        settingButton.onClick.AddListener(() => OpenPanelWithMenuClose(settingPanel));
        gameEndButton.onClick.AddListener(() => OpenPanelWithMenuClose(gameEndPanel));
        closeShopButton.onClick.AddListener(() => ClosePanel(shopPanel));
        closeCashButton.onClick.AddListener(() => ClosePanel(cashPanel));
        closeSettingButton.onClick.AddListener(() => ClosePanelAndOpenMenu(settingPanel));
        stayButton.onClick.AddListener(() => ClosePanelAndOpenMenu(gameEndPanel));
        endButton.onClick.AddListener(EndGame);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameEndPanel.gameObject.SetActive(true);
        }
    }

    private void ToggleMenu()
    {
        isOpen = !isOpen;
        menuList.gameObject.SetActive(isOpen);
    }

    private void OpenPanel(RectTransform panel)
    {
        panel.gameObject.SetActive(true);
    }

    private void ClosePanel(RectTransform panel)
    {
        panel.gameObject.SetActive(false);
    }

    private void OpenPanelWithMenuClose(RectTransform panel)
    {
        OpenPanel(panel);
        menuUI.gameObject.SetActive(false);
    }

    private void ClosePanelAndOpenMenu(RectTransform panel)
    {
        ClosePanel(panel);
        menuUI.gameObject.SetActive(true);
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