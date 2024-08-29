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
    public RectTransform creatRoomPanel;

    [Space(20)]
    public Button findRoomButton;
    public Button quitFindButton;
    public RectTransform findRoomPanel;

    [Space(20)]
    public Button chatButton;
    public RectTransform chatPanel;
    private bool isOpen = false;

    private void Start()
    {
        backToVillageButton.onClick.AddListener(BTVButtonClick);
        chatButton.onClick.AddListener(chatButtonClick);
        createRoomButton.onClick.AddListener(createRoomButtonClick);
        findRoomButton.onClick.AddListener(findRoomButtonClick);
        quitCreateButton.onClick.AddListener(quitCreateButtonClick);
        quitFindButton.onClick.AddListener(quitFindButtonClick);
    }

    public void BTVButtonClick()
    {
        SceneManager.LoadScene("Game_Scene");
    }

    public void chatButtonClick()
    {
        isOpen = !isOpen;
        chatPanel.gameObject.SetActive(isOpen);
    }

    public void createRoomButtonClick()
    {
        creatRoomPanel.gameObject.SetActive(true);
        backToVillageButton.gameObject.SetActive(false);
        createRoomButton.gameObject.SetActive(false);
        findRoomButton.gameObject.SetActive(false);
    }

    public void findRoomButtonClick()
    {
        findRoomPanel.gameObject.SetActive(true);
        backToVillageButton.gameObject.SetActive(false);
        createRoomButton.gameObject.SetActive(false);
        findRoomButton.gameObject.SetActive(false);
    }

    public void quitCreateButtonClick()
    {
        creatRoomPanel.gameObject.SetActive(false);
        backToVillageButton.gameObject.SetActive(true);
        createRoomButton.gameObject.SetActive(true);
        findRoomButton.gameObject.SetActive(true);
    }

    public void quitFindButtonClick()
    {
        findRoomPanel.gameObject.SetActive(false);
        backToVillageButton.gameObject.SetActive(true);
        createRoomButton.gameObject.SetActive(true);
        findRoomButton.gameObject.SetActive(true);
    }
}