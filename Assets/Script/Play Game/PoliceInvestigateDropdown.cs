using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Linq;

public class PoliceInvestigateDropdown : MonoBehaviourPunCallbacks
{
    public static PoliceInvestigateDropdown Instance { get; private set; }

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

        if (localPlayer.CustomProperties.ContainsKey("Job") && localPlayer.CustomProperties["Job"].Equals("����"))
        {
            Player selectedPlayer = GetSelectedPlayer();

            if (selectedPlayer != null)
            {
                Hashtable policeAction = new Hashtable
            {
                { "nightAction", "Police" },
                { "PoliceSelectedPlayer", selectedPlayer.NickName }
            };

                PhotonNetwork.LocalPlayer.SetCustomProperties(policeAction);

                string message = $"[�ý���]{PhotonNetwork.LocalPlayer.NickName}���� <color=green>{selectedPlayer.NickName}<color=white>���� �����մϴ�...";

                PoliceChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_Police", message);

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
        Player selectedTarget = CheckVotes();
        if (selectedTarget != null)
        {
            PoliceAction(selectedTarget);
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

        foreach (Player police in PhotonNetwork.PlayerList)
        {
            if (police.CustomProperties.ContainsKey("PoliceSelectedPlayer"))
            {
                string selectedPlayerName = (string)police.CustomProperties["PoliceSelectedPlayer"];
                Player selectedPlayer = PhotonNetwork.PlayerListOthers.FirstOrDefault(p => p.NickName == selectedPlayerName);

                if (selectedPlayer == null)
                {
                    string message = ($"[�ý���]���� ���� �ƹ��� �������� �ʾҽ��ϴ�.");

                    PoliceChatting.Instance.DisplaySystemMessage(message);

                    continue;
                }

                if (lastSelectedPlayer == null)
                {
                    lastSelectedPlayer = selectedPlayer;
                }
                else
                {
                    if (lastSelectedPlayer != selectedPlayer)
                    {
                        allVotesMatch = false;

                        string message = ($"[�ý���]���� ���� �ƹ��� �������� �ʾҽ��ϴ�.");

                        PoliceChatting.Instance.DisplaySystemMessage(message);

                        break;
                    }
                }
            }
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