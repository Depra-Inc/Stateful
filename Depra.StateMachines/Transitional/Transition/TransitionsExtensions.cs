// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using System;
using Depra.StateMachines.Abstract;

namespace Depra.StateMachines.Transitional.Transition
{
	public static class TransitionsExtensions
	{
		public static ITransitions At(this ITransitions self, IState from, IState to, params Func<bool>[] conditions)
		{
			self.Add(from, new StateTransition(to, conditions));
			return self;
		}

		public static ITransitions AnyAt(this ITransitions self, IState to, params Func<bool>[] conditions)
		{
			self.AddAny(new StateTransition(to, conditions));
			return self;
		}
	}
}