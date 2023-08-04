// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using Depra.StateMachines.Abstract;
using Depra.StateMachines.Transitional.Transition;

namespace Depra.StateMachines.Transitional.Strategies
{
	public sealed class TransitionStrategy : ITransitionStrategy
	{
		private readonly ITransitions _transitions;
		private IReadOnlyList<IStateTransition> _current = Transitions.Empty;

		public TransitionStrategy(ITransitions transitions) =>
			_transitions = transitions ?? throw new ArgumentNullException(nameof(transitions));

		public void Execute(IState state) =>
			_current = _transitions[state] ?? Transitions.Empty;

		public bool NeedTransition(out IState nextState)
		{
			var transitions = _transitions.Any.Concat(_current);
			foreach (var transition in transitions)
			{
				if (transition.ShouldTransition() == false)
				{
					continue;
				}

				nextState = transition.NextState;
				return true;
			}

			nextState = null;
			return false;
		}
	}
}