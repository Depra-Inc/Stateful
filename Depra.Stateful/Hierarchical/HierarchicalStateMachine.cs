// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Depra.Stateful.Abstract;

namespace Depra.Stateful.Hierarchical
{
	public sealed class HierarchicalStateMachine : IHierarchicalStateMachine
	{
		private readonly bool _allowReentry;

		public event StateChangedDelegate StateChanged;

		public HierarchicalStateMachine() : this(false) { }

		public HierarchicalStateMachine(bool allowReentry) => _allowReentry = allowReentry;

		public IStateNode CurrentState { get; private set; }

		public async Task SwitchState(IStateNode to, CancellationToken cancellationToken = default)
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
		private bool CanEnterState(IStateNode state) => _allowReentry || CurrentState != state;
	}
}