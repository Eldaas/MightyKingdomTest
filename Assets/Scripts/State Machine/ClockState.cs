using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ClockState
{
    protected Clock clock;
    protected ClockStateMachine stateMachine;

    protected ClockState(Clock clock, ClockStateMachine stateMachine)
    {
        this.clock = clock;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {

    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Exit()
    {

    }
}
