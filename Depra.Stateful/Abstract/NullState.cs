// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using System;

namespace Depra.Stateful.Abstract
{
	public sealed class NullState : IState
	{
		void IState.Enter() => throw new NullStateException();

		void IState.Exit() => throw new NullStateException();

		private sealed class NullStateException : Exception
		{
			private const string MESSAGE = "Null state is not allowed.";

			public NullStateException() : base(MESSAGE) { }
		}
	}
}