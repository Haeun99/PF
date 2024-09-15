using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MafiaRoomSetting : MonoBehaviour
{
    public static MafiaRoomSetting Instance { get; private set; }

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

    private int dayTimeSecond;
    private int nightTimeSecond;
    private bool isNoLimitDay;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        settingConfirmButton.onClick.AddListener(SettingConfirm);
        dayTime.onValueChanged.AddListener(SetDayTime);
        nightTime.onValueChanged.AddListener(SetNightTime);

        UpdateDayText();
        UpdateNightText();
    }

    public void SettingConfirm()
    {

    }

    private void SetDayTime(int option)
    {
        switch (option)
        {
            case 0:
                dayTimeSecond = 60;
                isNoLimitDay = false;
                break;
            case 1:
                dayTimeSecond = 90;
                isNoLimitDay = false;
                break;
            case 2:
                dayTimeSecond = 120;
                isNoLimitDay = false;
                break;
            case 3:
                dayTimeSecond = 150;
                isNoLimitDay = false;
                break;
            case 4:
                isNoLimitDay = true;
                dayTimeSecond = -1;
                break;
        }

        UpdateDayText();
    }

    private void UpdateDayText()
    {
        if (isNoLimitDay)
        {
            dayTimeText.text = "≥∑ Ω√∞£: ¡¶«— æ¯¿Ω";
        }
        else
        {
            int minutes = dayTimeSecond / 60;
            int seconds = dayTimeSecond % 60;

            if (seconds == 0)
            {
                dayTimeText.text = $"≥∑ Ω√∞£: {minutes}∫–";
            }
            else
            {
                dayTimeText.text = $"≥∑ Ω√∞£: {minutes}∫– {seconds}√ ";
            }
        }
    }

    private void SetNightTime(int option)
    {
        switch (option)
        {
            case 0:
                nightTimeSecond = 30;
                break;
            case 1:
                nightTimeSecond = 60;
                break;
            case 2:
                nightTimeSecond = 90;
                break;
            case 3:
                nightTimeSecond = 120;
                break;
        }

        UpdateNightText();
    }

    private void UpdateNightText()
    {
        int minutes = nightTimeSecond / 60;
        int seconds = nightTimeSecond % 60;

        if (seconds == 0)
        {
            nightTimeText.text = $"π„ Ω√∞£: {minutes}∫–";
        }
        else
        {
            nightTimeText.text = $"π„ Ω√∞£: {minutes}∫– {seconds}√ ";
        }
    }
}