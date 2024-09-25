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

        MafiaTeamChatting.Instance.DisplaySystemMessage($"{PhotonNetwork.LocalPlayer.NickName}¥‘¿Ã <color=green>{selectedPlayer.NickName}<color=white>¥‘¿ª ¡∂ªÁ«’¥œ¥Ÿ...");

        GangsterAction(selectedPlayer);
    }

    private void GangsterAction(Player targetPlayer)
    {
        if (targetPlayer.CustomProperties.ContainsKey("job"))
        {
            string job = (string)targetPlayer.CustomProperties["job"];

            MafiaTeamChatting.Instance.DisplaySystemMessage($"<color=green>{targetPlayer.NickName}<color=white>¥‘¿∫ <color=blue>{job}<color=white>¿‘¥œ¥Ÿ!");
        }
    }
}