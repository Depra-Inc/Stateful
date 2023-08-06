// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Depra.Stateful.Abstract;

namespace Depra.Stateful.Transitional
{
	public static class StateTransitionsExtensions
	{
		public static IStateTransitions At(this IStateTransitions self, IState from, IState to,
			params Func<bool>[] conditions)
		{
			self.Add(from, new StateTransition(to, conditions));
			return self;
		}

		public static IStateTransitions AnyAt(this IStateTransitions self, IState to, params Func<bool>[] conditions)
		{
			self.AddAny(new StateTransition(to, conditions));
			return self;
		}

		public static IStateTransitions RepeatAt(this IStateTransitions self, IState from, IState to, int count,
			params Func<bool>[] conditions)
		{
			for (var i = 0; i < count; i++)
			{
				self.Add(from, new StateTransition(to, conditions));
			}

			return self;
		}

		public static IStateTransitions RepeatAtAny(this IStateTransitions self, IState to, int count,
			params Func<bool>[] conditions)
		{
			for (var i = 0; i < count; i++)
			{
				self.AddAny(new StateTransition(to, conditions));
			}

			return self;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool NeedTransition(this IEnumerable<IStateTransition> self, out IState to)
		{
			foreach (var transition in self)
			{
				if (!transition.ShouldTransition())
				{
					continue;
				}

				to = transition.NextState;
				return true;
			}

			to = default;
			return false;
		}
	}
}