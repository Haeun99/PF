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
        InGameChatting.Instance.DisplaySystemMessage("[시스템]밤이 찾아왔습니다...");

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
        InGameChatting.Instance.DisplaySystemMessage("[시스템]낮이 되었습니다.");
        InGameChatting.Instance.DisplaySystemMessage("[시스템]추리와 충분한 회의를 통해 투표를 진행하세요.");

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
        InGameChatting.Instance.DisplaySystemMessage("[시스템]투표 마감. 최후 변론을 시작합니다.");
        InGameChatting.Instance.DisplaySystemMessage("[시스템]변론을 듣고 자신의 의견과 일치하는 버튼을 누르세요.");

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

                if (job == "마피아" || job == "건달")
                {
                    aliveMafiaTeamCount++;
                }
            }

            if (player.CustomProperties.ContainsKey("isDead") && (bool)player.CustomProperties["isDead"])
            {
                string job = (string)player.CustomProperties["Job"];

                if (job == "마피아" || job == "건달")
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
            EndGame("[시스템]<color=blue>시민팀 <color=white>승리!");
            return true;
        }

        else if (aliveMafiaTeamCount >= alivePlayerCount - aliveMafiaTeamCount)
        {
            EndGame("[시스템]<color=red>마피아팀 <color=white>승리!");
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