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

        if (localPlayer.CustomProperties.ContainsKey("Job") && localPlayer.CustomProperties["Job"].Equals("���Ǿ�"))
        {
            Player selectedPlayer = GetSelectedPlayer();

            mafiaVotes[localPlayer] = selectedPlayer;

            Hashtable mafiaAction = new Hashtable
            {
                { "nightAction", "Mafia" },
                { "selectedPlayer", selectedPlayer }
            };

            string message = $"[�ý���]{localPlayer.NickName}���� ���� ������� <color=green>{selectedPlayer.NickName}<color=white>���� �����߽��ϴ�.";

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
                InGameChatting.Instance.DisplaySystemMessage($"[�ý���]<color=yellow>���� ��, ���Ǿƿ��� ���� ���� ����� �ǻ簡 ��Ƚ��ϴ�!");
            }
            else
            {
                MafiaAction(killTarget);
            }
        }
        else
        {
            InGameChatting.Instance.DisplaySystemMessage("[�ý���]���� ���� �ƹ� �ϵ� �Ͼ�� �ʾҽ��ϴ�.");
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

        InGameChatting.Instance.DisplaySystemMessage($"[�ý���]���� ��, ���Ǿƿ� ���� {targetPlayer.NickName}���� ���ش��߽��ϴ�.");
    }
}