// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

namespace Depra.Stateful.Abstract
{
	public interface IStateMachine : IStateMachine<IState> { }

	public interface IStateMachine<TState>
	{
		event StateChangedDelegate StateChanged;

		TState CurrentState { get; }

		void SwitchState(TState to);

		public delegate void StateChangedDelegate(TState state);
	}
}