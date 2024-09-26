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

        string message = $"[시스템]{PhotonNetwork.LocalPlayer.NickName}님이 살해 대상으로 <color=green>{selectedPlayer.NickName}<color=white>님을 선택했습니다.";

        MafiaTeamChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_MafiaTeam", message);

        MafiaAction(selectedPlayer);

        selectButton.gameObject.SetActive(false);
    }

    // 마피아 선택 일치해야함 이 부분 보수
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