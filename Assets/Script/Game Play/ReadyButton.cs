using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class ReadyButton : MonoBehaviourPun
{
    private bool isReady = false;
    private Button readyButton;
    private PhotonView photonview;

    private void Start()
    {
        photonview = GetComponent<PhotonView>();
        readyButton = GetComponent<Button>();

        readyButton.onClick.AddListener(ReadyState);
    }

    private void ReadyState()
    {
        isReady = !isReady;
        readyButton.image.color = isReady ? Color.white : Color.gray;
        readyButton.GetComponentInChildren<Text>().text = isReady ? "준비 취소" : "준비하기";

        photonview.RPC("SetReadyState", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber, isReady);
    }

    [PunRPC]
    public void SetReadyState(int playerID, bool readyState)
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "IsReady", readyState } });
    }
}
