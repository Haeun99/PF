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
    public Coroutine gameLoopCoroutine;

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

        gameLoopCoroutine = StartCoroutine(GameLoop());
    }

    private void Update()
    {
        CheckGameEndConditions();
    }

    public IEnumerator GameLoop()
    {
        yield return new WaitForSeconds(5f);

        while (true)
        {
            yield return StartCoroutine(NightPhase());

            JobProcess();

            yield return StartCoroutine(DayPhase());

            InGamePlayerDropdown.Instance.CalculateVoteResults();

            if (isFinalAppeal)
            {
                yield return StartCoroutine(FinalAppealPhase());
            }
        }
    }

    public virtual IEnumerator NightPhase()
    {
        voteButton.gameObject.SetActive(false);

        chattingInput.interactable = false;

        TimeSlider.Instance.slider.gameObject.SetActive(true);
        TimeSlider.Instance.StartTimer("NightTime");

        while (TimeSlider.Instance.timeRemaining > 0)
        {
            yield return null;
        }

        ResetRoleActions();

        TimeSlider.Instance.slider.gameObject.SetActive(false);
    }

    public IEnumerator DayPhase()
    {
        TimeSlider.Instance.slider.gameObject.SetActive(true);
        TimeSlider.Instance.StartTimer("DayTime");

        chattingInput.interactable = true;
        voteButton.gameObject.SetActive(true);

        yield return new WaitForSeconds(dayTime);

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
        chattingInput.interactable = false;

        TimeSlider.Instance.StartTimer(30);
        yield return new WaitForSeconds(30);

        InGameChatting.Instance.FinalAppealSystemMessage();

        TimeSlider.Instance.StartTimer(10);
        FinalAppealSystem.Instance.CalculateFinalAppeal();
    }

    public bool CheckGameEndConditions()
    {
        int mafiaCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["MafiaCount"];
        int gangsterCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["GangsterCount"];
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        if (mafiaCount + gangsterCount == 0)
        {
            EndGame("[½Ã½ºÅÛ]<color=blue>½Ã¹ÎÆÀ <color=white>½Â¸®!");
            return true;
        }
        else if (mafiaCount + gangsterCount >= playerCount)
        {
            EndGame("[½Ã½ºÅÛ]<color=red>¸¶ÇÇ¾ÆÆÀ <color=white>½Â¸®!");
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

        string roleRevealMessage = GetRoleRevealMessage();
    }

    private string GetRoleRevealMessage()
    {
        string roleRevealMessage = "[½Ã½ºÅÛ]Á÷¾÷:\n";

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.ContainsKey("Job"))
            {
                string Job = (string)player.CustomProperties["Job"];
                string nickname = player.NickName;

                roleRevealMessage += $"{nickname} : {Job}\n";
            }
        }

        return roleRevealMessage;
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