using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MafiaGameUIManager : MonoBehaviour
{
    public RectTransform lobbyPanel;
    public RectTransform settingPanel;
    public Button settingCheckButton;
    public Button quitGameButton;

    [Space(20)]
    public Button chatButton;
    public RectTransform chatPanel;
    private bool isOpen = false;

    [Space(20)]
    public Button roomSettingButton;
    public RectTransform masterRoomSettingPanel;
    public RectTransform playerRoomSettingPanel;
    public Button masterSettingFinButton;
    public Button playerSettingCheckButton;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isOpen = !isOpen;
            settingPanel.gameObject.SetActive(isOpen);
        }
    }

    private void Start()
    {
        quitGameButton.onClick.AddListener(() => SceneManager.LoadScene("Game_Scene"));
        chatButton.onClick.AddListener(() => RepeatTogglePanel(chatPanel));
        settingCheckButton.onClick.AddListener(() => ClosePanel(settingPanel));
        masterSettingFinButton.onClick.AddListener(() => ClosePanel(masterRoomSettingPanel));
        playerSettingCheckButton.onClick.AddListener(() => ClosePanel(playerRoomSettingPanel));

        //if (isMaster)
        //{
        //    roomSettingButton.onClick.AddListener(() => OpenPanel(masterRoomSettingPanel));
        //}

        //else
        //{
        //    roomSettingButton.onClick.AddListener(() => OpenPanel(playerRoomSettingPanel));
        //}
    }

    private void RepeatTogglePanel(RectTransform panel)
    {
        isOpen = !isOpen;
        panel.gameObject.SetActive(isOpen);
        // 버튼 토글 로직 추가해야함
    }

    private void OpenPanel(RectTransform panel)
    {
        // isMaster / !isMaster 구분할 것
        panel.gameObject.SetActive(true);
    }

    private void ClosePanel(RectTransform panel)
    {
        panel.gameObject.SetActive(false);
    }
}
