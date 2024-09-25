using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class FinalAppealSystem : MonoBehaviour
{
    public static FinalAppealSystem Instance { get; private set; }

    public Button finalKillButton;
    public Button finalSaveButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        finalKillButton.onClick.AddListener(finalKillButtonClick);
        finalSaveButton.onClick.AddListener(finalSaveButtonClick);
    }

    public void finalKillButtonClick()
    {

    }

    public void finalSaveButtonClick()
    {

    }
}