// SPDX-License-Identifier: Apache-2.0
// © 2022-2025 Depra <n.melnikov@depra.org>

using System.Runtime.CompilerServices;
using Depra.Stateful.Abstract;

namespace Depra.Stateful.Registry
{
	public static class StateRegistryExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsRegistered<TState>(this StateRegistry self) where TState : IState =>
			self.IsRegistered(typeof(TState));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TState Get<TState>(this StateRegistry self) where TState : IState =>
			(TState)self.Get(typeof(TState));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryGet<TState>(this StateRegistry self, out TState state) where TState : IState
		{
			if (self.TryGet(typeof(TState), out var obj))
			{
				state = (TState)obj;
				return true;
			}

			state = default;
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Register<TState>(this StateRegistry self, TState state) where TState : IState =>
			self.Register(typeof(TState), state);
	}
}