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


    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SetDead(Player player, bool dead)
    {
        Hashtable playerProperties = new Hashtable { { "isDead", dead } };
        player.SetCustomProperties(playerProperties);

        SetUIActive(player, !dead);
        InGameChatting.Instance.SubscribeToChannels(dead);
    }

    private void SetUIActive(Player player, bool isInteractable)
    {
        if (player.NickName == PhotonNetwork.LocalPlayer.NickName)
        {
            foreach (Button button in playerVote)
            {
                button.interactable = isInteractable;
            }

            foreach (Button button in jobVote)
            {
                button.interactable = isInteractable;
            }

            foreach (TMP_InputField inputField in jobChatting)
            {
                inputField.interactable = isInteractable;
            }
        }
    }
}