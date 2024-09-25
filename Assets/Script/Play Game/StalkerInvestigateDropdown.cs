using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class StalkerInvestigateDropdown : InGamePlayerDropdown
{
    public override void PlayerVote()
    {
        Player selectedPlayer = GetSelectedPlayer();

        StalkerChatting.Instance.DisplaySystemMessage($"[시스템]{PhotonNetwork.LocalPlayer.NickName}님이 <color=green>{selectedPlayer.NickName}<color=white>님을 스토킹합니다...");

        StalkerAction(selectedPlayer);

        selectButton.gameObject.SetActive(false);
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