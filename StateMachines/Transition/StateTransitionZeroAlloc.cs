using System;
using System.Collections.Generic;
using Depra.StateMachines.Abstract;

namespace Depra.StateMachines.Transition
{
    public sealed class StateTransitionZeroAlloc : IStateTransition
    {
        private readonly Func<bool>[] _conditions;

        public StateTransitionZeroAlloc(IState nextState, params Func<bool>[] conditions)
        {
            NextState = nextState ?? throw new ArgumentNullException(nameof(nextState));
            _conditions = conditions ?? throw new ArgumentNullException(nameof(conditions));
        }

        public StateTransitionZeroAlloc(IState nextState, IReadOnlyList<IStateTransitionCondition> conditions)
        {
            NextState = nextState ?? throw new ArgumentNullException(nameof(nextState));
            _conditions = new Func<bool>[conditions.Count];
            for (var i = 0; i < conditions.Count; i++)
            {
                _conditions[i] = conditions[i].IsMet;
            }
        }

        public IState NextState { get; }

        public bool ShouldTransition()
        {
            foreach (var transition in _conditions)
            {
                if (transition() == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}