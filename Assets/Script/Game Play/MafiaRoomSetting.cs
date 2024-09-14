using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MafiaRoomSetting : MonoBehaviour
{
    public RectTransform masterSetting;
    public TMP_Dropdown dayTime;
    public TMP_Dropdown nightTime;
    public Toggle finalAppeal;
    public Toggle anonymousVote;
    public TMP_Dropdown mafiaCount;
    public TMP_Dropdown gangsterCount;
    public TMP_Dropdown doctorCount;
    public TMP_Dropdown policeCount;
    public TMP_Dropdown stalkerCount;
    public Button settingConfirmButton;

    [Space(20)]
    public TextMeshProUGUI dayTimeText;
    public TextMeshProUGUI nightTimeText;
    public TextMeshProUGUI finalAppealText;
    public TextMeshProUGUI anonymousVoteText;
    public TextMeshProUGUI mafiaCountText;
    public TextMeshProUGUI gangsterCountText;
    public TextMeshProUGUI doctorCountText;
    public TextMeshProUGUI policeCountText;
    public TextMeshProUGUI stalkerCountText;

    private void Start()
    {
        settingConfirmButton.onClick.AddListener(SettingConfirm);
    }

    public void SettingConfirm()
    {

    }
}
