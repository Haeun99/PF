using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyChatting : MonoBehaviour
{
    public Button sendButton;
    public TMP_InputField chatInput;

    private void Start()
    {
        ChattingManager.Instance.Start();
        ChattingManager.Instance.JoinChannel(PhotonNetwork.CurrentRoom.Name, "Lobby");

        sendButton.onClick.AddListener(Chatting);
    }

    private void Update()
    {
        ChattingManager.Instance.Update();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Chatting();
        }
    }

    private void Chatting()
    {
        ChattingManager.Instance.SendMessageToChannel($"{PhotonNetwork.CurrentRoom.Name}_Lobby", chatInput);
    }

    void OnEnable()
    {
        ChattingManager.Instance.OnChatMessageReceived += HandleChatMessage;
    }

    void OnDisable()
    {
        ChattingManager.Instance.OnChatMessageReceived -= HandleChatMessage;
    }

    void HandleChatMessage(string channelName, string sender, string message)
    {
        
    }
}