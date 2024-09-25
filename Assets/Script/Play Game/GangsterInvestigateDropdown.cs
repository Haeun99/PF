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

        MafiaTeamChatting.Instance.DisplaySystemMessage($"{PhotonNetwork.LocalPlayer.NickName}님이 <color=green>{selectedPlayer.NickName}<color=white>님을 조사합니다...");

        GangsterAction(selectedPlayer);
    }

    private void GangsterAction(Player targetPlayer)
    {
        if (targetPlayer.CustomProperties.ContainsKey("job"))
        {
            string job = (string)targetPlayer.CustomProperties["job"];

            if (job == "Mafia")
            {
                MafiaTeamChatting.Instance.DisplaySystemMessage($"<color=green>{targetPlayer.NickName}<color=white>님은 <color=red>마피아<color=white>입니다!");
            }

            else if (job == "Gangster")
            {
                MafiaTeamChatting.Instance.DisplaySystemMessage($"<color=green>{targetPlayer.NickName}<color=white>님은 <color=red>건달<color=white>입니다!");
            }

            else if (job == "Doctor")
            {
                MafiaTeamChatting.Instance.DisplaySystemMessage($"<color=green>{targetPlayer.NickName}<color=white>님은 <color=green>의사<color=white>입니다!");
            }

            else if (job == "Police")
            {
                MafiaTeamChatting.Instance.DisplaySystemMessage($"<color=green>{targetPlayer.NickName}<color=white>님은 <color=blue>경찰<color=white>입니다!");
            }

            else if (job == "Stalker")
            {
                MafiaTeamChatting.Instance.DisplaySystemMessage($"<color=green>{targetPlayer.NickName}<color=white>님은 <color=blue>스토커<color=white>입니다!");
            }

            else
            {
                MafiaTeamChatting.Instance.DisplaySystemMessage($"<color=green>{targetPlayer.NickName}<color=white>님은 시민입니다.");
            }
        }
    }
}