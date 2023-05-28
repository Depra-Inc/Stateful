using Depra.StateMachines.Abstract;

namespace Depra.StateMachines.Transition
{
    public interface IStateTransitionCoordination
    {
        void Update(IState state);
        
        bool NeedTransition(out IState nextState);
        
        void AddAnyTransition(IStateTransition transition);

        void AddTransition(IState from, IStateTransition transition);
    }
}