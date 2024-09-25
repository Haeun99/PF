using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameChatting : MonoBehaviour, IChatClientListener
{
    public static InGameChatting Instance { get; private set; }

    public GameObject myChat;
    public GameObject otherChat;
    public GameObject deadChat;
    public GameObject systemChat;
    public GameObject finalSystemChat;
    public Transform chatContent;
    public TMP_InputField chattingInput;
    public Button sendButton;

    public ChatClient chatClient;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

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
            chatClient.PublishMessage($"{PhotonNetwork.CurrentRoom.Name}_InGame", message);

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

    private void DisplayMyDeadChat(string message)
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

    private void DisplaOtherDeadChat(string message, string sender)
    {
        var chatBubble = Instantiate(deadChat, chatContent);

        chatBubble.transform.Find("Nickname").GetComponent<TextMeshProUGUI>().text = sender;
        chatBubble.transform.Find("Chat Bubble/Chat").GetComponent<TextMeshProUGUI>().text = message;
    }

    public void DisplaySystemMessage(string message)
    {
        string actualMessage = message.Replace("[시스템]", string.Empty);

        var chatBubble = Instantiate(systemChat, chatContent);
        chatBubble.transform.Find("Chat Bubble/Chat").GetComponent<TextMeshProUGUI>().text = actualMessage;
    }

    public void FinalAppealSystemMessage()
    {
        Instantiate(finalSystemChat, chatContent);
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if (channelName == $"{PhotonNetwork.CurrentRoom.Name}_InGame")
        {
            for (int i = 0; i < senders.Length; i++)
            {
                string sender = senders[i];
                string message = messages[i].ToString();

                if (message.StartsWith("[시스템]"))
                {
                    DisplaySystemMessage(message);
                }
                else
                {
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
        }

        else if (channelName == $"{PhotonNetwork.CurrentRoom.Name}_Dead")
        {
            for (int i = 0; i < senders.Length; i++)
            {
                string sender = senders[i];
                string message = messages[i].ToString();

                if (sender == PhotonNetwork.LocalPlayer.NickName)
                {
                    DisplayMyDeadChat(message);
                }
                else
                {
                    DisplaOtherDeadChat(message, sender);
                }
            }
        }
    }

    public void SubscribeToChannels(bool isDead)
    {
        if (isDead)
        {
            chatClient.Subscribe(new string[] { $"{PhotonNetwork.CurrentRoom.Name}_Dead" });
        }
        else
        {
            return;
        }
    }

    public void OnConnected()
    {
        chatClient.Subscribe(new string[] { $"{PhotonNetwork.CurrentRoom.Name}_InGame" });
    }

    public void OnDisconnected()
    {
        chatClient?.Disconnect();
        chatClient.Unsubscribe(new string[] { $"{PhotonNetwork.CurrentRoom.Name}_InGame", $"{PhotonNetwork.CurrentRoom.Name}_Dead" });
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
