using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanelControl : MonoBehaviourPunCallbacks
{
    public Button gameStartButton;
    public Button readyButton;
    public override void OnJoinedRoom()
    {
        EnterLobby();
    }

    private void EnterLobby()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            gameStartButton.gameObject.SetActive(true);
            readyButton.gameObject.SetActive(false);
        }
        else
        {
            readyButton.gameObject.SetActive(true);
            gameStartButton.gameObject.SetActive(false);
        }
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        EnterLobby();
    }
}