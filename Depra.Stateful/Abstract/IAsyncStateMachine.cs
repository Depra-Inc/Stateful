// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System.Threading;
using System.Threading.Tasks;

namespace Depra.Stateful.Abstract
{
	public interface IAsyncStateMachine : IAsyncStateMachine<IState> { }

	public interface IAsyncStateMachine<TState>
	{
		event StateChangedDelegate StateChanged;

		TState CurrentState { get; }

		Task SwitchState(TState to, CancellationToken cancellationToken = default);
	}
}