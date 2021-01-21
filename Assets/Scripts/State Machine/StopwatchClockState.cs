public class StopwatchClockState : ClockState
{
    public StopwatchClockState(Clock clock, ClockStateMachine stateMachine) : base(clock, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        clock.stopwatchModePanel.gameObject.SetActive(true);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        clock.stopwatchModePanel.UpdateUI();
    }

    public override void Exit()
    {
        base.Exit();
        clock.stopwatchModePanel.gameObject.SetActive(false);
        clock.stopwatchModePanel.ResetClock();
    }
}
