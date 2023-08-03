// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using Depra.StateMachines.Abstract;

namespace Depra.StateMachines.Transition
{
	public sealed partial class StateTransitionCoordination
	{
		public sealed class ZeroAlloc : IStateTransitionCoordination
		{
			private readonly List<IStateTransition> _anyTransitions = new();
			private readonly Dictionary<Type, List<IStateTransition>> _transitions = new();

			private List<IStateTransition> _currentTransitions = new();

			void IStateTransitionCoordination.Update(IState state)
			{
				_transitions.TryGetValue(state.GetType(), out _currentTransitions);
				_currentTransitions ??= EMPTY_TRANSITIONS;
			}

			bool IStateTransitionCoordination.NeedTransition(out IState nextState)
			{
				var totalTransitions = _anyTransitions.Count + _currentTransitions.Count;
				for (var index = 0; index < totalTransitions; index++)
				{
					var transition = index < _anyTransitions.Count
						? _anyTransitions[index]
						: _currentTransitions[index - _anyTransitions.Count];

					if (transition.ShouldTransition() == false)
					{
						continue;
					}

					nextState = transition.NextState;
					return true;
				}

				nextState = null;
				return false;
			}

			void IStateTransitionCoordination.AddAnyTransition(IStateTransition transition) =>
				_anyTransitions.Add(transition);

			void IStateTransitionCoordination.AddTransition(IState from, IStateTransition transition)
			{
				var sourceStateType = from.GetType();
				if (_transitions.TryGetValue(sourceStateType, out var transitions) == false)
				{
					transitions = new List<IStateTransition>();
					_transitions[sourceStateType] = transitions;
				}

				transitions.Add(transition);
			}
		}
	}
}