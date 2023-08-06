// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using System;
using Depra.Stateful.Abstract;

namespace Depra.Stateful.Transitional
{
	public sealed class StatefulTransitionSystem : IStateMachine
	{
		private readonly IStateMachine _machine;
		private readonly IStateTransitions _transitions;

		public event StateChangedDelegate StateChanged
		{
			add => _machine.StateChanged += value;
			remove => _machine.StateChanged -= value;
		}

		public StatefulTransitionSystem(IStateMachine machine, IStateTransitions transitions)
		{
			_machine = machine ?? throw new ArgumentNullException(nameof(machine));
			_transitions = transitions ?? throw new ArgumentNullException(nameof(transitions));
		}

		public IState CurrentState => _machine.CurrentState;

		public void Tick()
		{
			if (_transitions.NeedTransition(CurrentState, out var nextState))
			{
				SwitchState(nextState);
			}
		}

		public void SwitchState(IState to) => _machine.SwitchState(to);
	}
}