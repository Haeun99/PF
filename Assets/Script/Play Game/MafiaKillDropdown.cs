using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Linq;

public class MafiaKillDropdown : MonoBehaviourPunCallbacks
{
    public static MafiaKillDropdown Instance { get; private set; }

    public TMP_Dropdown playerDropdown;
    public Button selectButton;

    private int voteEnd;

    private int nightTime;

    public List<Player> players = new List<Player>();

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
        nightTime = (int)roomProperties["NightTime"];
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
            if (player == PhotonNetwork.LocalPlayer)
                continue;

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

        if (localPlayer.CustomProperties.ContainsKey("Job") && localPlayer.CustomProperties["Job"].Equals("���Ǿ�"))
        {
            Player selectedPlayer = GetSelectedPlayer();

            Hashtable mafiaAction = new Hashtable
            {
                { "nightAction", "Mafia" },
                { "MafiaSelectedPlayer", selectedPlayer.NickName }
            };

            PhotonNetwork.LocalPlayer.SetCustomProperties(mafiaAction);

            string message = $"[�ý���]{localPlayer.NickName}���� ���� ������� <color=green>{selectedPlayer.NickName}<color=white>���� �����߽��ϴ�.";

            MafiaTeamChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_MafiaTeam", message);

            selectButton.gameObject.SetActive(false);

            if (nightTime == 0)
            {
                selectButton.gameObject.SetActive(false);
            }
        }

        else
        {
            return;
        }
    }

    public IEnumerator OnNightTimeEnd()
    {
        Player killTarget = CheckVotes();
        string cureTarget = (string)PhotonNetwork.LocalPlayer.CustomProperties["DoctorSelectedPlayer"];

        if (killTarget != null)
        {
            Debug.Log($"Cure Target: {cureTarget}, Kill Target: {killTarget.NickName}");

            if (!string.IsNullOrEmpty(cureTarget) && killTarget.NickName == cureTarget)
            {
                PlayerStatus.Instance.SetDead(killTarget, false);
                InGameChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_InGame", $"[�ý���]<color=yellow>���� ��, ���Ǿƿ��� ���� ���� ����� �ǻ簡 ��Ƚ��ϴ�!");
            }
            else
            {
                MafiaAction(killTarget);
            }
        }
        else
        {
            InGameChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_InGame", "[�ý���]���� ���� �ƹ� �ϵ� �Ͼ�� �ʾҽ��ϴ�.");
        }

        yield return null;
    }

    public Player CheckVotes()
    {
        Dictionary<string, int> voteCounts = new Dictionary<string, int>();

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.ContainsKey("nightAction") && player.CustomProperties["nightAction"].Equals("Mafia"))
            {
                string selectedPlayerName = (string)player.CustomProperties["MafiaSelectedPlayer"];
                if (!voteCounts.ContainsKey(selectedPlayerName))
                {
                    voteCounts[selectedPlayerName] = 0;
                }
                voteCounts[selectedPlayerName]++;
            }
        }

        if (voteCounts.Count == 1)
        {
            string targetPlayerName = voteCounts.Keys.First();
            return PhotonNetwork.PlayerList.FirstOrDefault(p => p.NickName == targetPlayerName);
        }

        return null;
    }

    public void MafiaAction(Player targetPlayer)
    {
        PlayerStatus.Instance.SetDead(targetPlayer, true);

        InGameChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_InGame", $"[�ý���]���� ��, ���Ǿƿ� ���� <color=red>{targetPlayer.NickName}<color=white>���� ���ش��߽��ϴ�.");
    }
}