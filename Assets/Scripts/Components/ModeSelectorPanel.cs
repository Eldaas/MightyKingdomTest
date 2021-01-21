using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeSelectorPanel : MonoBehaviour
{
    [Header("Inspector References")]
    public Button clockModeButton;
    public Button timerModeButton;
    public Button stopwatchModeButton;
    public Clock clock;

    private void Awake()
    {
        RegisterListeners();
    }

    private void RegisterListeners()
    {
        clockModeButton.onClick.AddListener(selectClockMode);
        timerModeButton.onClick.AddListener(selectTimerMode);
        stopwatchModeButton.onClick.AddListener(selectStopwatchMode);
    }

    private void selectClockMode()
    {
        if(clock.clockSM.CurrentState != clock.standard)
        {
            clock.clockSM.ChangeState(clock.standard);
        }
        
    }

    private void selectTimerMode()
    {
        if (clock.clockSM.CurrentState != clock.timer)
        {
            clock.clockSM.ChangeState(clock.timer);
        }

    }

    private void selectStopwatchMode()
    {
        if (clock.clockSM.CurrentState != clock.stopwatch)
        {
            clock.clockSM.ChangeState(clock.stopwatch);
        }
    }
}
