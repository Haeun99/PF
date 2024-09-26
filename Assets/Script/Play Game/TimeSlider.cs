using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSlider : MonoBehaviour
{
    public static TimeSlider Instance { get; private set; }

    public Slider slider;

    private int currentTime;
    public float timeRemaining { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateSlider();
        }
    }

    public void StartDayPhase()
    {
        currentTime = GetTimeFromRoomProperties("DayTime");
        timeRemaining = currentTime;
        slider.maxValue = currentTime;
        slider.value = currentTime;
    }

    public void StartNightPhase()
    {
        currentTime = GetTimeFromRoomProperties("NightTime");
        timeRemaining = currentTime;
        slider.maxValue = currentTime;
        slider.value = currentTime;
    }

    public void StartVotePhase()
    {
        currentTime = 10;
        timeRemaining = currentTime;
        slider.maxValue = currentTime;
        slider.value = currentTime;
    }

    public void FinalAppealPhase()
    {
        currentTime = 30;
        timeRemaining = currentTime;
        slider.maxValue = currentTime;
        slider.value = currentTime;
    }

    private void UpdateSlider()
    {
        slider.value = timeRemaining;
    }

    private int GetTimeFromRoomProperties(string key)
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(key))
        {
            return (int)PhotonNetwork.CurrentRoom.CustomProperties[key];
        }
        return 0;
    }
}