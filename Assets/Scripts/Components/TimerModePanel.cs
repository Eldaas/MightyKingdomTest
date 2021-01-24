using UnityEngine;

public class TimerModePanel : CounterClockType
{
    [Header("Non-inherited Inspector References")]
    public AudioSource audioSource;
    public AudioClip timerElapseSound;    

    protected override void Awake()
    {
        base.Awake();
        RegisterListeners();
        audioSource = Camera.main.GetComponent<AudioSource>();
    }
    
    protected override void StartTimer()
    {
        secondsTimer = InputToSeconds();

        if (secondsTimer > 0)
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
    /// Used by the timer state to listen for when the timer reaches zero. Upon this occurring the timer will be zeroed, an audio notification will play and the timer will be stopped.
    /// </summary>
    public void ListenForZero()
    {
        if(isRunning && secondsTimer <= float.Epsilon)
        {
            secondsTimer = 0f;
            StopTimer();
            audioSource.PlayOneShot(timerElapseSound);
            ResetClock();
        }
    }

    public override void ButtonValidation()
    {
        base.ButtonValidation();
        if(!isRunning && CheckInputFields())
        {
            playButton.interactable = true;
        }        
        else
        {
            playButton.interactable = false;
        }
    }

    /// <summary>
    /// Checks each of the timer clock input fields to determine whether the field contains a string number value higher than 0.
    /// </summary>
    /// <returns>True if a timer clock string field is '00' or '0', false if all fields are either '00' or '0'.</returns>
    private bool CheckInputFields()
    {

        if(hoursInput.text != "00")
        {
            if (hoursInput.text != "0")
            {
                return true;
            }
        }

        if(minutesInput.text != "00")
        {
            if(minutesInput.text != "0")
            {
                return true;
            }
        }

        if(secondsInput.text != "00")
        {
            if(secondsInput.text != "0")
            {
                return true;
            } 
        }

        return false;
    }

    public override void UpdateUI()
    {
        base.UpdateUI();
        if(isRunning)
        {
            secondsTimer -= Time.deltaTime;
            ParseTime();
        }
    }

    public override void ResetClock()
    {
        base.ResetClock();
        LockInputs(false);
    }

    /// <summary>
    /// Locks the timer clock hours, minutes and seconds input.
    /// </summary>
    /// <param name="state">If 'true', locks input. If 'false', unlocks input.</param>
    private void LockInputs(bool state)
    {
        hoursInput.interactable = !state;
        minutesInput.interactable = !state;
        secondsInput.interactable = !state;
    }
}
