using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using Hashtable = ExitGames.Client.Photon.Hashtable;

// ��� �гο� �Ҵ��� �� / citizen�� �׳� �ƹ� ��Ӵٿ� ������ �־������
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
        // �ִ� ��ǥ���� inputField�� true

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
        // ���� ���� ó�� ����
    }

    private void ResetRoleActions()
    {
        // ���� Ȱ�� �ʱ�ȭ ����
    }

    private void ResetVoting()
    {
        // ��ǥ �ʱ�ȭ ����
    }
}