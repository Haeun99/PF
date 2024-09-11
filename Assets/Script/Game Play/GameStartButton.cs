using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class GameStartButton : MonoBehaviourPunCallbacks
{
    private Button gameStartButton;

    private void Start()
    {
        gameStartButton = GetComponent<Button>();

        if (PhotonNetwork.IsMasterClient)
        {
            gameStartButton.interactable = false;
            gameStartButton.onClick.AddListener(StartGame);
        }

        CheckAllPlayersReady();
        MinPlayer();
    }

    [PunRPC]
    public void CheckAllPlayersReady()
    {
        bool allReady = true;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.ContainsKey("IsReady") || !(bool)player.CustomProperties["IsReady"])
            {
                allReady = false;
                break;
            }
        }

        if (allReady)
        {
            gameStartButton.interactable = true;
        }
        else
        {
            gameStartButton.interactable = false;
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

    private void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // 게임 시작
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
