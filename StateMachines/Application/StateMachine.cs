using System;
using Depra.StateMachines.Domain;

namespace Depra.StateMachines.Application
{
    public sealed class StateMachine : IStateMachine
    {
        private readonly bool _allowReentry;

        public event Action<IState> StateChanged;

        public StateMachine(IState startingState, bool allowReentry = false)
        {
            _allowReentry = allowReentry;
            ChangeState(startingState);
        }

        public IState CurrentState { get; private set; }

        public void ChangeState(IState state)
        {
            if (CanEnterState(state) == false)
            {
                return;
            }

            CurrentState?.Exit();
            CurrentState = state;
            CurrentState?.Enter();

            StateChanged?.Invoke(CurrentState);
        }

        private bool CanEnterState(IState state) => 
            _allowReentry || CurrentState != state;
    }
}