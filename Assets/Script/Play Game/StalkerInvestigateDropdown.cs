using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class StalkerInvestigateDropdown : MonoBehaviourPunCallbacks
{
    public static StalkerInvestigateDropdown Instance { get; private set; }

    public TMP_Dropdown playerDropdown;
    public Button selectButton;

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
        Player selectedPlayer = GetSelectedPlayer();

        string message = $"[�ý���]{PhotonNetwork.LocalPlayer.NickName}���� <color=green>{selectedPlayer.NickName}<color=white>���� �����մϴ�...";

        StalkerChatting.Instance.DisplaySystemMessage(message);

        selectButton.gameObject.SetActive(false);

        if (nightTime == 0)
        {
            selectButton.gameObject.SetActive(false);
        }
    }

    public void OnNightTimeEnd()
    {
        Player investigateTarget = CheckVotes();

        if (investigateTarget != null)
        {
            StalkerAction(investigateTarget);
        }
        else
        {
            return;
        }
    }

    private Player CheckVotes()
    {
        Player localPlayer = PhotonNetwork.LocalPlayer;

        if (!localPlayer.CustomProperties.ContainsKey("Job") || !localPlayer.CustomProperties["Job"].Equals("����Ŀ"))
        {
            return null;
        }

        Player selectedPlayer = GetSelectedPlayer();

        if (selectedPlayer != null && !selectedPlayer.CustomProperties.ContainsKey("isDead") || !((bool)selectedPlayer.CustomProperties["isDead"]))
        {
            return selectedPlayer;
        }

        return null;
    }

    private void StalkerAction(Player targetPlayer)
    {
        if (targetPlayer.CustomProperties.ContainsKey("nightAction"))
        {
            string nightAction = (string)targetPlayer.CustomProperties["nightAction"];
            string selectedPlayerNickName = null;

            switch (nightAction)
            {
                case "Gangster":
                    selectedPlayerNickName = (string)targetPlayer.CustomProperties["GangsterSelectedPlayer"];
                    break;
                case "Police":
                    selectedPlayerNickName = (string)targetPlayer.CustomProperties["PoliceSelectedPlayer"];
                    break;
                case "Doctor":
                    selectedPlayerNickName = (string)targetPlayer.CustomProperties["DoctorSelectedPlayer"];
                    break;
                case "Mafia":
                    selectedPlayerNickName = (string)targetPlayer.CustomProperties["MafiaSelectedPlayer"];
                    break;
                default:
                    Debug.LogWarning("Unknown night action: " + nightAction);
                    break;
            }

            if (selectedPlayerNickName != null)
            {
                string message = $"[�ý���]{targetPlayer.NickName}���� ���� �㿡 <color=green>{selectedPlayerNickName}<color=white>���� �湮�߽��ϴ�!";
                StalkerChatting.Instance.DisplaySystemMessage(message);
            }
        }

        else
        {
            string message = $"[�ý���]{targetPlayer.NickName}���� ���� �� �ƹ��� �ൿ�� ���� �ʾҽ��ϴ�.";

            StalkerChatting.Instance.DisplaySystemMessage(message);
        }
    }
}