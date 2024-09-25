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
        CheckGameEndConditions();

        chattingInput.interactable = false;
        roleAction.gameObject.SetActive(true);

        TimeSlider.Instance.slider.gameObject.SetActive(true);
        TimeSlider.Instance.StartNightPhase();

        while (TimeSlider.Instance.timeRemaining > 0)
        {
            yield return null;
        }

        roleAction.gameObject.SetActive(false);
        ResetRoleActions();
    }
}