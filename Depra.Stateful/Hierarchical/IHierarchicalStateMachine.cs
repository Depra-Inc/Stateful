// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System.Threading;
using System.Threading.Tasks;
using Depra.Stateful.Abstract;

namespace Depra.Stateful.Hierarchical
{
	public interface IHierarchicalStateMachine : IStateMachine<IStateNode>
	{
		Task SwitchState(IStateNode to, CancellationToken cancellationToken = default);
	}
}