using System.Collections;
using System.Collections.Generic;
using TMPro;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class MafiaRoomSetting : MonoBehaviourPunCallbacks
{
    public static MafiaRoomSetting Instance { get; private set; }

    public RectTransform masterSetting;
    public RectTransform playerSetting;
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
    private bool isFinalAppeal = false;
    private bool isAnonymous = false;

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
            DontDestroyOnLoad(gameObject);
        }

        Init();

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

        if (PhotonNetwork.IsMasterClient)
        {
            SettingConfirm();
            UpdateFromRoomProperties();
        }

        else
        {
            SetDayTime(0);
            SetNightTime(0);
            UpdateCountSetting();

            UpdateDayText();
            UpdateNightText();
            UpdateFinalAppealText();
            UpdateVoteText();
            SetMafiaCount();
            SetGangsterCount();
            SetDoctorCount();
            SetPoliceCount();
            SetStalkerCount();
        }
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

        Hashtable roomProperties = new Hashtable
    {
        { "DayTime", dayTimeSecond },
        { "NightTime", nightTimeSecond },
        { "FinalAppeal", isFinalAppeal },
        { "AnonymousVote", isAnonymous },
        { "MafiaCount", mafiaNumber },
        { "GangsterCount", gangsterNumber },
        { "DoctorCount", doctorNumber },
        { "PoliceCount", policeNumber },
        { "StalkerCount", stalkerNumber }
    };

        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);

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

            dayTimeText.text = (seconds == 0) ? $"{minutes}분" : $"{minutes}분 {seconds}초";
        }
    }

    private void SetNightTime(int option)
    {
        switch (option)
        {
            case 0:
                nightTimeSecond = 60;
                break;
            case 1:
                nightTimeSecond = 90;
                break;
            case 2:
                nightTimeSecond = 120;
                break;
        }

        UpdateNightText();
    }

    private void UpdateNightText()
    {
        int minutes = nightTimeSecond / 60;
        int seconds = nightTimeSecond % 60;

        nightTimeText.text = (seconds == 0) ? $"{minutes}분" : $"{minutes}분 {seconds}초";
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

    public void UpdateCountSetting()
    {
        mafiaCountText.text = mafiaNumber == 0 ? "1명" : $"{mafiaNumber}명";
        gangsterCountText.text = gangsterNumber == 1 ? "있음" : "없음";
        doctorCountText.text = doctorNumber == 0 ? "없음" : $"{doctorNumber}명";
        policeCountText.text = policeNumber == 0 ? "없음" : $"{policeNumber}명";
        stalkerCountText.text = stalkerNumber == 1 ? "있음" : "없음";
    }

    // todo : 현재 인원으로 수정해야함
    public void ValidateRoleCount()
    {
        int totalPlayer = mafiaNumber + gangsterNumber + policeNumber + doctorNumber + stalkerNumber;
        int maxPlayer = PhotonNetwork.CurrentRoom.MaxPlayers;

        if (totalPlayer > maxPlayer || mafiaNumber + gangsterNumber > maxPlayer / 2)
        {
            settingConfirmButton.interactable = false;
        }

        else
        {
            settingConfirmButton.interactable = true;
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        UpdateFromRoomProperties();
    }

    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.InLobby)
        {
            ValidateRoleCount();
        }
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        UpdateFromRoomProperties();
    }

    public void UpdateFromRoomProperties()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("DayTime", out object dayTimeObj))
        {
            dayTimeSecond = (int)dayTimeObj;
            UpdateDayText();
        }

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("NightTime", out object nightTimeObj))
        {
            nightTimeSecond = (int)nightTimeObj;
            UpdateNightText();
        }

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("FinalAppeal", out object finalAppealObj))
        {
            isFinalAppeal = (bool)finalAppealObj;
            UpdateFinalAppealText();
        }

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("AnonymousVote", out object anonymousVoteObj))
        {
            isAnonymous = (bool)anonymousVoteObj;
            UpdateVoteText();
        }

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("MafiaCount", out object mafiaCountObj))
        {
            mafiaNumber = (int)mafiaCountObj;
            UpdateCountSetting();
        }

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("GangsterCount", out object gangsterCountObj))
        {
            gangsterNumber = (int)gangsterCountObj;
            UpdateCountSetting();
        }

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("DoctorCount", out object doctorCountObj))
        {
            doctorNumber = (int)doctorCountObj;
            UpdateCountSetting();
        }

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("PoliceCount", out object policeCountObj))
        {
            policeNumber = (int)policeCountObj;
            UpdateCountSetting();
        }

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("StalkerCount", out object stalkerCountObj))
        {
            stalkerNumber = (int)stalkerCountObj;
            UpdateCountSetting();
        }
    }

    public void Init()
    {
        dayTimeSecond = 60;
        nightTimeSecond = 60;
        isFinalAppeal = false;
        isAnonymous = false;
        mafiaNumber = 1;
        gangsterNumber = 0;
        doctorNumber = 0;
        policeNumber = 0;
        stalkerNumber = 0;
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);

        UpdateFromRoomProperties();
    }
}