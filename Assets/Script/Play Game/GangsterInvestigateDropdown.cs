using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GangsterInvestigateDropdown : InGamePlayerDropdown
{
    public override void PlayerVote()
    {
        Player selectedPlayer = GetSelectedPlayer();

        Hashtable gangsterAction = new Hashtable
        {
            { "nightAction", "Gangster" }
        };

        selectButton.interactable = false;

        MafiaTeamChatting.Instance.DisplaySystemMessage($"{PhotonNetwork.LocalPlayer.NickName}���� <color=green>{selectedPlayer.NickName}<color=white>���� �����մϴ�...");

        GangsterAction(selectedPlayer);
    }

    private void GangsterAction(Player targetPlayer)
    {
        if (targetPlayer.CustomProperties.ContainsKey("job"))
        {
            string job = (string)targetPlayer.CustomProperties["job"];

            if (job == "Mafia")
            {
                MafiaTeamChatting.Instance.DisplaySystemMessage($"<color=green>{targetPlayer.NickName}<color=white>���� <color=red>���Ǿ�<color=white>�Դϴ�!");
            }

            else if (job == "Gangster")
            {
                MafiaTeamChatting.Instance.DisplaySystemMessage($"<color=green>{targetPlayer.NickName}<color=white>���� <color=red>�Ǵ�<color=white>�Դϴ�!");
            }

            else if (job == "Doctor")
            {
                MafiaTeamChatting.Instance.DisplaySystemMessage($"<color=green>{targetPlayer.NickName}<color=white>���� <color=green>�ǻ�<color=white>�Դϴ�!");
            }

            else if (job == "Police")
            {
                MafiaTeamChatting.Instance.DisplaySystemMessage($"<color=green>{targetPlayer.NickName}<color=white>���� <color=blue>����<color=white>�Դϴ�!");
            }

            else if (job == "Stalker")
            {
                MafiaTeamChatting.Instance.DisplaySystemMessage($"<color=green>{targetPlayer.NickName}<color=white>���� <color=blue>����Ŀ<color=white>�Դϴ�!");
            }

            else
            {
                MafiaTeamChatting.Instance.DisplaySystemMessage($"<color=green>{targetPlayer.NickName}<color=white>���� �ù��Դϴ�.");
            }
        }
    }
}