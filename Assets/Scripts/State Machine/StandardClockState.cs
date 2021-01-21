public class StandardClockState : ClockState
{
    public StandardClockState(Clock clock, ClockStateMachine stateMachine) : base(clock, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        clock.standardModePanel.gameObject.SetActive(true);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        clock.standardModePanel.UpdateUI();
    }

    public override void Exit()
    {
        base.Exit();
        clock.standardModePanel.gameObject.SetActive(false);
    }
}
