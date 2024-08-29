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

    public Button creditButton;
    public Button closeCreditButton;
    public RectTransform creditPanel;

    private bool isOpen = false;

    private void Start()
    {
        menuButton.onClick.AddListener(menuButtonClick);
        mafiaButton.onClick.AddListener(mafiaButtonClick);
        shopButton.onClick.AddListener(shopButtonClick);
        cashButton.onClick.AddListener(cashButtonClick);
        settingButton.onClick.AddListener(settingButtonClick);
        creditButton.onClick.AddListener(creditButtonClick);
        closeShopButton.onClick.AddListener(closeShopButtonClick);
        closeCashButton.onClick.AddListener(closeCashButtonClick);
        closeSettingButton.onClick.AddListener(closeSettingButtonClick);
        closeCreditButton.onClick.AddListener(closeCreditButtonClick);
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

    public void creditButtonClick()
    {
        creditPanel.gameObject.SetActive(true);
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

    public void closeCreditButtonClick()
    {
        creditPanel.gameObject.SetActive(false);
        menuUI.gameObject.SetActive(true);
    }
}