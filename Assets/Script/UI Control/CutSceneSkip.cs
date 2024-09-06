using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutSceneSkip : MonoBehaviour
{
    public RectTransform skipPopup;
    public Button skipButton;
    public Button noButton;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            skipPopup.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        skipButton.onClick.AddListener(() => SceneManager.LoadScene("Game_Scene"));
        noButton.onClick.AddListener(KeepPlayingCutScene);
    }

    private void KeepPlayingCutScene()
    {
        skipPopup.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}