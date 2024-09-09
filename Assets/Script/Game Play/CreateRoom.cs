using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    public static CreateRoom Instance { get; private set; }

    public TMP_InputField roomNameInput;
    public TextMeshProUGUI pwLabel;
    public TMP_InputField roomPWInput;
    public TMP_Dropdown maxPlayerDropdwon;
    public Toggle privateMode;
    public GameObject roomPrefab;
    public RectTransform roomList;

    private byte maxPlayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        ValidateRoomName();
        maxPlayerDropdwon.onValueChanged.AddListener(delegate { SetMaxPlayerNumber(); });
        roomNameInput.onValueChanged.AddListener(delegate { ValidateRoomName(); });
    }

    private void Update()
    {
        if (privateMode.isOn)
        {
            pwLabel.gameObject.SetActive(true);
            roomPWInput.gameObject.SetActive(true);
        }

        else
        {
            pwLabel.gameObject.SetActive(false);
            roomPWInput.gameObject.SetActive(false);
        }
    }

    private void SetMaxPlayerNumber()
    {
        int option = maxPlayerDropdwon.value;
        maxPlayer = (byte)(option + 4);
    }

    public void CreateMafiaRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayer;

        PhotonNetwork.CreateRoom(roomNameInput.text, roomOptions, TypedLobby.Default);
    }

    private void ValidateRoomName()
    {
        string roomName = roomNameInput.text;

        MafiaSceneUIManager.Instance.createButton.interactable = !(string.IsNullOrEmpty(roomName) || roomName.Length > 12);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        Debug.Log("마스터 클라이언트 이름: " + PhotonNetwork.MasterClient.NickName);
        Debug.Log("방 최대 인원 수: " + PhotonNetwork.CurrentRoom.MaxPlayers);
    }
} 