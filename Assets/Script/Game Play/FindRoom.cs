using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class FindRoom : MonoBehaviourPunCallbacks
{
    public static FindRoom Instance { get; private set; }

    private void Start()
    {
        if (Instance != null)
        {
            Instance = this;
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform child in CreateRoom.Instance.roomList)
        {
            Destroy(child.gameObject);
        }

        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList || roomInfo.PlayerCount == 0) continue;

            CreateRoom.Instance.RoomCreate();
        }
    }
}
