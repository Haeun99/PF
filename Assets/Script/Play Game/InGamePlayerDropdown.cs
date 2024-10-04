using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Chat;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Linq;

public class InGamePlayerDropdown : MonoBehaviourPunCallbacks
{
    public static InGamePlayerDropdown Instance { get; private set; }

    public TMP_Dropdown playerDropdown;
    public Button selectButton;

    private int voteEnd;

    public TMP_InputField[] chatting;

    public List<Player> players = new List<Player>();
    private bool isFinalAppeal;

    public bool AllVote
    {
        get
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (!player.CustomProperties.ContainsKey("votedPlayer") || player.CustomProperties["votedPlayer"] == null)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        UpdatePlayerList();
        selectButton.onClick.AddListener(PlayerVote);

        Hashtable roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
        isFinalAppeal = (bool)roomProperties["FinalAppeal"];
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        UpdatePlayerList();
    }

    public virtual void UpdatePlayerList()
    {
        playerDropdown.ClearOptions();
        players.Clear();

        List<string> playerNames = new List<string>();

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.ContainsKey("isDead") || !(bool)player.CustomProperties["isDead"])
            {
                playerNames.Add(player.NickName);
                players.Add(player);
            }
        }

        playerDropdown.AddOptions(playerNames);
    }

    public Player GetSelectedPlayer()
    {
        int selectedIndex = playerDropdown.value;

        if (selectedIndex < 0 || selectedIndex >= players.Count)
        {
            return null;
        }

        return players[selectedIndex];
    }

    public virtual void PlayerVote()
    {
        Player selectedPlayer = GetSelectedPlayer();

        Hashtable props = new Hashtable
        {
            { "votedPlayer", selectedPlayer }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        string message;

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("AnonymousVote"))
        {
            bool isAnonymousVote = (bool)PhotonNetwork.CurrentRoom.CustomProperties["AnonymousVote"];

            if (isAnonymousVote == false)
            {
                message = $"[�ý���]{PhotonNetwork.LocalPlayer.NickName}���� <color=yellow>{selectedPlayer.NickName}</color>�Կ��� ��ǥ�߽��ϴ�.";

                InGameChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_InGame", message);
            }

            else
            {
                message = $"[�ý���]{PhotonNetwork.LocalPlayer.NickName}���� ��ǥ�߽��ϴ�.";

                InGameChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_InGame", message);
            }
        }

        selectButton.gameObject.SetActive(false);
    }

    public void CalculateVoteResults()
    {
        Dictionary<Player, int> voteCount = new Dictionary<Player, int>();

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.ContainsKey("votedPlayer"))
            {
                Player votedPlayer = (Player)player.CustomProperties["votedPlayer"];

                if (votedPlayer != null)
                {
                    if (!voteCount.ContainsKey(votedPlayer))
                    {
                        voteCount[votedPlayer] = 0;
                    }
                    voteCount[votedPlayer]++;
                }
            }
        }

        Player mostVotedPlayer = null;
        int maxVotes = 0;
        int secondMaxVotes = 0;

        foreach (var entry in voteCount)
        {
            if (entry.Value > maxVotes)
            {
                secondMaxVotes = maxVotes;
                mostVotedPlayer = entry.Key;
                maxVotes = entry.Value;
            }
            else if (entry.Value > secondMaxVotes && entry.Value < maxVotes)
            {
                secondMaxVotes = entry.Value;
            }
        }


        if (mostVotedPlayer != null)
        {
            VoteManager.Instance.SetMostVotedPlayer(mostVotedPlayer);
        }

        if (maxVotes > secondMaxVotes)
        {
            bool isFinalAppeal = (bool)PhotonNetwork.CurrentRoom.CustomProperties["FinalAppeal"];

            if (isFinalAppeal == false)
            {
                PlayerStatus.Instance.SetDead(mostVotedPlayer, true);

                InGameChatting.Instance.DisplaySystemMessage($"[�ý���]{mostVotedPlayer.NickName}���� ó�� ���߽��ϴ�.");
            }

            else
            {
                return;
            }
        }

        else
        {
            InGameChatting.Instance.DisplaySystemMessage("[�ý���]��ǥ ����� �����Դϴ�. ó���� ������� �ʽ��ϴ�.");
        }
    }
}