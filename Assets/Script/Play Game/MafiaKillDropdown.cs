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
            { "nightAction", "Mafia" },
            { "selectedPlayer", selectedPlayer.NickName }
        };

        string message = $"[�ý���]{PhotonNetwork.LocalPlayer.NickName}���� ���� ������� <color=green>{selectedPlayer.NickName}<color=white>���� �����߽��ϴ�.";

        MafiaTeamChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_MafiaTeam", message);

        MafiaAction(selectedPlayer);

        selectButton.gameObject.SetActive(false);
    }

    // ���Ǿ� ���� ��ġ�ؾ��� �� �κ� ����
    private void MafiaAction(Player targetPlayer)
    {
        Hashtable props = new Hashtable
        {
            { "isDead", true }
        };

        targetPlayer.SetCustomProperties(props);

        DeadPlayer(targetPlayer);
    }

    private void DeadPlayer(Player player)
    {
        PlayerStatus.Instance.SetDead(true);
    }
}