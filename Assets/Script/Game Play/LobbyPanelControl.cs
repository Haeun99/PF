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

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        UpdatePlayerList();
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
        GameObject playerCard = PhotonNetwork.Instantiate(playerPrefab.name, playerList.position, Quaternion.identity);
        playerCard.transform.SetParent(playerList, false);

        PlayerPanelManager playerPanel = playerCard.GetComponent<PlayerPanelManager>();
        if (playerPanel != null)
        {
            playerPanel.Initialize(player);
        }
    }
}