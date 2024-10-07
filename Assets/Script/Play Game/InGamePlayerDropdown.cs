using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Chat;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Linq;

public class InGamePlayerDropdown : MonoBehaviourPunCallbacks
{
    public static InGamePlayerDropdown Instance { get; private set; }

    public TMP_Dropdown playerDropdown;
    public Button selectButton;

    public TMP_InputField[] chatting;

    public List<Player> players = new List<Player>();
    private bool isFinalAppeal;

    public bool AllVote
    {
        get
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if ((!player.CustomProperties.ContainsKey("isDead") || !(bool)player.CustomProperties["isDead"]) && !player.CustomProperties.ContainsKey("votedPlayer") || player.CustomProperties["votedPlayer"] == null)
                {
                    return false;
                }
            }

            return true;
        }
    }

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
        isFinalAppeal = (bool)roomProperties["FinalAppeal"];
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        UpdatePlayerList();
    }

    public virtual void UpdatePlayerList()
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
        Player selectedPlayer = GetSelectedPlayer();

        Hashtable props = new Hashtable
        {
            { "votedPlayer", selectedPlayer.ActorNumber }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        Hashtable currentRoomProps = PhotonNetwork.CurrentRoom.CustomProperties;

        if (currentRoomProps.ContainsKey("VoteDictionary"))
        {
            Hashtable voteDictionary = (Hashtable)currentRoomProps["VoteDictionary"];

            voteDictionary[PhotonNetwork.LocalPlayer.ActorNumber] = selectedPlayer.ActorNumber;

            currentRoomProps["VoteDictionary"] = voteDictionary;
            PhotonNetwork.CurrentRoom.SetCustomProperties(currentRoomProps);
        }

        string message;

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("AnonymousVote"))
        {
            bool isAnonymousVote = (bool)PhotonNetwork.CurrentRoom.CustomProperties["AnonymousVote"];

            if (isAnonymousVote == false)
            {
                message = $"[시스템]{PhotonNetwork.LocalPlayer.NickName}님이 <color=yellow>{selectedPlayer.NickName}</color>님에게 투표했습니다.";

                InGameChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_InGame", message);
            }

            else
            {
                message = $"[시스템]{PhotonNetwork.LocalPlayer.NickName}님이 투표했습니다.";

                InGameChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_InGame", message);
            }
        }

        selectButton.gameObject.SetActive(false);
    }

    public void CalculateVoteResults()
    {
        Dictionary<Player, int> voteCount = new Dictionary<Player, int>();

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.ContainsKey("votedPlayer"))
            {
                int votedPlayerActorNumber = (int)player.CustomProperties["votedPlayer"];
                Player votedPlayer = PhotonNetwork.PlayerList.FirstOrDefault(p => p.ActorNumber == votedPlayerActorNumber);

                if (votedPlayer != null)
                {
                    if (!voteCount.ContainsKey(votedPlayer))
                    {
                        voteCount[votedPlayer] = 0;
                    }
                    voteCount[votedPlayer]++;
                }
            }
        }

        Player mostVotedPlayer = null;
        int maxVotes = 0;
        List<Player> tiedPlayers = new List<Player>();

        foreach (var entry in voteCount)
        {
            if (entry.Value > maxVotes)
            {
                mostVotedPlayer = entry.Key;
                maxVotes = entry.Value;
                tiedPlayers.Clear();
            }
            else if (entry.Value == maxVotes)
            {
                tiedPlayers.Add(entry.Key);
            }
        }

        if (tiedPlayers.Count == 0 && mostVotedPlayer != null)
        {
            VoteManager.Instance.SetMostVotedPlayer(mostVotedPlayer);

            bool isFinalAppeal = (bool)PhotonNetwork.CurrentRoom.CustomProperties["FinalAppeal"];
            if (!isFinalAppeal)
            {
                PlayerStatus.Instance.SetDead(mostVotedPlayer, true);
                InGameChatting.Instance.DisplaySystemMessage($"[시스템]<color=red>{mostVotedPlayer.NickName}</color>님이 처형 당했습니다.");
            }
            else
            {
                InGameChatting.Instance.DisplaySystemMessage($"[시스템]최다 득표자 <color=red>{mostVotedPlayer.NickName}</color>님의 최후 변론을 시작합니다.");
            }
        }

        else
        {
            InGameChatting.Instance.DisplaySystemMessage("[시스템]투표 결과가 동점입니다. 처형이 진행되지 않습니다.");
        }
    }

    public void InitVoteDictionary()
    {
        Hashtable voteDictionary = new Hashtable();

        Hashtable props = new Hashtable
        {
            { "VoteDictionary", voteDictionary }
        };

        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }
}