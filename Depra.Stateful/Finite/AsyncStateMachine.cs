// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System.Threading;
using System.Threading.Tasks;
using Depra.Stateful.Abstract;

namespace Depra.Stateful.Finite
{
	public sealed class AsyncStateMachine : IAsyncStateMachine
	{
		private readonly bool _allowReentry;

		public event StateChangedDelegate StateChanged;

		public AsyncStateMachine() : this(false) { }

		public AsyncStateMachine(bool allowReentry) => _allowReentry = allowReentry;

		public IState CurrentState { get; private set; }

		public async Task SwitchState(IState to, CancellationToken cancellationToken = default)
		{
			if (CanEnterState(to) == false)
			{
				return;
			}

			if (CurrentState != null)
			{
				await CurrentState.ExitAsync(cancellationToken);
			}

			if (to != null)
			{
				CurrentState = to;
				await CurrentState.EnterAsync(cancellationToken);
				StateChanged?.Invoke(CurrentState);
			}
		}

		private bool CanEnterState(IState state) => _allowReentry || CurrentState != state;
	}
}