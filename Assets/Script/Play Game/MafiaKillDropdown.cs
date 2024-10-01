using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Linq;

public class MafiaKillDropdown : MonoBehaviourPunCallbacks
{
    public static MafiaKillDropdown Instance { get; private set; }

    public TMP_Dropdown playerDropdown;
    public Button selectButton;

    private int voteEnd;

    private int nightTime;

    public List<Player> players = new List<Player>();

    public Dictionary<Player, Player> mafiaVotes = new Dictionary<Player, Player>();

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

        if (localPlayer.CustomProperties.ContainsKey("Job") && localPlayer.CustomProperties["Job"].Equals("마피아"))
        {
            Player selectedPlayer = GetSelectedPlayer();

            if (selectedPlayer != null)
            {
                mafiaVotes[localPlayer] = selectedPlayer;
            }

            Hashtable mafiaAction = new Hashtable
            {
                { "nightAction", "Mafia" },
                { "MafiaSelectedPlayer", selectedPlayer.NickName }
            };

            PhotonNetwork.LocalPlayer.SetCustomProperties(mafiaAction);

            string message = $"[시스템]{localPlayer.NickName}님이 살해 대상으로 <color=green>{selectedPlayer.NickName}<color=white>님을 선택했습니다.";

            MafiaTeamChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_MafiaTeam", message);

            selectButton.gameObject.SetActive(false);

            if (nightTime == 0)
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
        string cureTarget = (string)PhotonNetwork.LocalPlayer.CustomProperties["DoctorSelectedPlayer"];

        if (killTarget != null)
        {
            Debug.Log($"Cure Target: {cureTarget}, Kill Target: {killTarget.NickName}");

            if (!string.IsNullOrEmpty(cureTarget) && killTarget.NickName == cureTarget)
            {
                PlayerStatus.Instance.SetDead(killTarget, false);

                InGameChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_InGame", $"[시스템]<color=yellow>지난 밤, 마피아에게 죽을 뻔한 사람을 의사가 살렸습니다!");
            }

            else
            {
                bool allVotesMatch = mafiaVotes.Values.All(player => player == killTarget);

                if (allVotesMatch)
                {
                    MafiaAction(killTarget);
                }
                else
                {
                    InGameChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_InGame", "[시스템]지난 밤은 아무 일도 일어나지 않았습니다.");
                }
            }
        }

        else
        {
            InGameChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_InGame", "[시스템]지난 밤은 아무 일도 일어나지 않았습니다.");
        }
    }

    public Player CheckVotes()
    {
        Player lastSelectedPlayer = null;
        bool allVotesMatch = true;
        bool hasVotes = false;

        foreach (Player Mafia in PhotonNetwork.PlayerList)
        {
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
        PlayerStatus.Instance.SetDead(targetPlayer, true);

        InGameChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_InGame", $"[시스템]지난 밤, 마피아에 의해 <color=red>{targetPlayer.NickName}<color=white>님이 살해당했습니다.");
    }
}