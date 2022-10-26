using System;
using System.Collections.Generic;
using System.Linq;
using Depra.StateMachines.Domain;

namespace Depra.StateMachines.Application
{
    public class StateTransition : IStateTransition
    {
        private readonly Func<bool>[] _conditions;

        public IState NextState { get; }

        public bool ShouldTransition() => _conditions.All(condition => condition());

        public StateTransition(IState nextState, params Func<bool>[] conditions)
        {
            NextState = nextState;
            _conditions = conditions;
        }

        public StateTransition(IState nextState, IReadOnlyList<IStateTransitionCondition> conditions)
        {
            NextState = nextState;
            _conditions = new Func<bool>[conditions.Count];
            for (var i = 0; i < conditions.Count; i++)
            {
                _conditions[i] = conditions[i].IsMet;
            }
        }
    }
}