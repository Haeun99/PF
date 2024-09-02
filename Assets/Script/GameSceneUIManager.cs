using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    private void Start()
    {
        menuButton.onClick.AddListener(menuButtonClick);
        mafiaButton.onClick.AddListener(mafiaButtonClick);
        shopButton.onClick.AddListener(shopButtonClick);
        cashButton.onClick.AddListener(cashButtonClick);
        settingButton.onClick.AddListener(settingButtonClick);
        gameEndButton.onClick.AddListener(gameEndButtonClick);
        closeShopButton.onClick.AddListener(closeShopButtonClick);
        closeCashButton.onClick.AddListener(closeCashButtonClick);
        closeSettingButton.onClick.AddListener(closeSettingButtonClick);
        stayButton.onClick.AddListener(stayButtonClick);
        endButton.onClick.AddListener(endButtonClick);
    }

    public void menuButtonClick()
    {
        isOpen = !isOpen;
        menuList.gameObject.SetActive(isOpen);
    }

    public void mafiaButtonClick()
    {
        SceneManager.LoadScene("Mafia_Scene");
    }

    public void shopButtonClick()
    {
        shopPanel.gameObject.SetActive(true);
    }

    public void cashButtonClick()
    {
        cashPanel.gameObject.SetActive(true);
    }

    public void settingButtonClick()
    {
        settingPanel.gameObject.SetActive(true);
        menuUI.gameObject.SetActive(false);
    }

    public void gameEndButtonClick()
    {
        gameEndPanel.gameObject.SetActive(true);
        menuUI.gameObject.SetActive(false);
    }

    public void closeShopButtonClick()
    {
        shopPanel.gameObject.SetActive(false);
    }

    public void closeCashButtonClick()
    {
        cashPanel.gameObject.SetActive(false);
    }

    public void closeSettingButtonClick()
    {
        settingPanel.gameObject.SetActive(false);
        menuUI.gameObject.SetActive(true);
    }

    public void stayButtonClick()
    {
        gameEndPanel.gameObject.SetActive(false);
        menuUI.gameObject.SetActive(true);
    }

    public void endButtonClick()
    {
        // 게임 종료
    }
}