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

    public IEnumerator GameLoop()
    {
        yield return new WaitForSeconds(5f);

        while (true)
        {
            yield return StartCoroutine(NightPhase());

            // 밤 / 낮 사이 직업 공개 시간

            yield return StartCoroutine(DayPhase());

            if (isFinalAppeal)
            {
                yield return StartCoroutine(FinalAppealPhase());
            }
        }
    }

    public virtual IEnumerator NightPhase()
    {
        TimeSlider.Instance.slider.gameObject.SetActive(true);
        TimeSlider.Instance.StartNightPhase();

        CheckGameEndConditions();

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

        CheckGameEndConditions();

        chattingInput.interactable = true;
        voteButton.gameObject.SetActive(true);

        yield return new WaitForSeconds(dayTime);

        ResetVoting();
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
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.CustomProperties == null)
        {
            Debug.LogError("Room이나 CustomProperties가 null입니다.");
            return false;
        }

        int mafiaCount = 0;
        int gangsterCount = 0;
        int playerCount = 0;

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("MafiaCount"))
        {
            mafiaCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["MafiaCount"];
        }
        else
        {
            Debug.LogError("MafiaCount가 설정되지 않았습니다.");
        }

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("GangsterCount"))
        {
            gangsterCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["GangsterCount"];
        }
        else
        {
            Debug.LogError("GangsterCount가 설정되지 않았습니다.");
        }

        if (PhotonNetwork.CurrentRoom != null)
        {
            playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        }
        else
        {
            Debug.LogError("Room이 null입니다.");
        }

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

        for (int i = 0; i < 8; i++)
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