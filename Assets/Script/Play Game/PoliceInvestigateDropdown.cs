using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PoliceInvestigateDropdown : MonoBehaviourPunCallbacks
{
    public static PoliceInvestigateDropdown Instance { get; private set; }

    public TMP_Dropdown playerDropdown;
    public Button selectButton;

    private int voteEnd;

    public List<Player> players = new List<Player>();

    private Dictionary<Player, Player> policeVotes = new Dictionary<Player, Player>();

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

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("NightTime"))
        {
            voteEnd = (int)PhotonNetwork.CurrentRoom.CustomProperties["NightTime"];
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

        if (localPlayer.CustomProperties.ContainsKey("Job") && localPlayer.CustomProperties["Job"].Equals("경찰"))
        {
            Player selectedPlayer = GetSelectedPlayer();

            if (selectedPlayer != null)
            {
                policeVotes[localPlayer] = selectedPlayer;
            }

            Hashtable policeAction = new Hashtable
            {
                { "nightAction", "Police" },
                { "selectedPlayer", selectedPlayer }
            };

            string message = $"[시스템]{PhotonNetwork.LocalPlayer.NickName}님이 <color=green>{selectedPlayer.NickName}<color=white>님을 조사합니다...";

            PoliceChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_Police", message);
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
            PoliceAction(selectedTarget);
        }
        else
        {
            return;
        }
    }

    public Player CheckVotes()
    {
        Player lastSelectedPlayer = null;
        bool allVotesMatch = true;
        bool hasVotes = false;

        foreach (Player Police in PhotonNetwork.PlayerList)
        {
            if (!policeVotes.ContainsKey(Police))
            {
                continue;
            }

            hasVotes = true;

            if (lastSelectedPlayer == null)
            {
                lastSelectedPlayer = policeVotes[Police];
            }
            else
            {
                if (lastSelectedPlayer != policeVotes[Police])
                {
                    allVotesMatch = false;
                    break;
                }
            }
        }

        if (!hasVotes || lastSelectedPlayer == null)
        {
            return null;
        }

        return allVotesMatch ? lastSelectedPlayer : null;
    }

    public void PoliceAction(Player targetPlayer)
    {
        string job = StartGame.Instance.GetPlayerJob(targetPlayer);

        if (job == "마피아" || job == "건달")
        {
            string message = ($"[시스템]<color=green>{targetPlayer.NickName}<color=white>님은 <color=red>마피아<color=white>입니다!");

            PoliceChatting.Instance.DisplaySystemMessage(message);
        }

        else
        {
            string message = ($"[시스템]<color=green>{targetPlayer.NickName}<color=white>님은 마피아가 아닙니다.");

            PoliceChatting.Instance.DisplaySystemMessage(message);
        }
    }
}