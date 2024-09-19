using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyControl : MonoBehaviourPunCallbacks
{
    public RectTransform playerList;
    public GameObject playerPrefab;

    public Button startButton;
    public Button readyButton;

    private Dictionary<int, bool> playersReady = new();

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
        PhotonNetwork.AutomaticallySyncScene = true;

        Hashtable customProps = new Hashtable
        {
            { "IsReady", false }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(customProps);

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

        if (PhotonNetwork.IsMasterClient && !newPlayer.CustomProperties.ContainsKey("IsReady"))
        {
            playersReady[newPlayer.ActorNumber] = false;
            CheckReady();
        }

        SortPlayers();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        if (playersReady.ContainsKey(otherPlayer.ActorNumber))
        {
            playersReady.Remove(otherPlayer.ActorNumber);
        }

        UpdatePlayerList();
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

    public void SetPlayerReady(int actNumber, bool isReady)
    {
        if (playerEntries.TryGetValue(actNumber, out PlayerPanelManager playerPanel))
        {
            playerPanel.SetReadyCheck(isReady);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            playersReady[actNumber] = isReady;
            CheckReady();
        }
    }

    private void CheckReady()
    {
        bool allReady = playersReady.Values.All(x => x);

        GameStartButton[] gameStartButtons = FindObjectsOfType<GameStartButton>();
        foreach (var gameStartButton in gameStartButtons)
        {
            gameStartButton.CheckAllPlayersReady();
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("IsReady"))
        {
            bool isReady = (bool)changedProps["IsReady"];
            SetPlayerReady(targetPlayer.ActorNumber, isReady);

            Debug.Log($"Player {targetPlayer.ActorNumber} properties updated: IsReady = {isReady}");
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            CheckReady();
        }

        EnterLobby();
        SortPlayers();
    }
}