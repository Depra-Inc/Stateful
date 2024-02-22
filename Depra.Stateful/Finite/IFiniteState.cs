// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using Depra.Stateful.Abstract;

namespace Depra.Stateful.Finite
{
	public interface IFiniteState : IState
	{
		void Enter();

		void Exit();
	}
}