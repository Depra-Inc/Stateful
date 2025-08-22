// SPDX-License-Identifier: Apache-2.0
// © 2022-2025 Depra <n.melnikov@depra.org>

using System;

namespace Depra.Stateful.Finite
{
	public readonly struct NullState : IFiniteState
	{
		void IFiniteState.Enter() => throw new NullStateException();
		void IFiniteState.Exit() => throw new NullStateException();

		private sealed class NullStateException : Exception
		{
			public NullStateException() : base("Null state is not allowed.") { }
		}
	}
}