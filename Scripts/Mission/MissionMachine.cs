using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionMachine
{
    IMissionState currentState;
    IMissionState previousState;
    public void ChangeState(IMissionState state)
    {
        if (currentState != null)
        {
            currentState.Exit();
            previousState = currentState;
        }
        currentState = state;
        currentState.Enter();
    }
    public void SwitchToPreviusState()
    {
        ChangeState(previousState);
    }

    public void ExecuteStateLogic()
    {
        var newState = currentState.Execute();
        if (newState != null)
        {
            currentState.Exit();
            newState.Enter();
            previousState = currentState;
            currentState = newState;
        }
    }
}
