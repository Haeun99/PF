using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanelController : MonoBehaviour
{
    public TextMeshProUGUI roomNameText;
    public TextMeshProUGUI playerNumber;
    public Image privateRoom;

    public void RoomInformation(string roomName, int currentPlayer, int maxPlayer, bool isPrivate)
    {
        roomNameText.text = roomName;
        playerNumber.text = $"{currentPlayer} / {maxPlayer}";
        privateRoom.gameObject.SetActive(isPrivate);
    }
}