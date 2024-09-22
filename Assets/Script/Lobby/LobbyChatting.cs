using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Chat;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class LobbyChatting : MonoBehaviour, IChatClientListener
{
    public GameObject myChat;
    public GameObject otherChat;
    public GameObject systemChat;
    public Transform chatContent;
    public TMP_InputField chattingInput;
    public Button sendButton;

    private ChatClient chatClient;

    void Start()
    {
        chatClient = new ChatClient(this);
        chatClient.Connect("0d890b75-4bab-4ee1-8f70-1e44f6fe3a17", "1.0", new AuthenticationValues(PhotonNetwork.LocalPlayer.NickName));

        sendButton.onClick.AddListener(SendChatMessage);
    }

    void Update()
    {
        if (chatClient != null)
        {
            chatClient.Service();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendChatMessage();
        }
    }

    public void SendChatMessage()
    {
        string message = chattingInput.text;
        if (!string.IsNullOrEmpty(message))
        {
            chatClient.PublishMessage("Lobby", message);
            DisplayMyChat(message);
            chattingInput.text = "";
            chattingInput.ActivateInputField();
        }
    }

    private void DisplayMyChat(string message)
    {
        var chatBubble = Instantiate(myChat, chatContent);

        var nicknameText = chatBubble.transform.Find("Nickname").GetComponent<TextMeshProUGUI>();
        var messageText = chatBubble.transform.Find("Chat Bubble/Chat").GetComponent<TextMeshProUGUI>();

        if (nicknameText != null && messageText != null)
        {
            nicknameText.text = PhotonNetwork.LocalPlayer.NickName;
            messageText.text = message;
        }
    }

    public void OnChatMessageReceived(string channel, string message, object sender)
    {
        if (sender.ToString() == PhotonNetwork.LocalPlayer.NickName)
        {
            DisplayMyChat(message);
        }
        else
        {
            DisplayOtherChat(message, sender.ToString());
        }
    }

    private void DisplayOtherChat(string message, string sender)
    {
        var chatBubble = Instantiate(otherChat, chatContent);

        chatBubble.transform.Find("Nickname").GetComponent<TextMeshProUGUI>().text = sender;
        chatBubble.transform.Find("Chat Bubble/Chat").GetComponent<TextMeshProUGUI>().text = message;
    }

    public void DisplaySystemMessage(string message)
    {
        var chatBubble = Instantiate(systemChat, chatContent);

        chatBubble.transform.Find("Chat Bubble/Chat").GetComponent<TextMeshProUGUI>().text = message;
    }

    public void PlayerJoined(string playerName)
    {
        DisplaySystemMessage($"{playerName}님이 들어왔습니다.");
    }

    public void OnConnected()
    {
        chatClient.Subscribe(new string[] { "Lobby" });
    }

    public void OnDisconnected()
    {
    }
    public void OnPrivateMessage(string sender, object message, string channel)
    {
    }
    public void OnStatusUpdate(string user, int status, bool gotMessage)
    {
    }
    public void OnUserSubscribed(string channel, string user)
    {
    }
    public void OnUserUnsubscribed(string channel, string user)
    {
    }
    public void OnSubscribed(string[] channels, bool[] results)
    {
    }
    public void OnUnsubscribed(string[] channels)
    {
    }
    public void DebugReturn(DebugLevel level, string message)
    {
    }
    public void OnChatStateChange(ChatState state)
    {
    }
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
    }
    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
    }
}