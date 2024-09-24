using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Instance { get; private set; }

    public bool isDead;

    private void Awake()
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
    }

    private void UpdatePlayerStatus()
    {
        Hashtable playerProperties = new Hashtable { { "isDead", isDead } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }
}
