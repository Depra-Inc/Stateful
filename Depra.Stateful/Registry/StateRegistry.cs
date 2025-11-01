// SPDX-License-Identifier: Apache-2.0
// © 2022-2025 Depra <n.melnikov@depra.org>

using System;
using System.Collections.Generic;
using Depra.Stateful.Abstract;
using Depra.Stateful.Finite;

namespace Depra.Stateful.Registry
{
	public sealed class StateRegistry
	{
		private readonly Dictionary<Type, IState> _states = new();

		public bool IsRegistered(Type type) => _states.ContainsKey(type);

		public IState Get(Type type) => _states.TryGetValue(type, out var state) ? state : new NullFiniteState();

		public bool TryGet(Type type, out IState state)
		{
			if (_states.TryGetValue(type, out var obj))
			{
				state = obj;
				return true;
			}

			state = new NullFiniteState();
			return false;
		}

		public void Register(IState state) => Register(state.GetType(), state);

		public void Register(Type type, IState state)
		{
			if (!_states.TryAdd(type, state))
			{
				throw new InvalidOperationException($"State of type {type} is already registered.");
			}
		}

		public void Unregister(Type type) => _states.Remove(type);
	}
}