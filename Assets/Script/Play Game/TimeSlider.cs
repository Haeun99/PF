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
    public double timeRemaining { get; private set; }
    private double startTime;

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
            timeRemaining = currentTime - (PhotonNetwork.Time - startTime);

            if (timeRemaining < 0)
            {
                timeRemaining = 0;
            }

            UpdateSlider();
        }
    }

    public void StartTimer(string timeKey)
    {
        currentTime = GetTimeFromRoomProperties(timeKey);

        startTime = PhotonNetwork.Time;

        timeRemaining = currentTime;
        slider.maxValue = currentTime;
        slider.value = currentTime;
    }

    public void StartTimer(double duration)
    {
        currentTime = (int)duration;

        startTime = PhotonNetwork.Time;

        timeRemaining = currentTime;
        slider.maxValue = currentTime;
        slider.value = currentTime;
    }

    private void UpdateSlider()
    {
        slider.value = (float)timeRemaining;
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