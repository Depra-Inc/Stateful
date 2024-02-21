// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Depra.Stateful.Abstract
{
	public readonly struct NullState : IState
	{
		void IState.Enter() => throw new NullStateException();
		void IState.Exit() => throw new NullStateException();

		Task IState.EnterAsync(CancellationToken cancellationToken) => throw new NullStateException();
		Task IState.ExitAsync(CancellationToken cancellationToken) => throw new NullStateException();

		private sealed class NullStateException : Exception
		{
			public NullStateException() : base("Null state is not allowed.") { }
		}
	}
}