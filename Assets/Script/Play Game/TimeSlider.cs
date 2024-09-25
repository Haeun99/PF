using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSlider : MonoBehaviour
{
    public static TimeSlider Instance { get; private set; }

    public Slider slider;

    private float currentTime;
    private float timeRemaining;
    private bool isDayPhase = true;

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

            //if (timeRemaining <= 0)
            //{
            //    SwitchPhase();
            //}
        }
    }

    public void StartDayPhase()
    {
        currentTime = GetTimeFromRoomProperties("DayTime");
        timeRemaining = currentTime;
        slider.maxValue = currentTime;
        slider.value = currentTime;
        isDayPhase = true;
    }

    public void StartNightPhase()
    {
        currentTime = GetTimeFromRoomProperties("NightTime");
        timeRemaining = currentTime;
        slider.maxValue = currentTime;
        slider.value = currentTime;
        isDayPhase = false;
    }

    public void StartVotePhase()
    {
        currentTime = 10f;
        timeRemaining = currentTime;
        slider.maxValue = currentTime;
        slider.value = currentTime;
    }

    public void FinalAppealPhase()
    {
        currentTime = 30f;
        timeRemaining = currentTime;
        slider.maxValue = currentTime;
        slider.value = currentTime;
    }

    private void UpdateSlider()
    {
        slider.value = timeRemaining;
    }

    //private void SwitchPhase()
    //{
    //    if (isDayPhase)
    //    {
    //        StartNightPhase();
    //    }
    //    else
    //    {
    //        StartDayPhase();
    //    }
    //}

    private float GetTimeFromRoomProperties(string key)
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(key))
        {
            return (float)PhotonNetwork.CurrentRoom.CustomProperties[key];
        }
        return 0;
    }
}
