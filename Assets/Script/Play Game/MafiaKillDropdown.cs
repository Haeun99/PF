using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MafiaKillDropdown : InGamePlayerDropdown
{
    public override void PlayerVote()
    {
        Player selectedPlayer = GetSelectedPlayer();

        Hashtable mafiaAction = new Hashtable
        {
            { "nightAction", "Mafia" }
        };

        MafiaTeamChatting.Instance.DisplaySystemMessage($"{PhotonNetwork.LocalPlayer.NickName}���� ���� ������� <color=green>{selectedPlayer.NickName}<color=white>���� �����߽��ϴ�.");

        MafiaAction(selectedPlayer);
    }

    // ���Ǿ� ���� ��ġ�ؾ��� �� �κ� ����
    private void MafiaAction(Player targetPlayer)
    {
        Hashtable props = new Hashtable
        {
            { "isDead", true }
        };

        targetPlayer.SetCustomProperties(props);
    }

    private void DeadPlayer()
    {
        PlayerStatus.Instance.SetDead(true);
    }
}