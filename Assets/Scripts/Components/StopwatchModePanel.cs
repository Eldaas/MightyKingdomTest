using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StopwatchModePanel : ClockPanel
{
    [Header("Inspector References")]
    public TMP_InputField hoursInput;
    public TMP_InputField minutesInput;
    public TMP_InputField secondsInput;
    public Button playButton;
    public Button stopButton;
    public Button resetButton;

    [Header("Data")]
    public float secondsSinceStart;

    protected override void Awake()
    {
        base.Awake();
        RegisterListeners();
    }

    protected override void Start()
    {
        base.Start();
        Initialise();
    }

    protected override void RegisterListeners()
    {
        playButton.onClick.AddListener(StartTimer);
        stopButton.onClick.AddListener(StopTimer);
        resetButton.onClick.AddListener(ResetClock);
    }

    protected override void Initialise()
    {
        base.Initialise();
        ResetClock();
    }

    public override void UpdateUI()
    {
        base.UpdateUI();
        if (isRunning)
        {
            secondsSinceStart += Time.deltaTime;
            SecondsToInputText();
        }
    }

    public override void ResetClock()
    {
        base.ResetClock();
        isRunning = false;
        hoursInput.text = "00";
        minutesInput.text = "00";
        secondsInput.text = "00";
        secondsSinceStart = 0f;
    }

    /// <summary>
    /// Converts the number of seconds since the timer started into hours, minutes and seconds. Updates the input fields on the clock.
    /// </summary>
    private void SecondsToInputText()
    {
        TimeSpan time = TimeSpan.FromSeconds(secondsSinceStart);

        hoursInput.text = string.Format("{0:D2}", time.Hours);
        minutesInput.text = string.Format("{0:D2}", time.Minutes);
        secondsInput.text = string.Format("{0:D2}", time.Seconds);
    }
}
