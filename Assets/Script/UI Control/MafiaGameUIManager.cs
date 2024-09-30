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
    public Button playerSettingCheckButton;

    [Space(20)]
    public Button quitButton;
    public Button readyButton;
    public Button startButton;
    public Button settingButton;

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
        quitGameButton.onClick.AddListener(QuitGame);
        chatButton.onClick.AddListener(() => RepeatTogglePanel(chatPanel));
        settingCheckButton.onClick.AddListener(() => ClosePanel(settingPanel));
        playerSettingCheckButton.onClick.AddListener(() => ClosePanel(playerRoomSettingPanel));
        roomSettingButton.onClick.AddListener(OpenPanel);
    }

    private void RepeatTogglePanel(RectTransform panel)
    {
        isOpen = !isOpen;
        panel.gameObject.SetActive(isOpen);
        quitButton.gameObject.SetActive(!isOpen);
        settingButton.gameObject.SetActive(!isOpen);

        if (PhotonNetwork.IsMasterClient)
        {
            startButton.gameObject.SetActive(!isOpen);
        }
        else
        {
            readyButton.gameObject.SetActive(!isOpen);
        }
    }

    private void OpenPanel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            masterRoomSettingPanel.gameObject.SetActive(true);
            playerRoomSettingPanel.gameObject.SetActive(false);
        }

        else
        {
            masterRoomSettingPanel.gameObject.SetActive(false);
            playerRoomSettingPanel.gameObject.SetActive(true);
        }
    }

    private void QuitGame()
    {
        SceneManager.LoadScene("Game_Scene");

        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }

        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        settingPanel.gameObject.SetActive(false);
        MafiaSceneUIManager.Instance.canvas.gameObject.SetActive(false);
    }

    private void ClosePanel(RectTransform panel)
    {
        panel.gameObject.SetActive(false);
    }
}
