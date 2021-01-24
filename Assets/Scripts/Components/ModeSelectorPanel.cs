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

    /// <summary>
    /// Adds onClick listeners to the mode selector panel's icons, representing each mode for the clock.
    /// </summary>
    private void RegisterListeners()
    {
        clockModeButton.onClick.AddListener(SelectClockMode);
        timerModeButton.onClick.AddListener(SelectTimerMode);
        stopwatchModeButton.onClick.AddListener(SelectStopwatchMode);
    }

    /// <summary>
    /// Tied to an onClick listener, this changes the parent clock's mode to standard clock mode.
    /// </summary>
    private void SelectClockMode()
    {
        if(clock.clockSM.CurrentState != clock.standard)
        {
            clock.clockSM.ChangeState(clock.standard);
        }
        
    }

    /// <summary>
    /// Tied to an onClick listener, this changes the parent clock's mode to timer clock mode.
    /// </summary>
    private void SelectTimerMode()
    {
        if (clock.clockSM.CurrentState != clock.timer)
        {
            clock.clockSM.ChangeState(clock.timer);
        }

    }

    /// <summary>
    /// Tied to an onClick listener, this changes the parent clock's mode to stopwatch clock mode.
    /// </summary>
    private void SelectStopwatchMode()
    {
        if (clock.clockSM.CurrentState != clock.stopwatch)
        {
            clock.clockSM.ChangeState(clock.stopwatch);
        }
    }
}
