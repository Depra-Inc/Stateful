// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using Depra.StateMachines.Abstract;

namespace Depra.StateMachines.Transition
{
    public sealed class StateTransition : IStateTransition
    {
        private readonly Func<bool>[] _conditions;

        public StateTransition(IState nextState, params Func<bool>[] conditions)
        {
            NextState = nextState ?? throw new ArgumentNullException(nameof(nextState));
            _conditions = conditions ?? throw new ArgumentNullException(nameof(conditions));
        }

        public StateTransition(IState nextState, IReadOnlyList<IStateTransitionCondition> conditions)
        {
            NextState = nextState ?? throw new ArgumentNullException(nameof(nextState));
            _conditions = new Func<bool>[conditions.Count];
            for (var i = 0; i < conditions.Count; i++)
            {
                _conditions[i] = conditions[i].IsMet;
            }
        }

        public IState NextState { get; }

        public bool ShouldTransition() =>
            _conditions.All(condition => condition());
    }
}