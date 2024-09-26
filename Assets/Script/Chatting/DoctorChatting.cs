using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoctorChatting : MonoBehaviour, IChatClientListener
{
    public static DoctorChatting Instance { get; private set; }

    public GameObject myChat;
    public GameObject otherChat;
    public GameObject systemChat;
    public Transform chatContent;
    public TMP_InputField chattingInput;
    public Button sendButton;

    private ChatClient chatClient;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        DisplaySystemMessage("[시스템]<color=blue>의사<color=white> 전용 채팅방입니다.");
        DisplaySystemMessage("[시스템]의사는 매일 밤 한 명의 시민을 살릴 수 있습니다. 충분한 회의를 통해 의견을 통일하세요.");
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
            chatClient.PublishMessage($"{PhotonNetwork.CurrentRoom.Name}_Doctor", message);

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
        string actualMessage = message.Replace("[시스템]", string.Empty);

        var chatBubble = Instantiate(systemChat, chatContent);
        chatBubble.transform.Find("Chat Bubble/Chat").GetComponent<TextMeshProUGUI>().text = actualMessage;
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if (channelName != $"{PhotonNetwork.CurrentRoom.Name}_Doctor")
            return;

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

    public void SendSystemMessage(string channel, string message)
    {
        chatClient.PublishMessage(channel, message);
    }

    public void OnConnected()
    {
        chatClient.Subscribe(new string[] { $"{PhotonNetwork.CurrentRoom.Name}_Doctor" });
    }

    public void OnDisconnected()
    {
        chatClient?.Disconnect();
        chatClient.Unsubscribe(new string[] { $"{PhotonNetwork.CurrentRoom.Name}_Doctor" });
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
