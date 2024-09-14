using System.Collections;
using System.Collections.Generic;
using TMPro;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RoomPanelController : MonoBehaviour
{
    public static RoomPanelController Instance { get; private set; }

    public TextMeshProUGUI roomNameText;
    public TextMeshProUGUI playerNumber;
    public Image privateRoom;
    public RoomInfo roomInfo;
    private Button roomCard;

    private static RoomPanelController selectedRoomPanel;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        roomCard = GetComponent<Button>();

        roomCard.onClick.AddListener(() => RoomCardClick());
    }

    public void RoomInformation(string roomName, int currentPlayer, int maxPlayer, bool isPrivate, RoomInfo info)
    {
        roomNameText.text = roomName;
        playerNumber.text = $"{currentPlayer} / {maxPlayer}";
        privateRoom.gameObject.SetActive(isPrivate);
        roomInfo = info;
    }

    private void RoomCardClick()
    {
        if (selectedRoomPanel != null)
        {
            selectedRoomPanel.GetComponent<Image>().color = Color.white;
        }

        selectedRoomPanel = this;
        GetComponent<Image>().color = Color.gray;
    }

    public static RoomInfo GetSelectedRoomInfo()
    {
        if (selectedRoomPanel != null)
        {
            return selectedRoomPanel.roomInfo;
        }

        return null;
    }

    public void UpdateButtonState()
    {
        if (roomInfo == null)
            return;

        bool isFull = PhotonNetwork.CurrentRoom.PlayerCount >= roomInfo.MaxPlayers;
        roomCard.interactable = !isFull;
    }
}