// SPDX-License-Identifier: Apache-2.0
// © 2022-2025 Depra <n.melnikov@depra.org>

using System;
using Depra.Stateful.Abstract;

namespace Depra.Stateful.Transitional
{
	public sealed class StateTransition : IStateTransition
	{
		private readonly Func<bool>[] _conditions;

		public StateTransition(IState nextState, ITransitionCondition[] conditions)
		{
			NextState = nextState ?? throw new ArgumentNullException(nameof(nextState));
			_conditions = conditions?.Flatten() ?? throw new ArgumentNullException(nameof(conditions));
		}

		public StateTransition(IState nextState, params Func<bool>[] conditions)
		{
			NextState = nextState ?? throw new ArgumentNullException(nameof(nextState));
			_conditions = conditions ?? throw new ArgumentNullException(nameof(conditions));
		}

		public IState NextState { get; }

		bool IStateTransition.ShouldTransition()
		{
			for (int index = 0, length = _conditions.Length; index < length; index++)
			{
				if (!_conditions[index]())
				{
					return false;
				}
			}

			return true;
		}
	}
}