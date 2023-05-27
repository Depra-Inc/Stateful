namespace Depra.StateMachines.Domain
{
    public interface ITransitionStateMachine : IStateMachine
    {
        void Tick();
        
        bool NeedTransition(out IState nextState);

        void AddAnyTransition(IStateTransition transition);
        
        void AddTransition(IState from, IStateTransition transition);
    }
}