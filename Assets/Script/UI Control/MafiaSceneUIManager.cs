using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MafiaSceneUIManager : MonoBehaviour
{
    public static MafiaSceneUIManager Instance { get; private set; }

    public Button backToVillageButton;

    [Space(20)]
    public Button createRoomButton;
    public Button quitCreateButton;
    public RectTransform createRoomPanel;

    [Space(20)]
    public Button findRoomButton;
    public Button quitFindButton;
    public RectTransform findRoomPanel;

    [Space(20)]
    public Button createButton;
    public Button createQuitButton;
    public RectTransform lobbyPanel;
    public Button quitButton;

    [Space(20)]
    public Button codeButton;
    public RectTransform passwordPopup;
    public RectTransform inviteCodePopup;
    public Button quitPasswordButton;
    public Button quitCodeButton;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        backToVillageButton.onClick.AddListener(() => SceneManager.LoadScene("Game_Scene"));
        createRoomButton.onClick.AddListener(() => TogglePanel(createRoomPanel));
        findRoomButton.onClick.AddListener(() => FindGame(findRoomPanel));
        quitCreateButton.onClick.AddListener(() => ClosePanel(createRoomPanel));
        quitFindButton.onClick.AddListener(() => ClosePanel(findRoomPanel));
        createButton.onClick.AddListener(() => CreateGamePanel(lobbyPanel));
        createQuitButton.onClick.AddListener(() => ClosePanel(createRoomPanel));
        codeButton.onClick.AddListener(() => TogglePanel(inviteCodePopup));
        quitPasswordButton.onClick.AddListener(() => ClosePanel(passwordPopup));
        quitCodeButton.onClick.AddListener(() => ClosePanel(inviteCodePopup));
        quitButton.onClick.AddListener(() => GameQuit(lobbyPanel));
    }

    private void TogglePanel(RectTransform panel)
    {
        panel.gameObject.SetActive(true);
        SetMainButtonsActive(false);

        InitInput();
    }

    private void ClosePanel(RectTransform panel)
    {
        panel.gameObject.SetActive(false);
        SetMainButtonsActive(true);
    }

    private void GameQuit(RectTransform panel)
    {
        panel.gameObject.SetActive(false);
        SetMainButtonsActive(true);

        PhotonNetwork.LeaveRoom();
    }

    private void FindGame(RectTransform panel)
    {
        panel.gameObject.SetActive(true);
        SetMainButtonsActive(false);

        PhotonNetwork.JoinLobby();
    }

    private void CreateGamePanel(RectTransform panel)
    {
        CreateRoom.Instance.CreateMafiaRoom();

        panel.gameObject.SetActive(true);
        SetPopupActive();
    }

    private void SetMainButtonsActive(bool isActive)
    {
        backToVillageButton.gameObject.SetActive(isActive);
        createRoomButton.gameObject.SetActive(isActive);
        findRoomButton.gameObject.SetActive(isActive);
    }

    public void SetPopupActive()
    {
        createRoomPanel.gameObject.SetActive(false);
        passwordPopup.gameObject.SetActive(false);
        inviteCodePopup.gameObject.SetActive(false);
        findRoomPanel.gameObject.SetActive(false);
    }

    private void InitInput()
    {
        CreateRoom.Instance.roomNameInput.text = string.Empty;
        CreateRoom.Instance.roomPWInput.text = string.Empty;
        CreateRoom.Instance.privateMode.isOn = false;
        CreateRoom.Instance.maxPlayerDropdwon.value = 0;
        FindRoom.Instance.inviteCodeInput.text = string.Empty;
        FindRoom.Instance.pwError.gameObject.SetActive(false);
        FindRoom.Instance.inviteCodeError.gameObject.SetActive(false);
    }
}