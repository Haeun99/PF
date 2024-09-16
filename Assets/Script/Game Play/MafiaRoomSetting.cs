using System.Collections;
using System.Collections.Generic;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
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
    private bool isFinalAppeal;
    private bool isAnonymous;

    private int mafiaNumber;
    private int gangsterNumber;
    private int doctorNumber;
    private int policeNumber;
    private int stalkerNumber;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        settingConfirmButton.onClick.AddListener(SettingConfirm);
        dayTime.onValueChanged.AddListener(SetDayTime);
        nightTime.onValueChanged.AddListener(SetNightTime);
        finalAppeal.onValueChanged.AddListener(delegate { isFinalAppeal = finalAppeal.isOn; UpdateFinalAppealText(); });
        anonymousVote.onValueChanged.AddListener(delegate { isAnonymous = anonymousVote.isOn; UpdateVoteText(); });

        mafiaCount.onValueChanged.AddListener(delegate { SetMafiaCount(); UpdateCountSetting(); ValidateRoleCount(); });
        gangsterCount.onValueChanged.AddListener(delegate { SetGangsterCount(); UpdateCountSetting(); ValidateRoleCount(); });
        doctorCount.onValueChanged.AddListener(delegate { SetDoctorCount(); UpdateCountSetting(); ValidateRoleCount(); });
        policeCount.onValueChanged.AddListener(delegate { SetPoliceCount(); UpdateCountSetting(); ValidateRoleCount(); });
        stalkerCount.onValueChanged.AddListener(delegate { SetStalkerCount(); UpdateCountSetting(); ValidateRoleCount(); });

        SetDayTime(0);
        SetNightTime(0);
        UpdateCountSetting();
        ValidateRoleCount();
        SettingConfirm();
    }

    public void SettingConfirm()
    {
        UpdateDayText();
        UpdateNightText();
        UpdateFinalAppealText();
        UpdateVoteText();
        SetMafiaCount();
        SetGangsterCount();
        SetDoctorCount();
        SetPoliceCount();
        SetStalkerCount();

        masterSetting.gameObject.SetActive(false);
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
            dayTimeText.text = "제한 없음";
        }

        else
        {
            int minutes = dayTimeSecond / 60;
            int seconds = dayTimeSecond % 60;

            if (seconds == 0)
            {
                dayTimeText.text = $"{minutes}분";
            }

            else
            {
                dayTimeText.text = $"{minutes}분 {seconds}초";
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
            nightTimeText.text = $"{minutes}분";
        }
        else
        {
            nightTimeText.text = $"{minutes}분 {seconds}초";
        }
    }

    private void UpdateFinalAppealText()
    {
        finalAppealText.text = isFinalAppeal ? "허용" : "불가";
    }

    private void UpdateVoteText()
    {
        anonymousVoteText.text = isAnonymous ? "허용" : "불가";
    }

    private void SetMafiaCount()
    {
        mafiaNumber = mafiaCount.value + 1;
    }

    private void SetGangsterCount()
    {
        gangsterNumber = gangsterCount.value;
    }

    private void SetDoctorCount()
    {
        doctorNumber = doctorCount.value;
    }

    private void SetPoliceCount()
    {
        policeNumber = policeCount.value;
    }

    private void SetStalkerCount()
    {
        stalkerNumber = stalkerCount.value;
    }

    private void UpdateCountSetting()
    {
        mafiaCountText.text = $"{mafiaNumber}명";
        gangsterCountText.text = $"{(gangsterNumber == 1 ? "있음" : "없음")}";
        doctorCountText.text = $"{doctorNumber}명";
        policeCountText.text = $"{policeNumber}명";
        stalkerCountText.text = $"{(stalkerNumber == 1 ? "있음" : "없음")}";
    }

    private void ValidateRoleCount()
    {
        int totalPlayer = mafiaNumber + gangsterNumber + policeNumber + doctorNumber + stalkerNumber;
        int maxPlayer = PhotonNetwork.CurrentRoom.MaxPlayers;

        if (totalPlayer > maxPlayer)
        {
            settingConfirmButton.interactable = false;
            return;
        }

        if (mafiaNumber + gangsterNumber > maxPlayer / 2)
        {
            settingConfirmButton.interactable = false;
            return;
        }

        settingConfirmButton.interactable = true;
    }
}