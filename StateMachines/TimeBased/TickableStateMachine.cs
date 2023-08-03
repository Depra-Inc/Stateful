// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using System;
using Depra.StateMachines.Abstract;

namespace Depra.StateMachines.TimeBased
{
	public sealed class TickableStateMachine : ITickableStateMachine,
		ITickableStateMachine<ITickableState>
	{
		private readonly IStateMachine<ITickableState> _stateMachine;

		public event Action<IState> StateChanged
		{
			add => _stateMachine.StateChanged += value;
			remove => _stateMachine.StateChanged -= value;
		}

		public TickableStateMachine(IStateMachine<ITickableState> stateMachine) =>
			_stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));

		ITickableState IStateMachine<ITickableState>.CurrentState =>
			_stateMachine.CurrentState;

		IState IStateMachine<IState>.CurrentState =>
			_stateMachine.CurrentState;

		void IStateMachine<ITickableState>.SwitchState(ITickableState to) =>
			_stateMachine.SwitchState(to);

		void IStateMachine<IState>.SwitchState(IState to) =>
			_stateMachine.SwitchState((ITickableState) to);

		void ITickableStateMachine.Tick() =>
			_stateMachine.CurrentState.Tick();
	}
}