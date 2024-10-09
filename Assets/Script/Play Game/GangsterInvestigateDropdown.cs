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

    private int nightTime;

    public List<Player> players = new List<Player>();

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        UpdatePlayerList();
        selectButton.onClick.AddListener(PlayerVote);

        Hashtable roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
        nightTime = (int)roomProperties["NightTime"];
    }

    private void Update()
    {
        if (nightTime == 0)
        {
            selectButton.gameObject.SetActive(false);
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
            if (player == PhotonNetwork.LocalPlayer)
                continue;

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

            Hashtable gangsterAction = new Hashtable
            {
                { "nightAction", "Gangster" },
                { "GangsterSelectedPlayer", selectedPlayer.NickName }
            };

            PhotonNetwork.LocalPlayer.SetCustomProperties(gangsterAction);

            string message = $"[시스템]{PhotonNetwork.LocalPlayer.NickName}님이 <color=green>{selectedPlayer.NickName}<color=white>님을 조사합니다...";

            MafiaTeamChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_MafiaTeam", message);

            selectButton.gameObject.SetActive(false);
        }

        else
        {
            return;
        }
    }

    public IEnumerator OnNightTimeEnd()
    {
        Player selectedTarget = CheckVotes();
        if (selectedTarget != null)
        {
            GangsterAction(selectedTarget);
        }
        else
        {
            yield return null;
        }
    }

    public Player CheckVotes()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("GangsterSelectedPlayer"))
        {
            string selectedPlayerName = (string)PhotonNetwork.LocalPlayer.CustomProperties["GangsterSelectedPlayer"];

            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (player.NickName == selectedPlayerName &&
                    (!player.CustomProperties.ContainsKey("isDead") || !(bool)player.CustomProperties["isDead"]))
                {
                    return player;
                }
            }
        }

        return null;
    }

    public void GangsterAction(Player targetPlayer)
    {
        string job = StartGame.Instance.GetPlayerJob(targetPlayer);

        string message = $"[시스템]<color=green>{targetPlayer.NickName}<color=white>님은 <color=yellow>{job}<color=white>입니다!";

        MafiaTeamChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_MafiaTeam", message);
    }
}