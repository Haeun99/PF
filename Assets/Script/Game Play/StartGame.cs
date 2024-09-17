using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class StartGame : MonoBehaviour
{
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
    private Dictionary<Player, string> playerJobs = new Dictionary<Player, string>();

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
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

            AssignJobsToPlayers();
        }
    }

    private void AssignJobsToPlayers()
    {
        PhotonView photonView = GetComponent<PhotonView>();

        List<Player> players = new List<Player>(PhotonNetwork.PlayerList);

        foreach (Player player in players)
        {
            int randomIndex = Random.Range(0, availableJobs.Count);
            string assignedJob = availableJobs[randomIndex];

            playerJobs[player] = assignedJob;

            photonView.RPC("SetPlayerJob", RpcTarget.All, player, assignedJob);

            availableJobs.RemoveAt(randomIndex);
        }
    }

    [PunRPC]
    public void SetPlayerJob(Player player, string job)
    {
        if (player == PhotonNetwork.LocalPlayer)
        {
            ShowJobUI(job);
        }
    }

    private void ShowJobUI(string job)
    {
        switch (job)
        {
            case "Mafia":
                mafiaRole.gameObject.SetActive(true);
                StartCoroutine(JobTextCoroutine(mafiaText));
                break;

            case "Gangster":
                gangsterRole.gameObject.SetActive(true);
                StartCoroutine(JobTextCoroutine(gangsterText));
                break;

            case "Doctor":
                doctorRole.gameObject.SetActive(true);
                StartCoroutine(JobTextCoroutine(doctorText));
                break;

            case "Police":
                policeRole.gameObject.SetActive(true);
                StartCoroutine(JobTextCoroutine(policeText));
                break;

            case "Stalker":
                stalkerRole.gameObject.SetActive(true);
                StartCoroutine(JobTextCoroutine(stalkerText));
                break;

            default:
                citizenRole.gameObject.SetActive(true);
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
}