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

        PoliceChatting.Instance.DisplaySystemMessage($"{PhotonNetwork.LocalPlayer.NickName}���� <color=green>{selectedPlayer.NickName}<color=white>���� �����մϴ�...");

        PoliceAction(selectedPlayer);
    }

    private void PoliceAction(Player targetPlayer)
    {
        if (targetPlayer.CustomProperties.ContainsKey("job"))
        {
            string job = (string)targetPlayer.CustomProperties["job"];

            if (job == "Mafia" || job == "Gangster")
            {
                PoliceChatting.Instance.DisplaySystemMessage($"<color=green>{targetPlayer.NickName}<color=white>���� <color=red>���Ǿ�<color=white>�Դϴ�!");
            }

            else
            {
                PoliceChatting.Instance.DisplaySystemMessage($"<color=green>{targetPlayer.NickName}<color=white>���� ���Ǿư� �ƴմϴ�.");
            }
        }
    }
}