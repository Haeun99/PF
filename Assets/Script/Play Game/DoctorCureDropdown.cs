using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class DoctorCureDropdown : InGamePlayerDropdown
{
    public override void PlayerVote()
    {
        Player selectedPlayer = GetSelectedPlayer();

        Hashtable doctorAction = new Hashtable
        {
            { "nightAction", "Doctor" }
        };

        DoctorAction(selectedPlayer);

        selectButton.interactable = false;

        DoctorChatting.Instance.DisplaySystemMessage($"{PhotonNetwork.LocalPlayer.NickName}���� <color=green>{selectedPlayer.NickName}<color=white>���� ġ���մϴ�...");
    }

    // ���Ǿƶ� ���� ������ �� system message �޶����
    private void DoctorAction(Player targetPlayer)
    {
        Hashtable props = new Hashtable
        {
            { "isDead", false }
        };
        targetPlayer.SetCustomProperties(props);
    }
}