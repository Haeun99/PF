using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Linq;

public class DoctorCureDropdown : MonoBehaviourPunCallbacks
{
    public static DoctorCureDropdown Instance { get; private set; }

    public TMP_Dropdown playerDropdown;
    public Button selectButton;

    private int voteEnd;

    private int nightTime;

    public List<Player> players = new List<Player>();

    public Dictionary<Player, Player> doctorVotes = new Dictionary<Player, Player>();

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

    private void Update()
    {
        if (nightTime == 0)
        {
            selectButton.gameObject.SetActive(false);
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

        if (localPlayer.CustomProperties.ContainsKey("Job") && localPlayer.CustomProperties["Job"].Equals("의사"))
        {
            Player selectedPlayer = GetSelectedPlayer();

            if (selectedPlayer != null)
            {
                Hashtable doctorAction = new Hashtable
                {
                    { "nightAction", "Doctor" },
                    { "DoctorSelectedPlayer", selectedPlayer.NickName }
                };

                PhotonNetwork.LocalPlayer.SetCustomProperties(doctorAction);

                DoctorAction(selectedPlayer);

                string message = $"[시스템]{PhotonNetwork.LocalPlayer.NickName}님이 <color=green>{selectedPlayer.NickName}<color=white>님을 치료합니다...";
                DoctorChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_Doctor", message);

                selectButton.gameObject.SetActive(false);
            }
        }
    }

    public IEnumerator OnNightTimeEnd()
    {
        Player selectedTarget = CheckVotes();
        if (selectedTarget != null)
        {
            DoctorAction(selectedTarget);
        }
        else
        {
            yield return null;
        }
    }

    public Player CheckVotes()
    {
        Player lastSelectedPlayer = null;
        bool allVotesMatch = true;
        bool hasVotes = false;

        foreach (Player doctor in PhotonNetwork.PlayerList)
        {
            if (!doctor.CustomProperties.ContainsKey("DoctorSelectedPlayer"))
            {
                continue;
            }

            hasVotes = true;
            string selectedPlayerName = (string)doctor.CustomProperties["DoctorSelectedPlayer"];
            Player selectedPlayer = PhotonNetwork.PlayerListOthers.FirstOrDefault(p => p.NickName == selectedPlayerName);

            if (lastSelectedPlayer == null)
            {
                lastSelectedPlayer = selectedPlayer;
            }
            else
            {
                if (lastSelectedPlayer != selectedPlayer)
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

    public void DoctorAction(Player targetPlayer)
    {
        PlayerStatus.Instance.SetDead(targetPlayer, false);
    }
}