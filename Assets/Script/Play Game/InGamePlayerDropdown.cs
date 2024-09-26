using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Chat;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class InGamePlayerDropdown : MonoBehaviourPunCallbacks
{
    public TMP_Dropdown playerDropdown;
    public Button selectButton;

    protected List<Player> players = new List<Player>();

    public virtual void Start()
    {
        UpdatePlayerList();
        selectButton.onClick.AddListener(PlayerVote);
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
                message = $"[시스템]{PhotonNetwork.LocalPlayer.NickName}님이 <color=green>{selectedPlayer.NickName}</color>님에게 투표했습니다.";

                InGameChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_InGame", message);
            }

            else
            {
                message = $"[시스템]{PhotonNetwork.LocalPlayer.NickName}님이 투표했습니다.";

                InGameChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_InGame", message);
            }
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

        foreach (var entry in voteCount)
        {
            if (entry.Value > maxVotes)
            {
                mostVotedPlayer = entry.Key;
                maxVotes = entry.Value;
            }
        }

        if (mostVotedPlayer != null)
        {
            // final appeal coroutine time에 말하기 활성화

            Hashtable props = new Hashtable
            {
                { "isDead", true }
            };
            mostVotedPlayer.SetCustomProperties(props);
        }
    }
}