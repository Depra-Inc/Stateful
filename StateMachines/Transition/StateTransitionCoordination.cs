using System;
using System.Collections.Generic;
using Depra.StateMachines.Abstract;

namespace Depra.StateMachines.Transition
{
    public sealed class StateTransitionCoordination : IStateTransitionCoordination
    {
        private static readonly IList<IStateTransition> EMPTY_TRANSITIONS = new List<IStateTransition>();
        
        private readonly IList<IStateTransition> _anyTransitions;
        private readonly IDictionary<Type, IList<IStateTransition>> _transitions;
        
        private IList<IStateTransition> _currentTransitions;

        public StateTransitionCoordination()
        {
            _anyTransitions = new List<IStateTransition>();
            _currentTransitions = new List<IStateTransition>();
            _transitions = new Dictionary<Type, IList<IStateTransition>>();
        }
        
        public void Update(IState state)
        {
            _transitions.TryGetValue(state.GetType(), out _currentTransitions);
            _currentTransitions ??= EMPTY_TRANSITIONS;
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

        public void AddAnyTransition(IStateTransition transition) =>
            _anyTransitions.Add(transition);

        public void AddTransition(IState from, IStateTransition transition)
        {
            var sourceStateType = from.GetType();
            if (_transitions.TryGetValue(sourceStateType, out var transitions) == false)
            {
                transitions = new List<IStateTransition>();
                _transitions[sourceStateType] = transitions;
            }

            transitions.Add(transition);
        }
    }
}