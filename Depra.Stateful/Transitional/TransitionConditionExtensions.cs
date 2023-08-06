// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using System;

namespace Depra.Stateful.Transitional
{
	internal static class TransitionConditionExtensions
	{
		public static Func<bool>[] Flatten(this ITransitionCondition[] conditions)
		{
			var result = new Func<bool>[conditions.Length];
			for (var index = 0; index < conditions.Length; index++)
			{
				result[index] = conditions[index].IsMet;
			}

			return result;
		}
	}
}