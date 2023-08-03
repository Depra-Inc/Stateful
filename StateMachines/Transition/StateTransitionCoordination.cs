using System;
using System.Collections.Generic;
using System.Linq;
using Depra.StateMachines.Abstract;

namespace Depra.StateMachines.Transition
{
	public sealed class StateTransitionCoordination : IStateTransitionCoordination
	{
		private static readonly List<IStateTransition> EMPTY_TRANSITIONS = new();

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
			var transitions = _anyTransitions.Concat(_currentTransitions);
			foreach (var transition in transitions)
			{
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