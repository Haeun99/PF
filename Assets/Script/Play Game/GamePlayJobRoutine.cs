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
        roleAction.interactable = true;

        yield return new WaitForSeconds(nightTime);

        roleAction.interactable = false;
        ResetRoleActions();
    }
}