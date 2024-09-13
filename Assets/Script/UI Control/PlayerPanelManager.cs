using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanelManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI playerNickname;
    public Image masterCrown;
    public Image readyCheck;

    public Player player;

    private void Start()
    {
        readyCheck.gameObject.SetActive(false);
    }

    public void SetNickname(string nickname)
    {
        playerNickname.text = nickname;
    }

    public void SetReadyCheck(bool isReady)
    {
        readyCheck.gameObject.SetActive(isReady);
    }

    public void Initialize(Player player)
    {
        SetNickname(player.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            masterCrown.gameObject.SetActive(true);
        }

        else
        {
            masterCrown.gameObject.SetActive(false);
        }

        if (player.CustomProperties.TryGetValue("isReady", out object isReady))
        {
            SetReadyCheck((bool)isReady);
        }
    }
}