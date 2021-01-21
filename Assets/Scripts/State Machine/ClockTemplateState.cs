public class ClockTemplateState : ClockState
{
    public ClockTemplateState(Clock clock, ClockStateMachine stateMachine) : base(clock, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
