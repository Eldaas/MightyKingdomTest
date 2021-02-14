using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    [Header("References")]
    public ModeSelectorPanel modeSelectorPanel;
    public StandardModePanel standardModePanel;
    public TimerModePanel timerModePanel;
    public StopwatchModePanel stopwatchModePanel;
    public Button deleteButton;

    [Header("State Machine")]
    public ClockStateMachine clockSM;
    public StandardClockState standard;
    public TimerClockState timer;
    public StopwatchClockState stopwatch;

    private void Awake()
    {
        clockSM = new ClockStateMachine();
        standard = new StandardClockState(this, clockSM);
        timer = new TimerClockState(this, clockSM);
        stopwatch = new StopwatchClockState(this, clockSM);
        deleteButton.onClick.AddListener(DeleteClock);
    }

    private void Start()
    {
        clockSM.Initialize(standard);
    }

    private void Update()
    {
        clockSM.CurrentState.LogicUpdate();
        DeleteButtonSelectable();
    }

    /// <summary>
    /// Remove this single clock instance from the list.
    /// </summary>
    public void DeleteClock()
    {
        ClockManager.instance.activeClocks.Remove(this);
        Destroy(gameObject);
    }

    /// <summary>
    /// Checks each frame if the clock delete button should be interactable or not. Not interactable if there's only one clock in the scene.
    /// </summary>
    private void DeleteButtonSelectable()
    {
        if(ClockManager.instance.activeClocks.Count > 1)
        {
            deleteButton.interactable = true;
        }
        else
        {
            deleteButton.interactable = false;
        }
    }
}
