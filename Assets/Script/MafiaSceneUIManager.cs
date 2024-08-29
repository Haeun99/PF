using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MafiaSceneUIManager : MonoBehaviour
{
    public Button backToVillageButton;
    public Button chatButton;

    public RectTransform chatBackground;

    private bool isOpen = false;

    private void Start()
    {
        backToVillageButton.onClick.AddListener(BTVButtonClick);
        chatButton.onClick.AddListener(chatButtonClick);
    }

    public void BTVButtonClick()
    {
        SceneManager.LoadScene("Game_Scene");
    }

    public void chatButtonClick()
    {
        isOpen = !isOpen;
        chatBackground.gameObject.SetActive(isOpen);
    }
}
