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

            yield return StartCoroutine(JobProcess());

            yield return StartCoroutine(DayPhase());

            if (isFinalAppeal)
            {
                yield return StartCoroutine(FinalAppealPhase());
            }
        }
    }

    public virtual IEnumerator NightPhase()
    {
        voteButton.gameObject.SetActive(false);

        TimeSlider.Instance.slider.gameObject.SetActive(true);
        TimeSlider.Instance.StartNightPhase();

        chattingInput.interactable = false;

        TimeSlider.Instance.slider.gameObject.SetActive(true);
        TimeSlider.Instance.StartNightPhase();

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
        TimeSlider.Instance.StartDayPhase();

        chattingInput.interactable = true;
        voteButton.gameObject.SetActive(true);

        yield return new WaitForSeconds(dayTime);

        ResetVoting();
    }

    public IEnumerator JobProcess()
    {


        yield return new WaitForSeconds(5);
    }

    public IEnumerator FinalAppealPhase()
    {
        chattingInput.interactable = false;
        // 최다 득표자의 inputField만 true -> 이건 InGameChatting에서 가져와야징.....

        TimeSlider.Instance.FinalAppealPhase();
        yield return new WaitForSeconds(30);

        InGameChatting.Instance.FinalAppealSystemMessage();
        TimeSlider.Instance.StartVotePhase();

        // 처형 메시지에 커스텀 프로퍼티 추가 + 그거 받아서 계수
    }

    public bool CheckGameEndConditions()
    {
        int mafiaCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["MafiaCount"];
        int gangsterCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["GangsterCount"];
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        if (mafiaCount + gangsterCount == 0)
        {
            EndGame("[시스템]<color=blue>시민팀 <color=white>승리!");
            return true;
        }
        else if (mafiaCount + gangsterCount >= playerCount)
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
        // 역할 활동 초기화 로직
        // nightAction 커스텀 프로퍼티 null
    }

    public void ResetVoting()
    {
        // 투표 초기화 로직
        // 투표 커스텀 프로퍼티 생성 후 null로 변경
    }
}