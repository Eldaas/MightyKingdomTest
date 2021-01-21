using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockStateMachine
{
    public ClockState CurrentState { get; private set; }

    public void Initialize(ClockState startingState)
    {
        CurrentState = startingState;
        startingState.Enter();
    }

    public void ChangeState(ClockState newState)
    {
        CurrentState.Exit();

        CurrentState = newState;
        newState.Enter();
    }
}
