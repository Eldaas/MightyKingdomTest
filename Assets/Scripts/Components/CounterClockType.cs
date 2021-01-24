using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CounterClockType : ClockPanel
{
    [Header("Inherited Inspector References [CounterClockType]")]
    public TMP_InputField hoursInput;
    public TMP_InputField minutesInput;
    public TMP_InputField secondsInput;
    public Button playButton;
    public Button stopButton;

    [Header("Inherited Data References [CounterClockType]")]
    public float secondsTimer;
    
    protected override void Start()
    {
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

    /// <summary>
    /// When called, sets the clock's 'isRunning' boolean to true. Child classes may override this with additional functionality.
    /// </summary>
    protected virtual void StartTimer()
    {
        isRunning = true;
    }

    /// <summary>
    /// When called, sets the clock's 'isRunning' boolean to false. Child classes may override this with additional functionality.
    /// </summary>
    protected virtual void StopTimer()
    {
        isRunning = false;
    }

    /// <summary>
    /// Resets the clock back to its default values.
    /// </summary>
    public virtual void ResetClock()
    {
        isRunning = false;
        hoursInput.text = "00";
        minutesInput.text = "00";
        secondsInput.text = "00";
        secondsTimer = 0f;
    }

    /// <summary>
    /// Performs a validation check on the clock's UI buttons to determine whether they should be interactable or not based upon the clock's state.
    /// </summary>
    public virtual void ButtonValidation()
    {
        stopButton.interactable = isRunning;
    }

    /// <summary>
    /// Converts the counted number of seconds into string hours, minutes and seconds. Updates the input fields on the clock.
    /// </summary>
    protected void ParseTime()
    {
        TimeSpan time = TimeSpan.FromSeconds(secondsTimer);

        hoursInput.text = string.Format("{0:D2}", time.Hours);
        minutesInput.text = string.Format("{0:D2}", time.Minutes);
        secondsInput.text = string.Format("{0:D2}", time.Seconds);
    }
}
