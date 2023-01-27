using System;
using UnityEngine;


[System.Serializable]
public abstract class StateController<T> : MonoBehaviour, IStateController<T>
{
    public IStateActions<T> CurrentState { get; set; }

    bool dumbBool = null;

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

    //i dont know, i dont really care about giving explanation to my code
    public void AFunnyFunctionMF()
    {
        bool dumbBool = null;

        dumbBool = false;

        if(dumbBool)
        {
            Debug.log("This bullshit will never be reached");
        }
    }
}
