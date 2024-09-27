using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GangsterInvestigateDropdown : MonoBehaviourPunCallbacks
{
    public static GangsterInvestigateDropdown Instance { get; private set; }

    public TMP_Dropdown playerDropdown;
    public Button selectButton;

    private int voteEnd;

    public List<Player> players = new List<Player>();

    private Dictionary<Player, Player> gangsterVote = new Dictionary<Player, Player>();

    public void Start()
    {
        UpdatePlayerList();
        selectButton.onClick.AddListener(PlayerVote);

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("NightTime"))
        {
            voteEnd = (int)PhotonNetwork.CurrentRoom.CustomProperties["NightTime"];
        }
    }

    public virtual void Update()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        UpdatePlayerList();
    }

    public void UpdatePlayerList()
    {
        playerDropdown.ClearOptions();
        players.Clear();

        List<string> playerNames = new List<string>();

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.ContainsKey("isDead") || !(bool)player.CustomProperties["isDead"])
            {
                playerNames.Add(player.NickName);
                players.Add(player);
            }
        }

        playerDropdown.AddOptions(playerNames);
    }

    public Player GetSelectedPlayer()
    {
        int selectedIndex = playerDropdown.value;

        if (selectedIndex < 0 || selectedIndex >= players.Count)
        {
            return null;
        }

        return players[selectedIndex];
    }

    public void PlayerVote()
    {
        Player localPlayer = PhotonNetwork.LocalPlayer;

        if (localPlayer.CustomProperties.ContainsKey("Job") && localPlayer.CustomProperties["Job"].Equals("건달"))
        {
            Player selectedPlayer = GetSelectedPlayer();

            gangsterVote[localPlayer] = selectedPlayer;

            Hashtable gangsterAction = new Hashtable
            {
                { "nightAction", "Gangster" },
                { "selectedPlayer", selectedPlayer }
            };

            string message = $"[시스템]{PhotonNetwork.LocalPlayer.NickName}님이 <color=green>{selectedPlayer.NickName}<color=white>님을 조사합니다...";

            MafiaTeamChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_MafiaTeam", message);

            if (Time.time >= voteEnd)
            {
                selectButton.gameObject.SetActive(false);
            }
        }

        else
        {
            return;
        }
    }

    public void OnNightTimeEnd()
    {
        Player selectedTarget = CheckVotes();
        if (selectedTarget != null)
        {
            GangsterAction(selectedTarget);
        }
        else
        {
            return;
        }
    }

    public Player CheckVotes()
    {
        Player selectedPlayer = GetSelectedPlayer();

        if (selectedPlayer != null && !((bool)selectedPlayer.CustomProperties["isDead"]))
        {
            return selectedPlayer;
        }

        return null;
    }

    public void GangsterAction(Player targetPlayer)
    {
        string job = StartGame.Instance.GetPlayerJob(targetPlayer);

        string message = $"[시스템]<color=green>{targetPlayer.NickName}<color=white>님은 <color=blue>{job}<color=white>입니다!";

        MafiaTeamChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_MafiaTeam", message);
    }
}