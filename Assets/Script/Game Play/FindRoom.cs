using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class FindRoom : MonoBehaviourPunCallbacks
{
    public static FindRoom Instance { get; private set; }

    private void Start()
    {
        if (Instance != null)
        {
            Instance = this;
        }
    }
}
