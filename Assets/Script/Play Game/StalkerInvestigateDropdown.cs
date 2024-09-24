using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class StalkerInvestigateDropdown : MonoBehaviourPunCallbacks
{
    public TMP_Dropdown playerDropdown;
    public Button selectButton;

    private List<Player> players = new List<Player>();

    private void Start()
    {
        UpdatePlayerList();

        selectButton.onClick.AddListener(PlayerVote);
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

        StalkerAction(selectedPlayer);

        selectButton.interactable = false;

        StalkerChatting.Instance.DisplaySystemMessage($"{PhotonNetwork.LocalPlayer.NickName}¥‘¿Ã <color=green>{selectedPlayer.NickName}<color=white>¥‘¿ª Ω∫≈‰≈∑«’¥œ¥Ÿ...");
    }

    private void StalkerAction(Player targetPlayer)
    {
        if (targetPlayer.CustomProperties.ContainsKey("nightAction"))
        {
            string action = (string)targetPlayer.CustomProperties["nightAction"];
        }

        else
        {

        }
    }
}