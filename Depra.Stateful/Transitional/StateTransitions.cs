// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System.Collections.Generic;
using Depra.Stateful.Abstract;

namespace Depra.Stateful.Transitional
{
	public sealed class StateTransitions : IStateTransitions
	{
		private static readonly List<IStateTransition> EMPTY = new();

		private readonly List<IStateTransition> _any = new();
		private readonly Dictionary<IState, List<IStateTransition>> _all = new();

		public bool NeedTransition(IState from, out IState to)
		{
			var current = _all.GetValueOrDefault(from, EMPTY);
			var totalTransitions = _any.Count + current.Count;
			for (var index = 0; index < totalTransitions; index++)
			{
				var transition = index < _any.Count ? _any[index] : current[index - _any.Count];
				if (transition.ShouldTransition() == false)
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
			if (_all.TryGetValue(from, out var transitions) == false)
			{
				_all[from] = transitions = EMPTY;
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