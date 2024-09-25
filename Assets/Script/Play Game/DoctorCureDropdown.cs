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

        string message = $"[시스템]{PhotonNetwork.LocalPlayer.NickName}님이 <color=green>{selectedPlayer.NickName}<color=white>님을 치료합니다...";

        DoctorChatting.Instance.DisplaySystemMessage(message);
        chatClient.PublishMessage($"{PhotonNetwork.CurrentRoom.Name}_Doctor", message);

        selectButton.gameObject.SetActive(false);
    }

    // 마피아랑 선택 겹쳤을 때 system message 달라야함
    private void DoctorAction(Player targetPlayer)
    {
        Hashtable props = new Hashtable
        {
            { "isDead", false }
        };
        targetPlayer.SetCustomProperties(props);
    }
}