// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.Runtime.CompilerServices;

namespace Depra.Stateful.Transitional
{
	internal static class TransitionConditionExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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