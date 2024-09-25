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
        // �ִ� ��ǥ���� inputField�� true -> �̰� InGameChatting���� �����;�¡.....

        TimeSlider.Instance.FinalAppealPhase();
        yield return new WaitForSeconds(30);

        InGameChatting.Instance.FinalAppealSystemMessage();
        TimeSlider.Instance.StartVotePhase();

        // ó�� �޽����� Ŀ���� ������Ƽ �߰� + �װ� �޾Ƽ� ���
    }

    public bool CheckGameEndConditions()
    {
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.CustomProperties == null)
        {
            Debug.LogError("Room�̳� CustomProperties�� null�Դϴ�.");
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
            Debug.LogError("MafiaCount�� �������� �ʾҽ��ϴ�.");
        }

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("GangsterCount"))
        {
            gangsterCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["GangsterCount"];
        }
        else
        {
            Debug.LogError("GangsterCount�� �������� �ʾҽ��ϴ�.");
        }

        if (PhotonNetwork.CurrentRoom != null)
        {
            playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        }
        else
        {
            Debug.LogError("Room�� null�Դϴ�.");
        }

        if (mafiaCount + gangsterCount == 0)
        {
            EndGame("[�ý���]<color=blue>�ù��� <color=white>�¸�!");
            return true;
        }
        else if (mafiaCount + gangsterCount >= playerCount)
        {
            EndGame("[�ý���]<color=red>���Ǿ��� <color=white>�¸�!");
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
        // ���� Ȱ�� �ʱ�ȭ ����
        // nightAction Ŀ���� ������Ƽ null
    }

    public void ResetVoting()
    {
        // ��ǥ �ʱ�ȭ ����
        // ��ǥ Ŀ���� ������Ƽ ���� �� null�� ����
    }
}