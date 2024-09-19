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

    public void SetNickname(string nickname)
    {
        playerNickname.text = nickname;
    }

    public void SetReadyCheck(bool isReady)
    {
        if (!player.IsMasterClient)
        {
            readyCheck.gameObject.SetActive(isReady);
        }
        else
        {
            readyCheck.gameObject.SetActive(false);
        }
    }

    public void Initialize(Player player)
    {
        this.player = player;
        SetNickname(player.NickName);

        masterCrown.gameObject.SetActive(player.IsMasterClient);

        if (player.IsMasterClient)
        {
            readyCheck.gameObject.SetActive(false);
        }
        else
        {
            readyCheck.gameObject.SetActive(false);
        }
    }
}