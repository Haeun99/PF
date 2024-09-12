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

    private int maxPlayer;

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
        ValidateRoomName();
        SetMaxPlayerNumber();
        ValidatePw();

        maxPlayerDropdwon.onValueChanged.AddListener(delegate { SetMaxPlayerNumber(); });
        roomNameInput.onValueChanged.AddListener(delegate { ValidateRoomName(); });
        roomPWInput.onValueChanged.AddListener(delegate { ValidatePw(); });
        privateMode.onValueChanged.AddListener(delegate { ValidatePw(); });
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
        maxPlayer = (byte)option + 4;
    }

    public void CreateMafiaRoom()
    {
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = maxPlayer,
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
            {
                { "IsPrivate", privateMode.isOn },
                { "RoomPassword", roomPWInput.text }
            },
            CustomRoomPropertiesForLobby = new string[] { "IsPrivate", "RoomPassword" }
        };

        PhotonNetwork.CreateRoom(roomNameInput.text, roomOptions, TypedLobby.Default);
    }

    private void ValidateRoomName()
    {
        string roomName = roomNameInput.text;

        MafiaSceneUIManager.Instance.createButton.interactable = !(string.IsNullOrEmpty(roomName) || roomName.Length > 12);
    }

    private void ValidatePw()
    {
        if (privateMode.isOn)
        {
            MafiaSceneUIManager.Instance.createButton.interactable = !string.IsNullOrEmpty(roomPWInput.text);
        }

        else
        {
            ValidateRoomName();
        }
    }

    public void RoomCreate(RoomInfo roomInfo)
    {
        bool isPrivate = roomInfo.CustomProperties.ContainsKey("IsPrivate") && (bool)roomInfo.CustomProperties["IsPrivate"];

        GameObject room = PhotonNetwork.Instantiate(roomPrefab.name, roomList.position, Quaternion.identity);
        room.transform.SetParent(roomList, false);

        RoomPanelController roomPanel = room.GetComponent<RoomPanelController>();

        if (roomPanel != null)
        {
            roomPanel.RoomInformation(roomInfo.Name, roomInfo.PlayerCount, roomInfo.MaxPlayers, isPrivate, roomInfo);
        }
    }
}