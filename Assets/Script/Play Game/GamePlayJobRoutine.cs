using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayJobRoutine : GamePlayRoutine
{
    public Button roleAction;

    public override IEnumerator NightPhase()
    {
        ResetRoleActions();
        ResetVoting();

        InGameChatting.Instance.DisplaySystemMessage("[�ý���]���� ã�ƿԽ��ϴ�...");

        voteButton.gameObject.SetActive(false);
        chattingInput.interactable = false;
        roleAction.gameObject.SetActive(true);

        TimeSlider.Instance.slider.gameObject.SetActive(true);
        TimeSlider.Instance.StartTimer("NightTime");
        yield return new WaitForSeconds(nightTime);

        TimeSlider.Instance.slider.gameObject.SetActive(false);

        yield return StartCoroutine(DayPhase());
    }
}