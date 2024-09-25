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
    public Button voteButton;

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

            // �� / �� ���� ���� ���� �ð�

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

        yield return new WaitForSeconds(nightTime);

        ResetRoleActions();

        TimeSlider.Instance.slider.gameObject.SetActive(false);
    }

    public IEnumerator DayPhase()
    {
        TimeSlider.Instance.slider.gameObject.SetActive(true);
        TimeSlider.Instance.StartDayPhase();

        CheckGameEndConditions();

        chattingInput.interactable = true;
        voteButton.interactable = true;

        yield return new WaitForSeconds(dayTime);

        voteButton.interactable = false;
        ResetVoting();
    }

    public IEnumerator FinalAppealPhase()
    {
        chattingInput.interactable = false;
        // �ִ� ��ǥ���� inputField�� true -> �̰� InGameChatting���� �����;�¡.....

        TimeSlider.Instance.FinalAppealPhase();
        yield return new WaitForSeconds(30);

        InGameChatting.Instance.FinalAppealSystemMessage();
        TimeSlider.Instance.StartVotePhase();

        // ó�� �޽����� Ŀ���� ������Ƽ �߰� + �װ� �޾Ƽ� ���
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
            EndGame("�ù����� �¸��߽��ϴ�!");
        }

        else if (aliveMafiaCount + aliveGangsterCount >= aliveNonMafiaPlayers)
        {
            EndGame("���Ǿ����� �¸��߽��ϴ�!");
        }
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
        // ���� Ȱ�� �ʱ�ȭ ����
        // nightAction Ŀ���� ������Ƽ null
    }

    public void ResetVoting()
    {
        // ��ǥ �ʱ�ȭ ����
        // ��ǥ Ŀ���� ������Ƽ ���� �� null�� ����
    }
}