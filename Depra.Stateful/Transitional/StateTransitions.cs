// SPDX-License-Identifier: Apache-2.0
// © 2022-2025 Depra <n.melnikov@depra.org>

using System.Collections.Generic;
using Depra.Stateful.Abstract;

namespace Depra.Stateful.Transitional
{
	public sealed class StateTransitions : IStateTransitions
	{
		private readonly List<IStateTransition> _any = new();
		private readonly Dictionary<IState, List<IStateTransition>> _all = new();

		public bool NeedTransition(IState from, out IState to)
		{
			if (!_all.TryGetValue(from, out var all))
			{
				_all[from] = all = new List<IStateTransition>();
			}

			var totalTransitions = _any.Count + all.Count;
			for (var index = 0; index < totalTransitions; index++)
			{
				var transition = index < _any.Count ? _any[index] : all[index - _any.Count];
				if (!transition.ShouldTransition())
				{
					continue;
				}

				to = transition.NextState;
				return true;
			}

			to = null;
			return false;
		}

		public void Add(IState from, IStateTransition transition)
		{
			if (!_all.TryGetValue(from, out var transitions))
			{
				_all[from] = transitions = new List<IStateTransition>();
			}

			transitions.Add(transition);
		}

		public void Remove(IState from, IStateTransition transition)
		{
			if (_all.TryGetValue(from, out var transitions))
			{
				transitions.Remove(transition);
			}
		}

		public void AddAny(IStateTransition transition) => _any.Add(transition);

		public void RemoveAny(IStateTransition transition) => _any.Remove(transition);
	}
}