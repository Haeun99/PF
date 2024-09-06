using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

    public FirebaseApp App { get; private set; }
    public FirebaseAuth Auth { get; private set; }
    public FirebaseDatabase DB { get; private set; }

    public InputField loginIDInput;
    public InputField loginPWInput;
    public TextMeshProUGUI loginError;

    [Space(20)]
    public InputField signupIDInput;
    public InputField signupNNInput;
    public InputField signupPWInput;
    public TextMeshProUGUI signupIDError;
    public TextMeshProUGUI signupNNError;

    private void Update()
    {
        Instance = this;
    }

    private void Start()
    {

    }
}
