using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GamePlayRoutine : MonoBehaviour
{
    protected int dayTime;
    protected int nightTime;
    protected bool isFinalAppeal;

    public Transform[] gamePanel;
    public TMP_InputField chattingInput;
    public Button voteButton;

    public void Start()
    {
        Hashtable roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
        dayTime = (int)roomProperties["DayTime"];
        nightTime = (int)roomProperties["NightTime"];
        isFinalAppeal = (bool)roomProperties["FinalAppeal"];

        StartCoroutine(GameLoop());
    }

    private void Update()
    {
        CheckGameEndConditions();
    }

    public IEnumerator GameLoop()
    {
        yield return new WaitForSeconds(5f);
        yield return StartCoroutine(NightPhase());
    }

    public virtual IEnumerator NightPhase()
    {
        InGameChatting.Instance.DisplaySystemMessage("[�ý���]���� ã�ƿԽ��ϴ�...");

        voteButton.gameObject.SetActive(false);

        chattingInput.interactable = false;

        TimeSlider.Instance.slider.gameObject.SetActive(true);
        TimeSlider.Instance.StartTimer("NightTime");
        yield return new WaitForSeconds(nightTime);

        JobProcess();
        ResetRoleActions();

        TimeSlider.Instance.slider.gameObject.SetActive(false);

        yield return StartCoroutine(DayPhase());
    }

    public IEnumerator DayPhase()
    {
        InGameChatting.Instance.DisplaySystemMessage("[�ý���]���� �Ǿ����ϴ�.");
        InGameChatting.Instance.DisplaySystemMessage("[�ý���]�߸��� ����� ȸ�Ǹ� ���� ��ǥ�� �����ϼ���.");

        if (dayTime > 0)
        {
            TimeSlider.Instance.slider.gameObject.SetActive(true);
            TimeSlider.Instance.StartTimer("DayTime");
        }
        else
        {
            TimeSlider.Instance.slider.gameObject.SetActive(false);
        }

        chattingInput.interactable = true;
        voteButton.gameObject.SetActive(true);

        float timer = dayTime;
        while (timer > 0)
        {
            if (InGamePlayerDropdown.Instance.AllVote)
            {
                InGamePlayerDropdown.Instance.CalculateVoteResults();

                if (isFinalAppeal == true)
                {
                    yield return StartCoroutine(FinalAppealPhase());
                }

                else
                {
                    yield return StartCoroutine(NightPhase());
                }

                yield break;
            }

            timer -= Time.deltaTime;

            yield return null;
        }

        InGamePlayerDropdown.Instance.CalculateVoteResults();
        ResetVoting();
    }

    public void JobProcess()
    {
        if (MafiaKillDropdown.Instance != null)
            MafiaKillDropdown.Instance.OnNightTimeEnd();

        if (GangsterInvestigateDropdown.Instance != null)
            GangsterInvestigateDropdown.Instance.OnNightTimeEnd();

        if (DoctorCureDropdown.Instance != null)
            DoctorCureDropdown.Instance.OnNightTimeEnd();

        if (PoliceInvestigateDropdown.Instance != null)
            PoliceInvestigateDropdown.Instance.OnNightTimeEnd();

        if (StalkerInvestigateDropdown.Instance != null)
            StalkerInvestigateDropdown.Instance.OnNightTimeEnd();
    }

    public IEnumerator FinalAppealPhase()
    {
        InGameChatting.Instance.DisplaySystemMessage("[�ý���]��ǥ ����. ���� ������ �����մϴ�.");
        InGameChatting.Instance.DisplaySystemMessage("[�ý���]������ ��� �ڽ��� �ǰ߰� ��ġ�ϴ� ��ư�� ��������.");

        chattingInput.interactable = false;

        TimeSlider.Instance.StartTimer(30);
        yield return new WaitForSeconds(30);

        InGameChatting.Instance.FinalAppealSystemMessage();

        TimeSlider.Instance.StartTimer(10);
        yield return new WaitForSeconds(10);
        FinalAppealSystem.Instance.CalculateFinalAppeal();

        yield return StartCoroutine(NightPhase());
    }

    public bool CheckGameEndConditions()
    {
        int aliveMafiaTeamCount = 0;
        int alivePlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.ContainsKey("Job"))
            {
                string job = (string)player.CustomProperties["Job"];

                if (job == "���Ǿ�" || job == "�Ǵ�")
                {
                    aliveMafiaTeamCount++;
                }
            }

            if (player.CustomProperties.ContainsKey("isDead") && (bool)player.CustomProperties["isDead"])
            {
                string job = (string)player.CustomProperties["Job"];

                if (job == "���Ǿ�" || job == "�Ǵ�")
                {
                    aliveMafiaTeamCount--;
                }

                else
                {
                    alivePlayerCount--;
                }
            }
        }

        if (aliveMafiaTeamCount == 0)
        {
            EndGame("[�ý���]<color=blue>�ù��� <color=white>�¸�!");
            return true;
        }

        else if (aliveMafiaTeamCount >= alivePlayerCount - aliveMafiaTeamCount)
        {
            EndGame("[�ý���]<color=red>���Ǿ��� <color=white>�¸�!");
            return true;
        }

        return false;
    }

    public void EndGame(string message)
    {
        TimeSlider.Instance.slider.gameObject.SetActive(false);

        for (int i = 0; i < 7; i++)
        {
            gamePanel[i].gameObject.SetActive(false);
        }

        LobbyChatting.Instance.DisplaySystemMessage(message);
    }

    public void ResetRoleActions()
    {
        Hashtable porps = new Hashtable
        {
            { "nightAction", null }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(porps);
    }

    public void ResetVoting()
    {
        Hashtable votingProperties = new Hashtable
        {
            { "votedPlayer", null }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(votingProperties);
    }
}