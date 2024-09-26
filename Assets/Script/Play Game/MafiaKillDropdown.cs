using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MafiaKillDropdown : MonoBehaviourPunCallbacks
{
    public static MafiaKillDropdown Instance { get; private set; }

    public TMP_Dropdown playerDropdown;
    public Button selectButton;

    private int voteEnd;

    public List<Player> players = new List<Player>();

    private Dictionary<Player, Player> mafiaVotes = new Dictionary<Player, Player>();

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

        if (localPlayer.CustomProperties.ContainsKey("Job") && localPlayer.CustomProperties["Job"].Equals("마피아"))
        {
            Player selectedPlayer = GetSelectedPlayer();

            mafiaVotes[localPlayer] = selectedPlayer;

            Hashtable mafiaAction = new Hashtable
            {
                { "nightAction", "Mafia" },
                { "selectedPlayer", selectedPlayer }
            };

            string message = $"[시스템]{localPlayer.NickName}님이 살해 대상으로 <color=green>{selectedPlayer.NickName}<color=white>님을 선택했습니다.";

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
        Player killTarget = CheckVotes();
        Player cureTarget = DoctorCureDropdown.Instance.CheckVotes();

        if (killTarget != null)
        {
            if (cureTarget != null && killTarget == cureTarget)
            {
                InGameChatting.Instance.DisplaySystemMessage($"[시스템]<color=yellow>지난 밤, 마피아에게 죽을 뻔한 사람을 의사가 살렸습니다!");
            }
            else
            {
                MafiaAction(killTarget);
            }
        }
        else
        {
            InGameChatting.Instance.DisplaySystemMessage("[시스템]지난 밤은 아무 일도 일어나지 않았습니다.");
        }
    }

    public Player CheckVotes()
    {
        Player lastSelectedPlayer = null;
        bool allVotesMatch = true;
        bool hasVotes = false;

        foreach (Player Mafia in PhotonNetwork.PlayerList)
        {
            if ((bool)Mafia.CustomProperties["isDead"])
            {
                continue;
            }

            if (!mafiaVotes.ContainsKey(Mafia))
            {
                continue;
            }

            hasVotes = true;

            if (lastSelectedPlayer == null)
            {
                lastSelectedPlayer = mafiaVotes[Mafia];
            }
            else
            {
                if (lastSelectedPlayer != mafiaVotes[Mafia])
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

    public void MafiaAction(Player targetPlayer)
    {
        PlayerStatus.Instance.SetDead(true);

        InGameChatting.Instance.DisplaySystemMessage($"[시스템]지난 밤, 마피아에 의해 {targetPlayer.NickName}님이 살해당했습니다.");
    }
}