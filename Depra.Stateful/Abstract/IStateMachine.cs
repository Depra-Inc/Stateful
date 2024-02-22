// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

namespace Depra.Stateful.Abstract
{
	public interface IStateMachine<out TState>
	{
		event StateChangedDelegate StateChanged;

		TState CurrentState { get; }
	}
}