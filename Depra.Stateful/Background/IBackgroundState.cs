// SPDX-License-Identifier: Apache-2.0
// © 2022-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System.Threading;
using System.Threading.Tasks;
using Depra.Stateful.Abstract;

namespace Depra.Stateful.Background
{
	public interface IBackgroundState : IState
	{
		Task Enter(CancellationToken cancellationToken);

		Task Exit(CancellationToken cancellationToken);
	}
}