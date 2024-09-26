using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class FinalAppealSystem : MonoBehaviour
{
    public static FinalAppealSystem Instance { get; private set; }

    public Button finalKillButton;
    public Button finalSaveButton;

    private int voteTime = 10;
    private Player mostVotedPlayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        finalKillButton.onClick.AddListener(finalKillButtonClick);
        finalSaveButton.onClick.AddListener(finalSaveButtonClick);

        StartCoroutine(CountdownAppealTime());
    }

    public void SetMostVotedPlayer(Player player)
    {
        mostVotedPlayer = player;
    }

    public void finalKillButtonClick()
    {
        Hashtable props = new Hashtable
        {
            { "finalAction", "Kill" }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        DisableButtons();
    }

    public void finalSaveButtonClick()
    {
        Hashtable props = new Hashtable
        {
            { "finalAction", "Save" }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        DisableButtons();
    }

    private void DisableButtons()
    {
        finalKillButton.interactable = false;
        finalSaveButton.interactable = false;
    }

    private IEnumerator CountdownAppealTime()
    {
        yield return new WaitForSeconds(voteTime);

        DisableButtons();
    }

    public void CalculateFinalAppeal()
    {
        int killVotes = 0;
        int saveVotes = 0;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.ContainsKey("isDead") && (bool)player.CustomProperties["isDead"])
            {
                DisableButtons();
                continue;
            }

            if (player.CustomProperties.ContainsKey("finalAction"))
            {
                string action = (string)player.CustomProperties["finalAction"];
                if (action == "Kill")
                {
                    killVotes++;
                }
                else if (action == "Save")
                {
                    saveVotes++;
                }
            }
            else
            {
                saveVotes++;
            }
        }

        if (killVotes > saveVotes)
        {
            ExecuteKill();
        }
        else if (killVotes == saveVotes)
        {
            TieSave();
        }
        else
        {
            ExecuteSave();
        }

        ResetFinalActions();
    }

    private void ExecuteKill()
    {
        if (mostVotedPlayer != null)
        {
            PlayerStatus.Instance.SetDead(true);
            mostVotedPlayer.SetCustomProperties(new Hashtable { { "isDead", true } });

            InGameChatting.Instance.DisplaySystemMessage($"[�ý���]{mostVotedPlayer.NickName}���� ���� ó���Ǿ����ϴ�.");
        }

        InGameChatting.Instance.DisplaySystemMessage("[�ý���]���� �Ǿ����ϴ�.");
    }

    private void ExecuteSave()
    {
        InGameChatting.Instance.DisplaySystemMessage("[�ý���]��ǥ ���, �ټ���� ���� �����߽��ϴ�.");
        InGameChatting.Instance.DisplaySystemMessage("[�ý���]���� �Ǿ����ϴ�.");
    }

    private void TieSave()
    {
        InGameChatting.Instance.DisplaySystemMessage("[�ý���]��ǥ ���, ������ ���� �����߽��ϴ�.");
        InGameChatting.Instance.DisplaySystemMessage("[�ý���]���� �Ǿ����ϴ�.");
    }

    private void ResetFinalActions()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            Hashtable props = new Hashtable
            {
                { "finalAction", null }
            };
            player.SetCustomProperties(props);
        }
    }
}