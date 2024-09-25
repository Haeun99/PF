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

        string message = $"[시스템]{PhotonNetwork.LocalPlayer.NickName}님이 <color=green>{selectedPlayer.NickName}<color=white>님을 조사합니다...";

        MafiaTeamChatting.Instance.DisplaySystemMessage(message);
        chatClient.PublishMessage($"{PhotonNetwork.CurrentRoom.Name}_MafiaTeam", message);

        GangsterAction(selectedPlayer);

        selectButton.gameObject.SetActive(false);
    }

    private void GangsterAction(Player targetPlayer)
    {
        if (targetPlayer.CustomProperties.ContainsKey("job"))
        {
            string job = (string)targetPlayer.CustomProperties["job"];

            MafiaTeamChatting.Instance.DisplaySystemMessage($"[시스템]<color=green>{targetPlayer.NickName}님은 <color=blue>{job}<color=white>입니다!");
        }
    }
}