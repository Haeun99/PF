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

            playerJobs[player] = assignedJob;
            randomJob.RemoveAt(randomIndex);
        }

        foreach (var pair in playerJobs)
        {
            Hashtable jobProperty = new Hashtable();
            jobProperty["Job"] = pair.Value;

            pair.Key.SetCustomProperties(jobProperty);
        }

        foreach (var player in players)
        {
            Debug.Log($"Player: {player.NickName}, Assigned Job: {playerJobs[player]}");
        }
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
        mafiaRole.gameObject.SetActive(job == "Mafia");
        gangsterRole.gameObject.SetActive(job == "Gangster");
        doctorRole.gameObject.SetActive(job == "Doctor");
        policeRole.gameObject.SetActive(job == "Police");
        stalkerRole.gameObject.SetActive(job == "Stalker");
        citizenRole.gameObject.SetActive(job == "Citizen");

        switch (job)
        {
            case "Mafia":
                StartCoroutine(JobTextCoroutine(mafiaText));
                break;
            case "Gangster":
                StartCoroutine(JobTextCoroutine(gangsterText));
                break;
            case "Doctor":
                StartCoroutine(JobTextCoroutine(doctorText));
                break;
            case "Police":
                StartCoroutine(JobTextCoroutine(policeText));
                break;
            case "Stalker":
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
        yield return new WaitForSeconds(5f);
        panel.gameObject.SetActive(false);
    }
}