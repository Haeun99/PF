using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Chat;
using ExitGames.Client.Photon;

public class ChattingManager : MonoBehaviour, IChatClientListener
{
    public static ChattingManager Instance { get; private set; }

    private Dictionary<string, Action<string, string>> channelHandlers;

    public event Action<string, string, string> OnChatMessageReceived;

    private ChatClient chatClient;

    public GameObject myChat;
    public GameObject otherChat;
    public GameObject deadChat;
    public GameObject systemChat;

    [Space(20)]
    public Transform LobbyContent;
    public Transform[] InGameContent;
    public Transform[] MafiaTeamContent;
    public Transform DoctorContent;
    public Transform PoliceContent;
    public Transform StalkerContent;

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

        channelHandlers = new Dictionary<string, Action<string, string>>
        {
            { "_Lobby", HandleLobbyChat },
            { "_InGame", HandleInGameChat },
            { "_MafiaTeam", HandleMafiaTeamChat },
            { "_Doctor", HandleDoctorChat },
            { "_Police", HandlePoliceChat },
            { "_Stalker", HandleStalkerChat }
        };
    }

    public void Start()
    {
        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, "1.0", new AuthenticationValues(PhotonNetwork.LocalPlayer.NickName));
    }

    public void Update()
    {
        if (chatClient != null)
        {
            chatClient.Service();
        }
    }

    public void JoinChannel(string roomName, string channelType)
    {
        string channel = roomName + "_" + channelType;
        SubscribeChannels(channel);
    }

    public void LeaveChannel(string roomName, string channelType)
    {
        string channel = roomName + "_" + channelType;
        UnsubscribeChannels(channel);
    }

    public void SubscribeChannels(params string[] channels)
    {
        chatClient.Subscribe(channels);
    }

    public void UnsubscribeChannels(params string[] channels)
    {
        chatClient.Unsubscribe(channels);
    }

    public void SendMessageToChannel(string channelName, TMP_InputField input)
    {
        string chat = input.text;

        if (!string.IsNullOrEmpty(chat))
        {
            chatClient.PublishMessage(channelName, chat);
            input.text = string.Empty;
            input.ActivateInputField();
        }
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < senders.Length; i++)
        {
            string sender = senders[i];
            string message = messages[i].ToString();

            OnChatMessageReceived?.Invoke(channelName, sender, message);

            foreach (var handler in channelHandlers)
            {
                if (channelName.EndsWith(handler.Key))
                {
                    handler.Value(sender, message);
                    break;
                }
            }
        }
    }

    private void HandleLobbyChat(string sender, string message)
    {
        if (sender == PhotonNetwork.LocalPlayer.NickName)
        {
            DisplayMyChat(message, LobbyContent);
        }
        else
        {
            DisplayOtherChat(message, sender, LobbyContent);
        }
    }

    private void HandleInGameChat(string sender, string message)
    {
        foreach (var chatContent in InGameContent)
        {
            if (sender == PhotonNetwork.LocalPlayer.NickName)
            {
                DisplayMyChat(message, chatContent);
            }

            else
            {
                DisplayOtherChat(message, sender, chatContent);
            }
        }
    }

    private void HandleMafiaTeamChat(string sender, string message)
    {
        foreach (var chatContent in MafiaTeamContent)
        {
            if (sender == PhotonNetwork.LocalPlayer.NickName)
            {
                DisplayMyChat(message, chatContent);
            }

            else
            {
                DisplayOtherChat(message, sender, chatContent);
            }
        }
    }

    private void HandleDoctorChat(string sender, string message)
    {
        if (sender == PhotonNetwork.LocalPlayer.NickName)
        {
            DisplayMyChat(message, DoctorContent);
        }
        else
        {
            DisplayOtherChat(message, sender, DoctorContent);
        }
    }

    private void HandlePoliceChat(string sender, string message)
    {
        if (sender == PhotonNetwork.LocalPlayer.NickName)
        {
            DisplayMyChat(message, PoliceContent);
        }
        else
        {
            DisplayOtherChat(message, sender, PoliceContent);
        }
    }

    private void HandleStalkerChat(string sender, string message)
    {
        if (sender == PhotonNetwork.LocalPlayer.NickName)
        {
            DisplayMyChat(message, StalkerContent);
        }
        else
        {
            DisplayOtherChat(message, sender, StalkerContent);
        }
    }

    public void DisplayMyChat(string message, Transform chatContent)
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

    public void DisplayOtherChat(string message, string sender, Transform chatContent)
    {
        var chatBubble = Instantiate(otherChat, chatContent);

        chatBubble.transform.Find("Nickname").GetComponent<TextMeshProUGUI>().text = sender;
        chatBubble.transform.Find("Chat Bubble/Chat").GetComponent<TextMeshProUGUI>().text = message;
    }

    public void DisplayDeadChat(string message, Transform chatContent)
    {
        var chatBubble = Instantiate(deadChat, chatContent);

        var nicknameText = chatBubble.transform.Find("Nickname").GetComponent<TextMeshProUGUI>();
        var messageText = chatBubble.transform.Find("Chat Bubble/Chat").GetComponent<TextMeshProUGUI>();

        if (nicknameText != null && messageText != null)
        {
            nicknameText.text = PhotonNetwork.LocalPlayer.NickName;
            messageText.text = message;
        }
    }

    public void DisplaySystemMessage(string message, Transform chatContent)
    {
        var chatBubble = Instantiate(systemChat, chatContent);

        chatBubble.transform.Find("Chat Bubble/Chat").GetComponent<TextMeshProUGUI>().text = message;
    }

    public void OnConnected() { }
    public void OnDisconnected() { }
    public void OnPrivateMessage(string sender, object message, string channel) { }
    public void OnStatusUpdate(string user, int status, bool gotMessage) { }
    public void OnUserSubscribed(string channel, string user) { }
    public void OnUserUnsubscribed(string channel, string user) { }
    public void OnSubscribed(string[] channels, bool[] results) { }
    public void OnUnsubscribed(string[] channels) { }
    public void DebugReturn(DebugLevel level, string message) { }
    public void OnChatStateChange(ChatState state) { }
    public void OnStatusUpdate(string user, int status, bool gotMessage, object message) { }
}