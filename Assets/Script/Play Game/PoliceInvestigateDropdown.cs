using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PoliceInvestigateDropdown : InGamePlayerDropdown
{
    public override void UpdatePlayerList()
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

    public override void PlayerVote()
    {
        Player selectedPlayer = GetSelectedPlayer();

        Hashtable policeAction = new Hashtable
        {
            { "nightAction", "Police" }
        };

        selectButton.interactable = false;

        PoliceChatting.Instance.DisplaySystemMessage($"{PhotonNetwork.LocalPlayer.NickName}님이 <color=green>{selectedPlayer.NickName}<color=white>님을 조사합니다...");

        PoliceAction(selectedPlayer);
    }

    private void PoliceAction(Player targetPlayer)
    {
        if (targetPlayer.CustomProperties.ContainsKey("job"))
        {
            string job = (string)targetPlayer.CustomProperties["job"];

            if (job == "Mafia" || job == "Gangster")
            {
                PoliceChatting.Instance.DisplaySystemMessage($"<color=green>{targetPlayer.NickName}<color=white>님은 <color=red>마피아<color=white>입니다!");
            }

            else
            {
                PoliceChatting.Instance.DisplaySystemMessage($"<color=green>{targetPlayer.NickName}<color=white>님은 마피아가 아닙니다.");
            }
        }
    }
}