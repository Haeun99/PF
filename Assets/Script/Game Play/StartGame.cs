using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private List<string> availableJobs = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            InitializeJobList();
            AssignJobsToPlayers();
        }
    }

    private void InitializeJobList()
    {
        availableJobs.Clear();

        int mafiaCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["MafiaCount"];
        int gangsterCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["GangsterCount"];
        int doctorCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["DoctorCount"];
        int policeCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["PoliceCount"];
        int stalkerCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["StalkerCount"];

        for (int i = 0; i < mafiaCount; i++) availableJobs.Add("Mafia");
        for (int i = 0; i < gangsterCount; i++) availableJobs.Add("Gangster");
        for (int i = 0; i < doctorCount; i++) availableJobs.Add("Doctor");
        for (int i = 0; i < policeCount; i++) availableJobs.Add("Police");
        for (int i = 0; i < stalkerCount; i++) availableJobs.Add("Stalker");

        int totalPlayers = PhotonNetwork.PlayerList.Length;
        int assignedJobs = availableJobs.Count;
        int citizenCount = totalPlayers - assignedJobs;

        for (int i = 0; i < citizenCount; i++) availableJobs.Add("Citizen");
    }

    private void AssignJobsToPlayers()
    {
        List<Player> players = new List<Player>(PhotonNetwork.PlayerList);
        Dictionary<Player, string> playerJobs = new Dictionary<Player, string>();

        foreach (Player player in players)
        {
            int randomIndex = Random.Range(0, availableJobs.Count);
            string assignedJob = availableJobs[randomIndex];

            playerJobs[player] = assignedJob;
            availableJobs.RemoveAt(randomIndex);
        }

        Hashtable jobProperties = new Hashtable();
        foreach (var pair in playerJobs)
        {
            jobProperties[pair.Key.ActorNumber] = pair.Value;
        }

        PhotonNetwork.CurrentRoom.SetCustomProperties(jobProperties);
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);

        if (propertiesThatChanged.ContainsKey(PhotonNetwork.LocalPlayer.ActorNumber))
        {
            string job = (string)propertiesThatChanged[PhotonNetwork.LocalPlayer.ActorNumber];
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