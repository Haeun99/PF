using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MafiaSceneUIManager : MonoBehaviour
{
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

    [Space(20)]
    public Button roomEnterButton;
    public Button codeButton;
    public RectTransform passwordPopup;
    public RectTransform inviteCodePopup;
    public Button inviteCodeConfirmButton;
    public Button quitPasswordButton;
    public Button quitCodeButton;

    [Space(20)]
    public Button chatButton;
    public RectTransform chatPanel;
    private bool isOpen = false;

    private void Start()
    {
        backToVillageButton.onClick.AddListener(() => SceneManager.LoadScene("Game_Scene"));
        chatButton.onClick.AddListener(ToggleChatPanel);
        createRoomButton.onClick.AddListener(() => TogglePanel(createRoomPanel));
        findRoomButton.onClick.AddListener(() => TogglePanel(findRoomPanel));
        quitCreateButton.onClick.AddListener(() => ClosePanel(createRoomPanel));
        quitFindButton.onClick.AddListener(() => ClosePanel(findRoomPanel));
        createButton.onClick.AddListener(() => ToStartGamePanel(lobbyPanel));
        createQuitButton.onClick.AddListener(() => ClosePanel(createRoomPanel));
        roomEnterButton.onClick.AddListener(() => ToStartGamePanel(lobbyPanel));
        codeButton.onClick.AddListener(() => TogglePanel(inviteCodePopup));
        inviteCodeConfirmButton.onClick.AddListener(() => ToStartGamePanel(lobbyPanel));
        quitPasswordButton.onClick.AddListener(() => ClosePanel(passwordPopup));
        quitCodeButton.onClick.AddListener(() => ClosePanel(inviteCodePopup));
    }

    private void ToggleChatPanel()
    {
        isOpen = !isOpen;
        chatPanel.gameObject.SetActive(isOpen);
    }

    private void TogglePanel(RectTransform panel)
    {
        panel.gameObject.SetActive(true);
        SetMainButtonsActive(false);
    }

    private void ClosePanel(RectTransform panel)
    {
        panel.gameObject.SetActive(false);
        SetMainButtonsActive(true);
    }

    private void ToStartGamePanel(RectTransform panel)
    {
        panel.gameObject.SetActive(true);
        SetPopupActive();
    }

    private void SetMainButtonsActive(bool isActive)
    {
        backToVillageButton.gameObject.SetActive(isActive);
        createRoomButton.gameObject.SetActive(isActive);
        findRoomButton.gameObject.SetActive(isActive);
    }

    private void SetPopupActive()
    {
        createRoomPanel.gameObject.SetActive(false);
        passwordPopup.gameObject.SetActive(false);
        inviteCodePopup.gameObject.SetActive(false);
    }
}