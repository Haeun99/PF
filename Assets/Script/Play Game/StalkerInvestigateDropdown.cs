using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class StalkerInvestigateDropdown : InGamePlayerDropdown
{
    public override void PlayerVote()
    {
        Player selectedPlayer = GetSelectedPlayer();

        selectButton.interactable = false;

        StalkerChatting.Instance.DisplaySystemMessage($"{PhotonNetwork.LocalPlayer.NickName}¥‘¿Ã <color=green>{selectedPlayer.NickName}<color=white>¥‘¿ª Ω∫≈‰≈∑«’¥œ¥Ÿ...");

        StalkerAction(selectedPlayer);
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