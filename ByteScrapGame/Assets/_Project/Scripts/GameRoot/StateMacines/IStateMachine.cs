public interface IStateMachine
{
    void ChangeState(IState newState);
    IState CurrentState { get; }
}