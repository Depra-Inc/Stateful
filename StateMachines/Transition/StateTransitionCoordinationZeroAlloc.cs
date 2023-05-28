using System;
using System.Collections.Generic;
using Depra.StateMachines.Abstract;

namespace Depra.StateMachines.Transition
{
    public sealed class StateTransitionCoordinationZeroAlloc : IStateTransitionCoordination
    {
        private static readonly List<IStateTransition> EMPTY_TRANSITIONS = new List<IStateTransition>();
        
        private readonly List<IStateTransition> _anyTransitions;
        private readonly Dictionary<Type, List<IStateTransition>> _transitions;
        
        private List<IStateTransition> _currentTransitions;

        public StateTransitionCoordinationZeroAlloc()
        {
            _anyTransitions = new List<IStateTransition>();
            _currentTransitions = new List<IStateTransition>();
            _transitions = new Dictionary<Type, List<IStateTransition>>();
        }
        
        public void Update(IState state)
        {
            _transitions.TryGetValue(state.GetType(), out _currentTransitions);
            _currentTransitions ??= EMPTY_TRANSITIONS;
        }

        public bool NeedTransition(out IState nextState)
        {
            for (var index = 0; index < _anyTransitions.Count; index++)
            {
                if (_anyTransitions[index].ShouldTransition())
                {
                    nextState = _anyTransitions[index].NextState;
                    return true;
                }
            }

            for (var index = 0; index < _currentTransitions.Count; index++)
            {
                if ( _currentTransitions[index].ShouldTransition())
                {
                    nextState =  _currentTransitions[index].NextState;
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