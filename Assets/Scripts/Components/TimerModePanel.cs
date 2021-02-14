using UnityEngine;
using TMPro;

public class TimerModePanel : CounterClockType
{
    [Header("Non-inherited Inspector References"), SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip timerElapseSound;    

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
    /// Checks each of the timer clock input fields.
    /// </summary>
    /// <returns>True if the user has entered anything other than '00' or '0' in any of the fields.</returns>
    private bool CheckInputFields()
    {

        if(CheckField(hoursInput) || CheckField(minutesInput) || CheckField(secondsInput))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks the input field for its text value.
    /// </summary>
    /// <param name="field">The TMP_InputField being checked.</param>
    /// <returns>True if the field being checked contains anything other than '00' or '0'.</returns>
    private bool CheckField(TMP_InputField field)
    {
        if(field.text == "00" || field.text == "0")
        {
            return false;
        }

        return true;
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
