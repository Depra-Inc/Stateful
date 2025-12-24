// SPDX-License-Identifier: Apache-2.0
// © 2022-2025 Depra <n.melnikov@depra.org>

using System;
using Depra.Stateful.Abstract;
using Depra.Stateful.Finite;

namespace Depra.Stateful.Transitional
{
	public sealed class TransitionDrivenStateMachine : IFiniteStateMachine
	{
		private readonly IFiniteStateMachine _machine;
		private readonly IStateTransitions _transitions;

		event StateChangedDelegate IStateMachine<IFiniteState>.StateChanged
		{
			add => _machine.StateChanged += value;
			remove => _machine.StateChanged -= value;
		}

		public TransitionDrivenStateMachine(IFiniteStateMachine machine, IStateTransitions transitions)
		{
			_machine = machine ?? throw new ArgumentNullException(nameof(machine));
			_transitions = transitions ?? throw new ArgumentNullException(nameof(transitions));
		}

		public IFiniteState CurrentState => _machine.CurrentState;

		public void Tick()
		{
			if (_transitions.NeedTransition(CurrentState, out var nextState))
			{
				SwitchState((IFiniteState) nextState);
			}
		}

		public void SwitchState(IFiniteState to) => _machine.SwitchState(to);
	}
}