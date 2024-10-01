using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class StartGame : MonoBehaviourPunCallbacks
{
    public static StartGame Instance { get; private set; }

    public RectTransform mafiaRole;
    public RectTransform gangsterRole;
    public RectTransform doctorRole;
    public RectTransform policeRole;
    public RectTransform stalkerRole;
    public RectTransform citizenRole;

    [Space(20)]
    public RectTransform mafiaText;
    public RectTransform gangsterText;
    public RectTransform doctorText;
    public RectTransform policeText;
    public RectTransform stalkerText;
    public RectTransform citizenText;

    private List<string> randomJob = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void StartButtonClick()
    {
        InitializeJobList();

        if (PhotonNetwork.IsMasterClient)
        {
            AssignJobsToPlayers();
        }
    }

    private void InitializeJobList()
    {
        randomJob.Clear();

        int mafiaCount = PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("MafiaCount") ? (int)PhotonNetwork.CurrentRoom.CustomProperties["MafiaCount"] : 1;
        int gangsterCount = PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("GangsterCount") ? (int)PhotonNetwork.CurrentRoom.CustomProperties["GangsterCount"] : 0;
        int doctorCount = PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("DoctorCount") ? (int)PhotonNetwork.CurrentRoom.CustomProperties["DoctorCount"] : 0;
        int policeCount = PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("PoliceCount") ? (int)PhotonNetwork.CurrentRoom.CustomProperties["PoliceCount"] : 0;
        int stalkerCount = PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("StalkerCount") ? (int)PhotonNetwork.CurrentRoom.CustomProperties["StalkerCount"] : 0;

        for (int i = 0; i < mafiaCount; i++) randomJob.Add("Mafia");
        for (int i = 0; i < gangsterCount; i++) randomJob.Add("Gangster");
        for (int i = 0; i < doctorCount; i++) randomJob.Add("Doctor");
        for (int i = 0; i < policeCount; i++) randomJob.Add("Police");
        for (int i = 0; i < stalkerCount; i++) randomJob.Add("Stalker");

        int totalPlayers = PhotonNetwork.PlayerList.Length;
        int assignedJobs = randomJob.Count;
        int citizenCount = totalPlayers - assignedJobs;

        for (int i = 0; i < citizenCount; i++) randomJob.Add("Citizen");
    }

    private void AssignJobsToPlayers()
    {
        List<Player> players = new List<Player>(PhotonNetwork.PlayerList);
        Dictionary<Player, string> playerJobs = new Dictionary<Player, string>();

        foreach (Player player in players)
        {
            int randomIndex = Random.Range(0, randomJob.Count);
            string assignedJob = randomJob[randomIndex];

            switch (assignedJob)
            {
                case "Mafia":
                    assignedJob = "마피아";
                    break;
                case "Gangster":
                    assignedJob = "건달";
                    break;
                case "Doctor":
                    assignedJob = "의사";
                    break;
                case "Police":
                    assignedJob = "경찰";
                    break;
                case "Stalker":
                    assignedJob = "스토커";
                    break;
                case "Citizen":
                    assignedJob = "시민";
                    break;
            }

            playerJobs[player] = assignedJob;
            randomJob.RemoveAt(randomIndex);
        }

        foreach (var pair in playerJobs)
        {
            Hashtable jobProperty = new Hashtable();
            jobProperty["Job"] = pair.Value;

            pair.Key.SetCustomProperties(jobProperty);
        }

        string jobAnnouncement = "[시스템]직업 배정:\n";

        foreach (var pair in playerJobs)
        {
            Debug.Log($"Player: {pair.Key.NickName}, Assigned Job: {pair.Value}");
            jobAnnouncement += $"{pair.Key.NickName} : {pair.Value}\n";
        }

        LobbyChatting.Instance.SendSystemMessage($"{PhotonNetwork.CurrentRoom.Name}_Lobby",jobAnnouncement);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        if (targetPlayer == PhotonNetwork.LocalPlayer && changedProps.ContainsKey("Job"))
        {
            string job = (string)changedProps["Job"];
            ShowJobUI(job);
        }
    }

    private void ShowJobUI(string job)
    {
        mafiaRole.gameObject.SetActive(job == "마피아");
        gangsterRole.gameObject.SetActive(job == "건달");
        doctorRole.gameObject.SetActive(job == "의사");
        policeRole.gameObject.SetActive(job == "경찰");
        stalkerRole.gameObject.SetActive(job == "스토커");
        citizenRole.gameObject.SetActive(job == "시민");

        switch (job)
        {
            case "마피아":
                StartCoroutine(JobTextCoroutine(mafiaText));
                break;
            case "건달":
                StartCoroutine(JobTextCoroutine(gangsterText));
                break;
            case "의사":
                StartCoroutine(JobTextCoroutine(doctorText));
                break;
            case "경찰":
                StartCoroutine(JobTextCoroutine(policeText));
                break;
            case "스토커":
                StartCoroutine(JobTextCoroutine(stalkerText));
                break;
            default:
                StartCoroutine(JobTextCoroutine(citizenText));
                break;
        }
    }

    IEnumerator JobTextCoroutine(RectTransform panel)
    {
        panel.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        panel.gameObject.SetActive(false);
    }

    public string GetPlayerJob(Player player)
    {
        if (player.CustomProperties.ContainsKey("Job"))
        {
            return (string)player.CustomProperties["Job"];
        } 
        return null;
    }
}