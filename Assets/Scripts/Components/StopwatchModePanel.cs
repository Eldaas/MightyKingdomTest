using UnityEngine;
using UnityEngine.UI;

public class StopwatchModePanel : CounterClockType
{
    [Header("Inspector References")]
    public Button resetButton;

    protected override void Awake()
    {
        base.Awake();
        RegisterListeners();
    }

    protected override void RegisterListeners()
    {
        base.RegisterListeners();
        resetButton.onClick.AddListener(ResetClock);
    }

    public override void ButtonValidation()
    {
        base.ButtonValidation();
        playButton.interactable = !isRunning;
    }

    public override void UpdateUI()
    {
        base.UpdateUI();
        if (isRunning)
        {
            secondsTimer += Time.deltaTime;
            ParseTime();
        }
    }


    
}
