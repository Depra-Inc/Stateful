namespace Depra.StateMachines.Domain
{
    public interface IStateMachine
    {
        IState CurrentState { get; }
        
        void Tick();
        
        void ChangeState(IState state);

        bool NeedTransition(out IState nextState);
    }
}