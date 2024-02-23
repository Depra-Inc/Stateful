// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Depra.Stateful.Abstract;

namespace Depra.Stateful.Background
{
	public sealed class BackgroundStateMachine : IBackgroundStateMachine
	{
		private readonly bool _allowReentry;

		public event StateChangedDelegate StateChanged;

		public BackgroundStateMachine() : this(false) { }

		public BackgroundStateMachine(bool allowReentry) => _allowReentry = allowReentry;

		public IBackgroundState CurrentState { get; private set; }

		public async Task SwitchState(IBackgroundState to, CancellationToken cancellationToken = default)
		{
			if (CanEnterState(to) == false)
			{
				return;
			}

			if (CurrentState != null)
			{
				await CurrentState.Exit(cancellationToken);
			}

			if (to != null)
			{
				CurrentState = to;
				await CurrentState.Enter(cancellationToken);
				StateChanged?.Invoke(CurrentState);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool CanEnterState(IBackgroundState state) => _allowReentry || CurrentState != state;
	}
}