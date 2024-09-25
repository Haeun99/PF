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

        DoctorChatting.Instance.DisplaySystemMessage($"{PhotonNetwork.LocalPlayer.NickName}¥‘¿Ã <color=green>{selectedPlayer.NickName}<color=white>¥‘¿ª ƒ°∑·«’¥œ¥Ÿ...");
    }

    // ∏∂««æ∆∂˚ º±≈√ ∞„√∆¿ª ∂ß system message ¥ﬁ∂Ûæﬂ«‘
    private void DoctorAction(Player targetPlayer)
    {
        Hashtable props = new Hashtable
        {
            { "isDead", false }
        };
        targetPlayer.SetCustomProperties(props);
    }
}