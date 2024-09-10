using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class GameStartButton : MonoBehaviourPunCallbacks
{
    private Button gameStartButton;

    private void Start()
    {
        gameStartButton = GetComponent<Button>();

        if (PhotonNetwork.IsMasterClient)
        {
            gameStartButton.interactable = false;
            gameStartButton.onClick.AddListener(StartGame);
        }
    }

    [PunRPC]
    public void CheckAllPlayersReady()
    {
        bool allReady = true;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.ContainsKey("IsReady") || !(bool)player.CustomProperties["IsReady"])
            {
                allReady = false;
                break;
            }
        }

        if (allReady)
        {
            gameStartButton.interactable = true;
        }
        else
        {
            gameStartButton.interactable = false;
        }
    }

    private void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // ���� ����
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        CheckAllPlayersReady();
    }
}
