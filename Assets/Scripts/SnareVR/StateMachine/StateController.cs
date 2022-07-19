using System;
using UnityEngine;


[System.Serializable]
public abstract class StateController<T> : MonoBehaviour, IStateController<T>
{
    public IStateActions<T> CurrentState { get; set; }

    protected virtual void Start()
    {
        CurrentState.EnterState(this);
    }

    //for dynamic repeativite items (like slowing scaling an object or changing color etc)
    protected virtual void Update()
    {
        CurrentState.ControlledUpdate(this);
    }

    public void SwitchState(IStateActions<T> state)
    {
        CurrentState = state; // Assign new state
        CurrentState.EnterState(this); // Start the new state
    }

    private void OnDisable()
    {
        CurrentState = null;
    }
}
