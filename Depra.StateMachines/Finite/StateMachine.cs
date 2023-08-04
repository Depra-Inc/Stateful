// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using Depra.StateMachines.Abstract;

namespace Depra.StateMachines.Finite
{
	public sealed class StateMachine : IStateMachine
	{
		private readonly bool _allowReentry;

		public event StateChangedDelegate StateChanged;

		public StateMachine(bool allowReentry = false) =>
			_allowReentry = allowReentry;

		public StateMachine(IState startingState, bool allowReentry = false) : this(allowReentry) =>
			SwitchState(startingState);

		public IState CurrentState { get; private set; }

		public void SwitchState(IState to)
		{
			if (CanEnterState(to) == false)
			{
				return;
			}

			CurrentState?.Exit();
			CurrentState = to;
			CurrentState?.Enter();

			StateChanged?.Invoke(CurrentState);
		}

		private bool CanEnterState(IState state) =>
			_allowReentry || CurrentState != state;
	}
}