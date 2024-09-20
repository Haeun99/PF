using System.Collections;
using System.Collections.Generic;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;


using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ReadyButton : MonoBehaviourPunCallbacks
{
    private bool isReady = false;
    private Button readyButton;
    private Image buttonImage;
    private TextMeshProUGUI buttonText;

    private void Start()
    {
        readyButton = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();

        readyButton.onClick.AddListener(ReadyState);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        isReady = false;

        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "IsReady", isReady } });

        buttonImage.color = Color.white;
        buttonText.text = "준비하기";
    }

    private void ReadyState()
    {
        isReady = !isReady;

        buttonImage.color = isReady ? Color.gray : Color.white;
        buttonText.text = isReady ? "준비 취소" : "준비하기";

        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "IsReady", isReady } });
    }
}