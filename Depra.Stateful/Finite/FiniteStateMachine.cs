// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System.Runtime.CompilerServices;
using Depra.Stateful.Abstract;

namespace Depra.Stateful.Finite
{
	public sealed class FiniteStateMachine : IFiniteStateMachine
	{
		private readonly bool _allowReentry;

		public event StateChangedDelegate StateChanged;

		public FiniteStateMachine() : this(false) { }

		public FiniteStateMachine(bool allowReentry) => _allowReentry = allowReentry;

		public FiniteStateMachine(IFiniteState startingState, bool allowReentry = false) : this(allowReentry) =>
			SwitchState(startingState);

		public IFiniteState CurrentState { get; private set; }

		public void SwitchState(IFiniteState to)
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool CanEnterState(IState state) => _allowReentry || CurrentState != state;
	}
}