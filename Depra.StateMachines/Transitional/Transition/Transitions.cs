// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using Depra.StateMachines.Abstract;

namespace Depra.StateMachines.Transitional.Transition
{
	public sealed class Transitions : ITransitions
	{
		internal static readonly List<IStateTransition> Empty = new();

		private readonly List<IStateTransition> _any = new();
		private readonly Dictionary<IState, List<IStateTransition>> _all = new();

		IEnumerable<IStateTransition> ITransitions.Any => _any;

		IReadOnlyList<IStateTransition> ITransitions.this[IState state] => _all[state];

		public void AddAny(IStateTransition transition) =>
			_any.Add(transition);

		public void Add(IState from, IStateTransition transition)
		{
			if (_all.TryGetValue(from, out var transitions) == false)
			{
				transitions = new List<IStateTransition>();
				_all[from] = transitions;
			}

			transitions.Add(transition);
		}
	}
}