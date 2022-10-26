using System;
using System.Collections.Generic;
using Depra.StateMachines.Domain;

namespace Depra.StateMachines.Application
{
    public class StateMachine : IStateMachine
    {
        private List<IStateTransition> _currentTransitions;
        private readonly List<IStateTransition> _anyTransitions;
        private readonly Dictionary<Type, List<IStateTransition>> _transitions;
        
        private static readonly List<IStateTransition> EMPTY_TRANSITIONS = new List<IStateTransition>();
        
        public IState CurrentState { get; private set; }

        public void Tick()
        {
            if (NeedTransition(out var nextState))
            {
                ChangeState(nextState);
            }
        }

        public void ChangeState(IState state)
        {
            if (state == CurrentState)
            {
                return;
            }

            CurrentState?.Exit();

            CurrentState = state;
            FetchCurrentTransitions();

            CurrentState?.Enter();
        }

        public bool NeedTransition(out IState nextState)
        {
            foreach (var transition in _anyTransitions)
            {
                if (transition.ShouldTransition())
                {
                    nextState = transition.NextState;
                    return true;
                }
            }

            foreach (var transition in _currentTransitions)
            {
                if (transition.ShouldTransition())
                {
                    nextState = transition.NextState;
                    return true;
                }
            }

            nextState = null;
            return false;
        }

        public void AddTransition(IState from, IState to, IStateTransition transition)
        {
            var sourceStateType = from.GetType();

            if (_transitions.TryGetValue(sourceStateType, out var transitions) == false)
            {
                transitions = new List<IStateTransition>();
                _transitions[sourceStateType] = transitions;
            }

            transitions.Add(transition);
        }
        
        public void AddAnyTransition(IState to, IStateTransition transition) =>  _anyTransitions.Add(transition);

        public StateMachine(IState startingState)
        {
            _anyTransitions = new List<IStateTransition>();
            _currentTransitions = new List<IStateTransition>();
            _transitions = new Dictionary<Type, List<IStateTransition>>();

            ChangeState(startingState);
        }

        private void FetchCurrentTransitions()
        {
            _transitions.TryGetValue(CurrentState.GetType(), out _currentTransitions);
            if (_currentTransitions == null)
            {
                _currentTransitions = EMPTY_TRANSITIONS;
            }
        }
    }
}