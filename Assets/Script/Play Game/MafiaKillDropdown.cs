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
    public TMP_Dropdown playerDropdown;
    public Button killButton;

    private List<Player> players = new List<Player>();

    private void Start()
    {
        UpdatePlayerList();

        killButton.onClick.AddListener(PlayerVote);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        UpdatePlayerList();
    }

    private void UpdatePlayerList()
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

    public void PlayerVote()
    {
        Player selectedPlayer = GetSelectedPlayer();

        Hashtable mafiaAction = new Hashtable
        {
            { "nightAction", "Mafia" }
        };

        MafiaAction(selectedPlayer);

        MafiaTeamChatting.Instance.DisplaySystemMessage($"{PhotonNetwork.LocalPlayer.NickName}님이 살해 대상으로 <color=green>{selectedPlayer.NickName}<color=white>님을 선택했습니다.");
    }

    private void MafiaAction(Player targetPlayer)
    {
        Hashtable props = new Hashtable
        {
            { "isDead", true }
        };

        targetPlayer.SetCustomProperties(props);
    }

    private void DeadPlayer()
    {
        PlayerStatus.Instance.SetDead(true);
    }
}