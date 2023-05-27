using System;

namespace Depra.StateMachines.Domain
{
    public interface IStateMachine
    {
        event Action<IState> StateChanged;
        
        IState CurrentState { get; }

        void ChangeState(IState state);
    }
}