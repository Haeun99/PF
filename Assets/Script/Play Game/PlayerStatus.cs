using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerStatus : MonoBehaviour
{
    public Button[] playerVote;
    public Button[] jobVote;
    public TMP_InputField[] jobChatting;

    public static PlayerStatus Instance { get; private set; }

    private bool isDead;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SetDead(bool dead)
    {
        isDead = dead;
        UpdatePlayerStatus();

        SetUIActive(!dead);

        InGameChatting.Instance.SubscribeToChannels(true);
    }

    public void UpdatePlayerStatus()
    {
        Hashtable playerProperties = new Hashtable { { "isDead", isDead } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }

    private void SetUIActive(bool isActive)
    {
        foreach (Button button in playerVote)
        {
            button.gameObject.SetActive(isActive);
        }

        foreach (Button button in jobVote)
        {
            button.gameObject.SetActive(isActive);
        }

        foreach (TMP_InputField inputField in jobChatting)
        {
            inputField.gameObject.SetActive(isActive);
        }
    }
}
