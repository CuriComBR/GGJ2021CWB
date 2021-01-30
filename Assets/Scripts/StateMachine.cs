using System;
using UnityEngine;

public class StateMachine
{
    private State currentState;

    public bool IsCurrentState(Type stateType)
    {
        return currentState?.GetType() == stateType;
    }

    public void TransitionTo(State newState)
    {
        if (currentState != null && !currentState.CanTransitionToSelf() && IsCurrentState(newState.GetType()))
        {
            return;
        }
        currentState?.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }

    public void Tick()
    {
        currentState?.Tick();
    }
}