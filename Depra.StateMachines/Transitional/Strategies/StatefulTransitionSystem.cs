// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using Depra.StateMachines.Abstract;
using Depra.StateMachines.Transitional.Commands;
using Depra.StateMachines.Transitional.Transition;

namespace Depra.StateMachines.Transitional.Strategies
{
	public sealed class StatefulTransitionSystem : IStateMachine, IStateCommand, IDisposable
	{
		private readonly IStateMachine _machine;
		private readonly ITransitions _transitions;
		private IReadOnlyList<IStateTransition> _current = Transitions.Empty;

		public event StateChangedDelegate StateChanged;

		public StatefulTransitionSystem(IStateMachine machine, ITransitions transitions)
		{
			_machine = machine ?? throw new ArgumentNullException(nameof(machine));
			_transitions = transitions ?? throw new ArgumentNullException(nameof(transitions));

			_machine.StateChanged += OnStateChanged;
		}

		public void Dispose() =>
			_machine.StateChanged -= OnStateChanged;

		public IState CurrentState =>
			_machine.CurrentState;

		public void SwitchState(IState to) =>
			_machine.SwitchState(to);

		public void Execute()
		{
			if (NeedTransition(out var nextState))
			{
				SwitchState(nextState);
			}
		}

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

		private void OnStateChanged(IState state) =>
			_current = _transitions[state] ?? Transitions.Empty;
	}
}