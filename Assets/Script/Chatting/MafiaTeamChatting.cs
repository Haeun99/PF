using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MafiaTeamChatting : MonoBehaviour, IChatClientListener
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
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, "1.0", new AuthenticationValues(PhotonNetwork.LocalPlayer.NickName));

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
            chatClient.PublishMessage($"{PhotonNetwork.CurrentRoom.Name}_MafiaTeam", message);

            chattingInput.text = "";
            chattingInput.ActivateInputField();
        }
    }


    private void DisplayMyChat(string message)
    {
        string job = StartGame.Instance.GetPlayerJob(PhotonNetwork.LocalPlayer);

        var chatBubble = Instantiate(myChat, chatContent);

        var nicknameText = chatBubble.transform.Find("Nickname").GetComponent<TextMeshProUGUI>();
        var messageText = chatBubble.transform.Find("Chat Bubble/Chat").GetComponent<TextMeshProUGUI>();

        if (nicknameText != null && messageText != null)
        {
            nicknameText.text = PhotonNetwork.LocalPlayer.NickName;
            messageText.text = message;
        }

        if (job == "Mafia")
        {
            nicknameText.color = new Color(1.0f, 0.1686f, 0.0627f);
        }

        else
        {
            nicknameText.color = new Color(0.1647f, 0.3529f, 1.0f);
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
        Photon.Realtime.Player targetPlayer = null;
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.NickName == sender)
            {
                targetPlayer = player;
                break;
            }
        }

        var chatBubble = Instantiate(otherChat, chatContent);
        var nicknameText = chatBubble.transform.Find("Nickname").GetComponent<TextMeshProUGUI>();
        var messageText = chatBubble.transform.Find("Chat Bubble/Chat").GetComponent<TextMeshProUGUI>();

        if (nicknameText != null && messageText != null)
        {
            nicknameText.text = sender;
            messageText.text = message;
        }

        if (targetPlayer != null)
        {
            string job = StartGame.Instance.GetPlayerJob(targetPlayer);

            if (job == "Mafia")
            {
                nicknameText.color = new Color(1.0f, 0.1686f, 0.0627f);
            }
            else
            {
                nicknameText.color = new Color(0.1647f, 0.3529f, 1.0f);
            }
        }
    }

    public void DisplaySystemMessage(string message)
    {
        var chatBubble = Instantiate(systemChat, chatContent);

        chatBubble.transform.Find("Chat Bubble/Chat").GetComponent<TextMeshProUGUI>().text = message;
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if (channelName != $"{PhotonNetwork.CurrentRoom.Name}_MafiaTeam")
            return;

        for (int i = 0; i < senders.Length; i++)
        {
            string sender = senders[i];
            string message = messages[i].ToString();

            if (sender == PhotonNetwork.LocalPlayer.NickName)
            {
                DisplayMyChat(message);
            }
            else
            {
                DisplayOtherChat(message, sender);
            }
        }
    }

    public void OnConnected()
    {
        chatClient.Subscribe(new string[] { $"{PhotonNetwork.CurrentRoom.Name}_MafiaTeam" });
    }

    public void OnDisconnected()
    {
        chatClient?.Disconnect();
        chatClient.Unsubscribe(new string[] { $"{PhotonNetwork.CurrentRoom.Name}_MafiaTeam" });
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
    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
    }
}
