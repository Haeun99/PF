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

        StalkerChatting.Instance.DisplaySystemMessage($"[�ý���]{PhotonNetwork.LocalPlayer.NickName}���� <color=green>{selectedPlayer.NickName}<color=white>���� ����ŷ�մϴ�...");

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