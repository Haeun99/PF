using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GangsterInvestigateDropdown : InGamePlayerDropdown
{
    public override void PlayerVote()
    {
        Player selectedPlayer = GetSelectedPlayer();

        Hashtable gangsterAction = new Hashtable
        {
            { "nightAction", "Gangster" }
        };

        string message = $"[�ý���]{PhotonNetwork.LocalPlayer.NickName}���� <color=green>{selectedPlayer.NickName}<color=white>���� �����մϴ�...";

        MafiaTeamChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_MafiaTeam", message);

        GangsterAction(selectedPlayer);

        selectButton.gameObject.SetActive(false);
    }

    private void GangsterAction(Player targetPlayer)
    {
        string job = StartGame.Instance.GetPlayerJob(targetPlayer);

        string message = $"[�ý���]<color=green>{targetPlayer.NickName}<color=white>���� <color=blue>{job}<color=white>�Դϴ�!";

        MafiaTeamChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_MafiaTeam", message);
    }
}