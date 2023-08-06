// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Linq;
using Depra.Stateful.Abstract;

namespace Depra.Stateful.Transitional
{
	public sealed class StateTransition : IStateTransition
	{
		private readonly Func<bool>[] _conditions;

		public StateTransition(IState nextState, params ITransitionCondition[] conditions)
			: this(nextState, conditions.Flatten()) { }

		public StateTransition(IState nextState, params Func<bool>[] conditions)
		{
			NextState = nextState ?? throw new ArgumentNullException(nameof(nextState));
			_conditions = conditions ?? throw new ArgumentNullException(nameof(conditions));
		}

		public IState NextState { get; }

		bool IStateTransition.ShouldTransition() =>
			_conditions.All(condition => condition());
	}
}