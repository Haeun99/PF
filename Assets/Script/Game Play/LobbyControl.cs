using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using ExitGames.Client.Photon;

using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;

public class LobbyControl : MonoBehaviourPunCallbacks
{
    public RectTransform playerList;
    public GameObject playerPrefab;

    public Button startButton;
    public Button readyButton;

    private Dictionary<int, bool> playersReady;
    public Dictionary<int, PlayerPanelManager> playerEntries = new();

    private void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            UpdatePlayerList();
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();

        foreach (Transform child in playerList.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public override void OnJoinedRoom()
    {
        var props = PhotonNetwork.CurrentRoom.CustomProperties;

        if (PhotonNetwork.IsMasterClient)
        {
            playersReady = new Dictionary<int, bool>();
        }

        else
        {

        }

        PhotonNetwork.AutomaticallySyncScene = true;

        EnterLobby();
        SortPlayers();
        UpdatePlayerList();
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
            JoinPlayer(player);

            if (player.CustomProperties.ContainsKey("IsReady"))
            {
                SetPlayerReady(player.ActorNumber, (bool)player.CustomProperties["IsReady"]);
            }
        }
    }

    public void JoinPlayer(Player newPlayer)
    {
        var playerCard = Instantiate(playerPrefab, playerList, false).GetComponent<PlayerPanelManager>();

        playerCard.playerNickname.text = newPlayer.NickName;
        playerCard.player = newPlayer;

        PlayerPanelManager playerPanel = playerCard.GetComponent<PlayerPanelManager>();
        if (playerPanel != null)
        {
            playerPanel.Initialize(newPlayer);
        }

        playerEntries[newPlayer.ActorNumber] = playerPanel;

        playerPanel.masterCrown.gameObject.SetActive(newPlayer.IsMasterClient);
        playerPanel.readyCheck.gameObject.SetActive(!newPlayer.IsMasterClient);

        if (PhotonNetwork.IsMasterClient)
        {
            playersReady[newPlayer.ActorNumber] = false;
            CheckReady();
        }

        SortPlayers();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        if (otherPlayer == null || !playerEntries.ContainsKey(otherPlayer.ActorNumber))
        {
            return;
        }

        if (playerEntries.ContainsKey(otherPlayer.ActorNumber))
        {
            Destroy(playerEntries[otherPlayer.ActorNumber].gameObject);
            playerEntries.Remove(otherPlayer.ActorNumber);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            playersReady.Remove(otherPlayer.ActorNumber);
            CheckReady();
        }

        UpdatePlayerList();
        SortPlayers();
        PhotonNetwork.JoinLobby();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        UpdatePlayerList();
    }

    public void SortPlayers()
    {
        var sortedPlayers = playerEntries.Values
        .Where(p => p != null && p.gameObject != null)
        .OrderBy(p => p.player.ActorNumber)
        .ToList();

        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            sortedPlayers[i].transform.SetSiblingIndex(i);
        }
    }

    private void EnterLobby()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.gameObject.SetActive(true);
            readyButton.gameObject.SetActive(false);
        }
        else
        {
            readyButton.gameObject.SetActive(true);
            startButton.gameObject.SetActive(false);
        }
    }

    public void ReadyToggleClick(bool isOn)
    {
        Player localPlayer = PhotonNetwork.LocalPlayer;

        Hashtable customProps = new Hashtable
    {
        { "IsReady", isOn }
    };

        localPlayer.SetCustomProperties(customProps);
    }

    public void SetPlayerReady(int actNumber, bool isReady)
    {
        playerEntries[actNumber].readyCheck.gameObject.SetActive(isReady);

        if (PhotonNetwork.IsMasterClient)
        {
            playersReady[actNumber] = isReady;
            CheckReady();
        }
    }

    private void CheckReady()
    {
        bool allReady = playersReady.Values.All(x => x);
        bool anyReady = playersReady.Values.Any(x => x);

        startButton.interactable = allReady;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("IsReady"))
        {
            SetPlayerReady(targetPlayer.ActorNumber, (bool)changedProps["IsReady"]);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playersReady = new Dictionary<int, bool>();

            foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                playersReady[player.ActorNumber] = false;
            }

            CheckReady();
        }

        EnterLobby();
        UpdatePlayerList();
    }
}