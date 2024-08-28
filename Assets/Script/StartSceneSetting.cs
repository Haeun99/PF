using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneSetting : MonoBehaviour
{
    public Button startButton;
    public Button settingButton;
    public Button backButton;
    public RectTransform background;

    private void Start()
    {
        startButton.onClick.AddListener(startButtonClick);
        settingButton.onClick.AddListener(settingButtonClick);
        backButton.onClick.AddListener(backButtonClick);
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
}
