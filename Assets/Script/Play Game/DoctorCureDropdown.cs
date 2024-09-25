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

        string message = $"[�ý���]{PhotonNetwork.LocalPlayer.NickName}���� <color=green>{selectedPlayer.NickName}<color=white>���� ġ���մϴ�...";

        DoctorChatting.Instance.DisplaySystemMessage(message);
        chatClient.PublishMessage($"{PhotonNetwork.CurrentRoom.Name}_Doctor", message);

        selectButton.gameObject.SetActive(false);
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