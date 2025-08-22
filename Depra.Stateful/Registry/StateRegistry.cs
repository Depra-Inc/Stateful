// SPDX-License-Identifier: Apache-2.0
// © 2022-2025 Depra <n.melnikov@depra.org>

using System;
using System.Collections.Generic;
using Depra.Stateful.Abstract;

namespace Depra.Stateful.Registry
{
	public sealed class StateRegistry
	{
		private readonly Dictionary<Type, object> _states = new();

		public bool IsRegistered(Type type) => _states.ContainsKey(type);

		public IState Get(Type type) => _states.TryGetValue(type, out var state)
			? (IState)state
			: throw new KeyNotFoundException($"State of type {type} is not registered.");

		public bool TryGet(Type type, out IState state)
		{
			if (_states.TryGetValue(type, out var obj))
			{
				state = (IState)obj;
				return true;
			}

			state = null;
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
	}
}