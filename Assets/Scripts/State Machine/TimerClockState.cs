public class TimerClockState : ClockState
{
    public TimerClockState(Clock clock, ClockStateMachine stateMachine) : base(clock, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        clock.timerModePanel.gameObject.SetActive(true);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        clock.timerModePanel.ListenForZero();
        clock.timerModePanel.UpdateUI();
    }

    public override void Exit()
    {
        base.Exit();
        clock.timerModePanel.gameObject.SetActive(false);
        clock.timerModePanel.ResetClock();
    }
}
