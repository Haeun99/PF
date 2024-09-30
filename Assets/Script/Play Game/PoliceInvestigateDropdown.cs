using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PoliceInvestigateDropdown : MonoBehaviourPunCallbacks
{
    public static PoliceInvestigateDropdown Instance { get; private set; }

    public TMP_Dropdown playerDropdown;
    public Button selectButton;

    private int voteEnd;

    public List<Player> players = new List<Player>();

    private Dictionary<Player, Player> policeVotes = new Dictionary<Player, Player>();

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

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("NightTime"))
        {
            voteEnd = (int)PhotonNetwork.CurrentRoom.CustomProperties["NightTime"];
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        UpdatePlayerList();
    }

    public void UpdatePlayerList()
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

    public void PlayerVote()
    {
        Player localPlayer = PhotonNetwork.LocalPlayer;

        if (localPlayer.CustomProperties.ContainsKey("Job") && localPlayer.CustomProperties["Job"].Equals("����"))
        {
            Player selectedPlayer = GetSelectedPlayer();

            if (selectedPlayer != null)
            {
                policeVotes[localPlayer] = selectedPlayer;
            }

            Hashtable policeAction = new Hashtable
            {
                { "nightAction", "Police" },
                { "selectedPlayer", selectedPlayer }
            };

            string message = $"[�ý���]{PhotonNetwork.LocalPlayer.NickName}���� <color=green>{selectedPlayer.NickName}<color=white>���� �����մϴ�...";

            PoliceChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_Police", message);
            if (Time.time >= voteEnd)
            {
                selectButton.gameObject.SetActive(false);
            }
        }

        else
        {
            return;
        }
    }

    public void OnNightTimeEnd()
    {
        Player selectedTarget = CheckVotes();
        if (selectedTarget != null)
        {
            PoliceAction(selectedTarget);
        }
        else
        {
            return;
        }
    }

    public Player CheckVotes()
    {
        Player lastSelectedPlayer = null;
        bool allVotesMatch = true;
        bool hasVotes = false;

        foreach (Player Police in PhotonNetwork.PlayerList)
        {
            if (!policeVotes.ContainsKey(Police))
            {
                continue;
            }

            hasVotes = true;

            if (lastSelectedPlayer == null)
            {
                lastSelectedPlayer = policeVotes[Police];
            }
            else
            {
                if (lastSelectedPlayer != policeVotes[Police])
                {
                    allVotesMatch = false;
                    break;
                }
            }
        }

        if (!hasVotes || lastSelectedPlayer == null)
        {
            return null;
        }

        return allVotesMatch ? lastSelectedPlayer : null;
    }

    public void PoliceAction(Player targetPlayer)
    {
        string job = StartGame.Instance.GetPlayerJob(targetPlayer);

        if (job == "���Ǿ�" || job == "�Ǵ�")
        {
            string message = ($"[�ý���]<color=green>{targetPlayer.NickName}<color=white>���� <color=red>���Ǿ�<color=white>�Դϴ�!");

            PoliceChatting.Instance.DisplaySystemMessage(message);
        }

        else
        {
            string message = ($"[�ý���]<color=green>{targetPlayer.NickName}<color=white>���� ���Ǿư� �ƴմϴ�.");

            PoliceChatting.Instance.DisplaySystemMessage(message);
        }
    }
}