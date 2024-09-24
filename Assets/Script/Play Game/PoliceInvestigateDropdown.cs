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

        // 조사해서 시스템 채팅으로 시스템 메시지 전달

        investigateButton.interactable = false;
    }
}