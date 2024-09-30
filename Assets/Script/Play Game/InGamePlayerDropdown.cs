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

    private int voteEnd;

    public TMP_InputField[] chatting;

    public List<Player> players = new List<Player>();
    private bool isFinalAppeal;

    public bool AllVote
    {
        get
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (!player.CustomProperties.ContainsKey("votedPlayer"))
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

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("NightTime"))
        {
            voteEnd = (int)PhotonNetwork.CurrentRoom.CustomProperties["NightTime"];
        }

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

    public virtual void PlayerVote()
    {
        Player selectedPlayer = GetSelectedPlayer();

        Hashtable props = new Hashtable
        {
            { "votedPlayer", selectedPlayer }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

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

        if (Time.time >= voteEnd)
        {
            selectButton.gameObject.SetActive(false);
        }
    }

    public void CalculateVoteResults()
    {
        Dictionary<Player, int> voteCount = new Dictionary<Player, int>();

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.ContainsKey("votedPlayer"))
            {
                Player votedPlayer = (Player)player.CustomProperties["votedPlayer"];

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
        int secondMaxVotes = 0;

        foreach (var entry in voteCount)
        {
            if (entry.Value > maxVotes)
            {
                secondMaxVotes = maxVotes;
                mostVotedPlayer = entry.Key;
                maxVotes = entry.Value;
            }
            else if (entry.Value > secondMaxVotes && entry.Value < maxVotes)
            {
                secondMaxVotes = entry.Value;
            }
        }


        if (mostVotedPlayer != null)
        {
            VoteManager.Instance.SetMostVotedPlayer(mostVotedPlayer);
        }

        if (maxVotes > secondMaxVotes)
        {
            bool isFinalAppeal = (bool)PhotonNetwork.CurrentRoom.CustomProperties["FinalAppeal"];

            if (isFinalAppeal == false)
            {
                PlayerStatus.Instance.SetDead(true);

                InGameChatting.Instance.DisplaySystemMessage($"[시스템]{mostVotedPlayer.NickName}님이 처형 당했습니다.");
            }

            else
            {
                ManageChatInputFields(mostVotedPlayer);
            }
        }

        else
        {
            InGameChatting.Instance.DisplaySystemMessage("[시스템]투표 결과가 동점입니다. 처형이 진행되지 않습니다.");
        }
    }

    public void ManageChatInputFields(Player mostVotedPlayer)
    {
        foreach (var inputField in chatting)
        {
            inputField.interactable = false;
        }

        if (mostVotedPlayer != null)
        {
            for (int i = 0; i < chatting.Length; i++)
            {
                string inputFieldPlayerName = chatting[i].gameObject.GetComponent<Player>().NickName;

                if (inputFieldPlayerName == mostVotedPlayer.NickName)
                {
                    chatting[i].interactable = true;
                }
            }
        }
    }
}