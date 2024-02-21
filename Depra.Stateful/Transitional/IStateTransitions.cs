// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using Depra.Stateful.Abstract;

namespace Depra.Stateful.Transitional
{
	public interface IStateTransitions
	{
		void AddAny(IStateTransition transition);

		void Add(IState from, IStateTransition transition);

		void RemoveAny(IStateTransition transition);

		void Remove(IState from, IStateTransition transition);

		bool NeedTransition(IState from, out IState to);
	}
}