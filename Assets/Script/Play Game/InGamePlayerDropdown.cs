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
    private ChatClient chatClient;

    public TMP_Dropdown playerDropdown;
    public Button selectButton;

    protected List<Player> players = new List<Player>();

    private void Start()
    {
        chatClient = InGameChatting.Instance.chatClient;

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
        return players[selectedIndex];
    }

    public virtual void PlayerVote()
    {
        Player selectedPlayer = GetSelectedPlayer();

        selectButton.interactable = false;

        string message;
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("AnonymousVote") && (bool)PhotonNetwork.CurrentRoom.CustomProperties["AnonymousVote"])
        {
            message = $"{PhotonNetwork.LocalPlayer.NickName}님이 <color=green>{selectedPlayer.NickName}</color>님에게 투표했습니다.";
        }
        else
        {
            message = $"{PhotonNetwork.LocalPlayer.NickName}님이 투표했습니다.";
        }

        InGameChatting.Instance.DisplaySystemMessage(message);
        chatClient.PublishMessage($"{PhotonNetwork.CurrentRoom.Name}_InGame", message);
    }
}