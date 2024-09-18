using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class GameStartButton : MonoBehaviourPunCallbacks
{
    public Button gameStartButton;
    private bool allReady = true;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            gameStartButton.interactable = false;
            gameStartButton.onClick.AddListener(GameStart);
        }

        CheckAllPlayersReady();
        MinPlayer();
    }

    [PunRPC]
    public void CheckAllPlayersReady()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.ContainsKey("IsReady") || !(bool)player.CustomProperties["IsReady"])
            {
                allReady = false;
                break;
            }
        }
    }

    public void MinPlayer()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount < 5)
        {
            gameStartButton.interactable = false;
        }

        else
        {
            gameStartButton.interactable = true;
        }
    }

    private void GameStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartGame.Instance.StartGameClick();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        CheckAllPlayersReady();
        MinPlayer();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        CheckAllPlayersReady();
        MinPlayer();
    }
}
