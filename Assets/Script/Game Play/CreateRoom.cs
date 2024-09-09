using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoom : MonoBehaviour
{
    public static CreateRoom Instance { get; private set; }

    public TMP_InputField roomNameInput;
    public TextMeshProUGUI pwLabel;
    public TMP_InputField roomPWInput;
    public TMP_Dropdown maxPlayer;
    public Toggle privateMode;
    public GameObject roomPrefab;
    public RectTransform roomList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
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

    public void CreateMafiaRoom()
    {
        RoomOptions roomOptions = new RoomOptions();


        PhotonNetwork.CreateRoom(roomNameInput.text, roomOptions, TypedLobby.Default);
    }
}
