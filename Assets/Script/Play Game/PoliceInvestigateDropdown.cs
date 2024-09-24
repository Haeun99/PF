using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PoliceInvestigateDropdown : MonoBehaviour
{
    public TMP_Dropdown playerDropdown;
    public Button investigateButton;

    private List<Player> players = new List<Player>();

    private void Start()
    {
        UpdatePlayerList();
        investigateButton.onClick.AddListener(playerSelect);
    }

    private void UpdatePlayerList()
    {
        playerDropdown.ClearOptions();
        players.Clear();

        List<string> playerNames = new List<string>();
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerNames.Add(player.NickName);
            players.Add(player);
        }

        playerDropdown.AddOptions(playerNames);
    }

    public Player GetSelectedPlayer()
    {
        int selectedIndex = playerDropdown.value;
        return players[selectedIndex];
    }

    public void playerSelect()
    {
        Player selectedPlayer = GetSelectedPlayer();

        Hashtable policeAction = new Hashtable
        {
            { "nightAction", "Police" }
        };

        PoliceAction(selectedPlayer);

        investigateButton.interactable = false;

        PoliceChatting.Instance.DisplaySystemMessage($"{PhotonNetwork.LocalPlayer.NickName}¥‘¿Ã <color=green>{selectedPlayer.NickName}<color=white>¥‘¿ª ¡∂ªÁ«’¥œ¥Ÿ...");
    }

    private void PoliceAction(Player targetPlayer)
    {
        if (targetPlayer.CustomProperties.ContainsKey("job"))
        {
            string job = (string)targetPlayer.CustomProperties["job"];
        }
    }
}