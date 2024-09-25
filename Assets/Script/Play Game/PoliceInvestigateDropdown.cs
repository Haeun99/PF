using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PoliceInvestigateDropdown : InGamePlayerDropdown
{
    public override void UpdatePlayerList()
    {
        playerDropdown.ClearOptions();
        players.Clear();

        List<string> playerNames = new List<string>();
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerNames.Add(player.NickName);
            players.Add(player);
        }

        playerDropdown.AddOptions(playerNames);
    }

    public override void PlayerVote()
    {
        Player selectedPlayer = GetSelectedPlayer();

        Hashtable policeAction = new Hashtable
        {
            { "nightAction", "Police" }
        };

        string message = $"[시스템]{PhotonNetwork.LocalPlayer.NickName}님이 <color=green>{selectedPlayer.NickName}<color=white>님을 조사합니다...";

        PoliceChatting.Instance.DisplaySystemMessage(message);
        chatClient.PublishMessage($"{PhotonNetwork.CurrentRoom.Name}_Police", message);

        PoliceAction(selectedPlayer);

        selectButton.gameObject.SetActive(false);
    }

    private void PoliceAction(Player targetPlayer)
    {
        if (targetPlayer.CustomProperties.ContainsKey("job"))
        {
            string job = (string)targetPlayer.CustomProperties["job"];

            if (job == "마피아" || job == "건달")
            {
                string message = ($"[시스템]<color=green>{targetPlayer.NickName}<color=white>님은 <color=red>마피아<color=white>입니다!");

                PoliceChatting.Instance.DisplaySystemMessage(message);
                chatClient.PublishMessage($"{PhotonNetwork.CurrentRoom.Name}_Police", message);
            }

            else
            {
                string message = ($"[시스템]<color=green>{targetPlayer.NickName}<color=white>님은 마피아가 아닙니다.");

                PoliceChatting.Instance.DisplaySystemMessage(message);
                chatClient.PublishMessage($"{PhotonNetwork.CurrentRoom.Name}_Police", message);
            }
        }
    }
}