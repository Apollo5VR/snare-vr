
public interface IStateController<T>
{
    IStateActions<T> CurrentState { get; set; }
    void SwitchState(IStateActions<T> state);
}