using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteManager : MonoBehaviour
{
    public static VoteManager Instance { get; private set; }
    private Player mostVotedPlayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMostVotedPlayer(Player player)
    {
        mostVotedPlayer = player;
    }

    public Player GetMostVotedPlayer()
    {
        return mostVotedPlayer;
    }
}
