using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanelControl : MonoBehaviourPunCallbacks
{
    public Button gameStartButton;
    public Button readyButton;
    public GameObject playerPrefab;
    public RectTransform playerList;

    void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            UpdatePlayerList();
        }
    }

    public override void OnJoinedRoom()
    {
        EnterLobby();
        UpdatePlayerList();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        AddPlayerList(newPlayer);
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

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        EnterLobby();
    }

    private void UpdatePlayerList()
    {
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
        {
            return;
        }

        foreach (Transform child in playerList)
        {
            Destroy(child.gameObject);
        }

        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            AddPlayerList(player);
        }
    }

    private void AddPlayerList(Player player)
    {
        GameObject players = Instantiate(playerPrefab, playerList);
        PlayerPanelManager playerPanel = players.GetComponent<PlayerPanelManager>();

        playerPanel.SetNickname(player.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            playerPanel.SetCrown();
        }

        if (player.CustomProperties.TryGetValue("isReady", out object isReady))
        {
            playerPanel.SetReadyCheck((bool)isReady);
        }
    }
}