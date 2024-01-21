// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;

namespace Depra.Stateful.Abstract
{
	public sealed class NullState : IState
	{
		void IState.Enter() => throw new NullStateException();

		void IState.Exit() => throw new NullStateException();

		private sealed class NullStateException : Exception
		{
			public NullStateException() : base("Null state is not allowed.") { }
		}
	}
}