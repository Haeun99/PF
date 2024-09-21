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
        if (PhotonNetwork.IsMasterClient)
        {
            gameStartButton.interactable = false;
            gameStartButton.onClick.AddListener(GameStart);
        }

        CheckAllPlayersReady();
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
        // 게임 시작을 위한 룸 프로퍼티 설정 (모든 클라이언트에 전파됨)
        Hashtable roomProperties = new Hashtable
    {
        { "GameStarted", true }
    };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);

        Debug.Log("GameStarted property set.");

        // 플레이어 상태 초기화
        ResetPlayersReadyState();
    }

    public override void OnRoomPropertiesUpdate(Hashtable changedProps)
    {
        base.OnRoomPropertiesUpdate(changedProps);

        Debug.Log("OnRoomPropertiesUpdate called!");

        // "GameStarted" 프로퍼티가 변경되었을 때 처리
        if (changedProps.ContainsKey("GameStarted") && (bool)changedProps["GameStarted"])
        {
            Debug.Log("Game started property detected.");
            gamePanel.SetActive(true);
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
        }
    }
}