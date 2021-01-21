using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerModePanel : ClockPanel
{
    [Header("Inspector References")]
    public TMP_InputField hoursInput;
    public TMP_InputField minutesInput;
    public TMP_InputField secondsInput;
    public Button playButton;
    public Button stopButton;
    public AudioSource audioSource;
    public AudioClip timerElapseSound;

    [Header("Data")]
    public float secondsRemaining;

    protected override void Awake()
    {
        base.Awake();
        RegisterListeners();
        audioSource = Camera.main.GetComponent<AudioSource>();
    }

    protected override void Start()
    {
        base.Start();
        Initialise();
    }

    protected override void RegisterListeners()
    {
        base.RegisterListeners();
        playButton.onClick.AddListener(StartTimer);
        stopButton.onClick.AddListener(StopTimer);
    }

    protected override void Initialise()
    {
        base.Initialise();
        ResetClock();
    }

    protected override void StartTimer()
    {
        secondsRemaining = InputToSeconds();

        if (secondsRemaining > 0)
        {
            base.StartTimer();
            LockInputs(true);
        }           
    }

    protected override void StopTimer()
    {
        base.StopTimer();
        LockInputs(false);
    }

    /// <summary>
    /// Takes the text data from the timer input fields and parses into integers before refining down to seconds.
    /// </summary>
    /// <returns>The user's timer input as integer seconds.</returns>
    private int InputToSeconds()
    {
        int hours = int.Parse(hoursInput.text);
        int minutes = int.Parse(minutesInput.text);
        int seconds = int.Parse(secondsInput.text);
        int totalSeconds = hours * 60 * 60 + minutes * 60 + seconds;
        return totalSeconds;
    }

    /// <summary>
    /// Converts the number of seconds remaining into hours, minutes and seconds and updates the input fields on the timer.
    /// </summary>
    private void ParseTimeRemaining()
    {
        TimeSpan time = TimeSpan.FromSeconds(secondsRemaining);

        hoursInput.text = string.Format("{0:D2}", time.Hours);
        minutesInput.text = string.Format("{0:D2}", time.Minutes);
        secondsInput.text = string.Format("{0:D2}", time.Seconds);
    }

    /// <summary>
    /// Used by the timer state to listen for when the timer reaches zero. Upon this occurring the timer will be zeroed, an audio notification will play and the timer will be stopped.
    /// </summary>
    public void ListenForZero()
    {
        if(isRunning && secondsRemaining <= float.Epsilon)
        {
            secondsRemaining = 0f;
            StopTimer();
            audioSource.PlayOneShot(timerElapseSound);
        }
    }

    public override void UpdateUI()
    {
        base.UpdateUI();
        if(isRunning)
        {
            secondsRemaining -= Time.deltaTime;
            ParseTimeRemaining();
        }
    }

    public override void ResetClock()
    {
        base.ResetClock();
        hoursInput.text = "00";
        minutesInput.text = "00";
        secondsInput.text = "00";
        LockInputs(false);
    }

    private void LockInputs(bool state)
    {
        hoursInput.interactable = !state;
        minutesInput.interactable = !state;
        secondsInput.interactable = !state;
    }
}
