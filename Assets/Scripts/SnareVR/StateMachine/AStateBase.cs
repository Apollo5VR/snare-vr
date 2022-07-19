using System;
using UnityEngine;

//TODO - can refactor ConsumableManager State Machine to inherit from this
[System.Serializable]
public abstract class AStateBase<T> : IStateActions<T>
{
    public T StatesController { get; set; }

    public virtual void EnterState(in T state)
    {
        StatesController ??= state;
    }

    public virtual void EnterState(in IStateController<T> state)
    {
        EnterState((T) state);
    }

    public abstract void ExitState(in T state);

    public abstract void ControlledUpdate(in IStateController<T> state);

    public virtual void CollisionEnter(in GameObject obj)
    {
        
    }
}

[System.Serializable]
public abstract class AStateMono<T> : MonoBehaviour, IStateActions<T>
{
    protected T StatesController { get; private set; }

    public virtual void EnterState(in T state)
    {
        gameObject.SetActive(true);
        StatesController ??= state;
    }

    public virtual void EnterState(in IStateController<T> state)
    {
        EnterState((T)state);
    }

    public abstract void ControlledUpdate(in IStateController<T> stateController);

    public virtual void CollisionEnter(in GameObject obj)
    {

    }
}
