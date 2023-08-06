// Copyright © 2022-2023 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using Depra.Stateful.Abstract;

namespace Depra.Stateful.Transitional
{
	public interface IStateTransitions
	{
		void AddAny(IStateTransition transition);

		void Add(IState from, IStateTransition transition);

		bool NeedTransition(IState from, out IState to);
	}
}