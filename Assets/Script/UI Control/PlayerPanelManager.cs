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

    public void SetNickname(string nickname)
    {
        playerNickname.text = nickname;
    }

    public void SetCrown()
    {
        masterCrown.gameObject.SetActive(true);
    }

    public void SetReadyCheck(bool isReady)
    {
        readyCheck.gameObject.SetActive(isReady);

        readyCheck.color = isReady ? Color.green : Color.white;
    }
}
