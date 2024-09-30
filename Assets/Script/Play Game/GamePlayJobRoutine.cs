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
        InGameChatting.Instance.DisplaySystemMessage("[시스템]밤이 찾아왔습니다...");

        voteButton.gameObject.SetActive(false);
        chattingInput.interactable = false;
        roleAction.gameObject.SetActive(true);

        TimeSlider.Instance.slider.gameObject.SetActive(true);
        TimeSlider.Instance.StartTimer("NightTime");
        yield return new WaitForSeconds(nightTime);

        JobProcess();
        ResetRoleActions();

        yield return StartCoroutine(DayPhase());
    }
}