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

    protected float dayTime;
    protected float nightTime;
    protected bool isFinalAppeal;

    public Transform[] gamePanel;
    public TMP_InputField chattingInput;
    public TMP_Dropdown voteDropdown;

    public void Start()
    {
        Hashtable roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
        dayTime = (float)roomProperties["DayTime"];
        nightTime = (float)roomProperties["NightTime"];
        isFinalAppeal = (bool)roomProperties["FinalAppeal"];

        gameLoopCoroutine = StartCoroutine(GameLoop());
    }

    public IEnumerator GameLoop()
    {
        yield return new WaitForSeconds(5f);

        while (true)
        {
            yield return StartCoroutine(NightPhase());

            yield return StartCoroutine(DayPhase());

            if (isFinalAppeal)
            {
                yield return StartCoroutine(FinalAppealPhase());
            }
        }
    }

    public virtual IEnumerator NightPhase()
    {
        CheckGameEndConditions();

        chattingInput.interactable = false;

        yield return new WaitForSeconds(nightTime);

        ResetRoleActions();
    }

    public IEnumerator DayPhase()
    {
        CheckGameEndConditions();

        chattingInput.interactable = true;
        voteDropdown.interactable = true;

        yield return new WaitForSeconds(dayTime);

        voteDropdown.interactable = false;
        ResetVoting();
    }

    public IEnumerator FinalAppealPhase()
    {
        chattingInput.interactable = false;
        // 최다 득표자의 inputField만 true -> 이건 InGameChatting에서 가져와야징.....

        yield return new WaitForSeconds(30);

        InGameChatting.Instance.FinalAppealSystemMessage();

        // 처형 메시지에 커스텀 프로퍼티 추가 + 그거 받아서 계수
    }

    public void CheckGameEndConditions()
    {
        int aliveMafiaCount = 0;
        int aliveGangsterCount = 0;
        int aliveNonMafiaPlayers = 0;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.ContainsKey("isDead") && (bool)player.CustomProperties["isDead"])
            {
                continue;
            }

            string job = player.CustomProperties["job"].ToString();
            if (job == "Mafia")
            {
                aliveMafiaCount++;
            }
            else if (job == "Gangster")
            {
                aliveGangsterCount++;
            }
            else
            {
                aliveNonMafiaPlayers++;
            }
        }

        if (aliveMafiaCount + aliveGangsterCount == 0)
        {
            EndGame("시민팀이 승리했습니다!");
        }

        else if (aliveMafiaCount + aliveGangsterCount >= aliveNonMafiaPlayers)
        {
            EndGame("마피아팀이 승리했습니다!");
        }
    }

    public void EndGame(string message)
    {
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