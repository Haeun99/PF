using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameStartButton : MonoBehaviourPunCallbacks
{
    public Button gameStartButton;
    public GameObject gamePanel;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        gameStartButton.onClick.AddListener(GameStart);

        if (PhotonNetwork.IsMasterClient)
        {
            gameStartButton.interactable = false;
            CheckAllPlayersReady();
        }
    }

    public void CheckAllPlayersReady()
    {
        bool allReady = true;
        int readyPlayers = 0;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.ContainsKey("IsReady") || !(bool)player.CustomProperties["IsReady"])
            {
                allReady = false;
            }
            else
            {
                readyPlayers++;
            }
        }

        gameStartButton.interactable = allReady && PhotonNetwork.CurrentRoom.PlayerCount >= 4;
    }

    private void GameStart()
    {
        Hashtable roomProperties = new Hashtable
        {
            { "GameStarted", true }
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);

        gamePanel.SetActive(true);
        StartGame.Instance.StartButtonClick();
        ResetPlayersReadyState();
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);

        if (propertiesThatChanged.ContainsKey("GameStarted"))
        {
            bool gameStarted = (bool)propertiesThatChanged["GameStarted"];
            
            if (gameStarted)
            {
                gamePanel.SetActive(true);
                StartGame.Instance.StartButtonClick();
                ResetPlayersReadyState();
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        CheckAllPlayersReady();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        CheckAllPlayersReady();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("IsReady"))
        {
            CheckAllPlayersReady();
        }
    }

    private void ResetPlayersReadyState()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            player.SetCustomProperties(new Hashtable { { "IsReady", false } });
            ReadyButton.Instance.ResetReadyState();
        }
    }
}