using TMPro;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class FindRoom : MonoBehaviourPunCallbacks
{
    public static FindRoom Instance { get; private set; }
    public TMP_InputField pwInput;
    public TMP_InputField inviteCodeInput;
    public Button pwConfirmButton;
    public TextMeshProUGUI pwError;
    public Button inviteCodeConfirmButton;
    public TextMeshProUGUI inviteCodeError;
    public Button roomEnterButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        roomEnterButton.onClick.AddListener(EnterSelectedRoom);
        pwConfirmButton.onClick.AddListener(EnterPrivateRoom);
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

            CreateRoom.Instance.RoomCreate(roomInfo);
        }
    }

    public void EnterSelectedRoom()
    {
        RoomInfo selectedRoom = RoomPanelController.GetSelectedRoomInfo();

        if (selectedRoom == null)
        {
            return;
        }

        string roomPW = null;

        if (selectedRoom.CustomProperties != null && selectedRoom.CustomProperties.ContainsKey("RoomPassword"))
        {
            roomPW = (string)selectedRoom.CustomProperties["RoomPassword"];
        }

        else
        {
            return;
        }

        if (selectedRoom.CustomProperties.ContainsKey("IsPrivate") && (bool)selectedRoom.CustomProperties["IsPrivate"])
        {
            AppearPWPopup();
        }

        else
        {
            PhotonNetwork.JoinRoom(selectedRoom.Name);
            MafiaSceneUIManager.Instance.SetPopupActive();
            MafiaSceneUIManager.Instance.lobbyPanel.gameObject.SetActive(true);
        }
    }

    public void EnterPrivateRoom()
    {
        RoomInfo selectedRoom = RoomPanelController.GetSelectedRoomInfo();

        string roomPW = null;

        if (selectedRoom == null)
        {
            return;
        }

        if (selectedRoom.CustomProperties != null && selectedRoom.CustomProperties.ContainsKey("RoomPassword"))
        {
            roomPW = (string)selectedRoom.CustomProperties["RoomPassword"];
        }

        if (pwInput.text == roomPW)
        {
            pwError.gameObject.SetActive(false);
            PhotonNetwork.JoinRoom(selectedRoom.Name);
            MafiaSceneUIManager.Instance.SetPopupActive();
            MafiaSceneUIManager.Instance.lobbyPanel.gameObject.SetActive(true);
        }

        else
        {
            pwError.gameObject.SetActive(true);
        }

    }

    public void AppearPWPopup()
    {
        pwInput.text = string.Empty;
        MafiaSceneUIManager.Instance.passwordPopup.gameObject.SetActive(true);
    }
}