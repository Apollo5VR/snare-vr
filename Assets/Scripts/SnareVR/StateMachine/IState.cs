using UnityEngine;

public interface IStateActions<T>
{
    void EnterState(in IStateController<T> stateControllere);

    void ControlledUpdate(in IStateController<T> stateController);
}
