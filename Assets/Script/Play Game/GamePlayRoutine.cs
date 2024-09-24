using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using Hashtable = ExitGames.Client.Photon.Hashtable;

// 모든 패널에 할당할 것 / citizen은 그냥 아무 드롭다운 만들어다 넣어놓을것
public class GamePlayRoutine : MonoBehaviour
{
    private Coroutine gameLoopCoroutine;

    private float dayTimeSeconds;
    private float nightTimeSeconds;
    private bool isFinalAppeal;

    public TMP_InputField chattingInput;
    public TMP_Dropdown voteDropdown;
    public TMP_Dropdown roleAction;

    private void Start()
    {
        Hashtable roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
        dayTimeSeconds = (float)roomProperties["DayTime"];
        nightTimeSeconds = (float)roomProperties["NightTime"];
        isFinalAppeal = (bool)roomProperties["FinalAppeal"];

        gameLoopCoroutine = StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        while (true)
        {
            yield return StartCoroutine(NightPhase());

            yield return StartCoroutine(DayPhase());

            if (isFinalAppeal)
            {
                yield return StartCoroutine(FinalAppealPhase());
            }

            if (CheckGameEndConditions())
            {
                EndGame();
                yield break;
            }
        }
    }

    private IEnumerator NightPhase()
    {
        chattingInput.interactable = false;
        roleAction.interactable = true;

        yield return new WaitForSeconds(nightTimeSeconds);

        roleAction.interactable = false;
        ResetRoleActions();
    }

    private IEnumerator DayPhase()
    {
        chattingInput.interactable = true;
        voteDropdown.interactable = true;

        yield return new WaitForSeconds(dayTimeSeconds);

        voteDropdown.interactable = false;
        ResetVoting();
    }

    private IEnumerator FinalAppealPhase()
    {
        // 최다 득표자의 inputField만 true

        yield return new WaitForSeconds(30);
    }

    private bool CheckGameEndConditions()
    {
        int mafiaCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["MafiaCount"];
        int gangsterCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["GangsterCount"];
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        if (mafiaCount + gangsterCount == 0 || mafiaCount + gangsterCount > playerCount)
        {
            return true;
        }

        return false;
    }

    private void EndGame()
    {
        // 게임 종료 처리 로직
    }

    private void ResetRoleActions()
    {
        // 역할 활동 초기화 로직
    }

    private void ResetVoting()
    {
        // 투표 초기화 로직
    }
}